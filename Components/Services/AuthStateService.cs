using System;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using cse325_team7_project.Api.DTOs;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;

namespace cse325_team7_project.Components.Services;

/// <summary>
/// Tracks the authenticated user for the Blazor UI and persists it in browser storage.
/// </summary>
public class AuthStateService
{
    private const string StorageKey = "moviehub7-auth";
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<AuthStateService> _logger;
    private readonly SemaphoreSlim _refreshLock = new(1, 1);
    private readonly TimeSpan _defaultRefreshLeadTime = TimeSpan.FromMinutes(1);

    private bool _initialized;
    private AuthResponseDto? _auth;

    public AuthStateService(IJSRuntime jsRuntime, ILogger<AuthStateService> logger)
    {
        _jsRuntime = jsRuntime;
        _logger = logger;
    }

    public event Action? AuthStateChanged;

    public bool IsAuthenticated => _auth is not null && _auth.ExpiresAt > DateTime.UtcNow;

    public bool HasAuth => _auth is not null;

    public UserResponseDto? CurrentUser => _auth?.User;

    public string? AccessToken => _auth?.AccessToken;

    public DateTime? ExpiresAt => _auth?.ExpiresAt;

    public async Task<bool> ApplyAuthorizationAsync(HttpClient client)
    {
        await EnsureInitializedAsync();
        var token = AccessToken;
        if (string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization = null;
            return false;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return true;
    }

    public bool IsTokenExpiringSoon(TimeSpan? threshold = null)
    {
        if (_auth is null)
        {
            return false;
        }

        var window = threshold ?? _defaultRefreshLeadTime;
        return _auth.ExpiresAt <= DateTime.UtcNow.Add(window);
    }

    /// <summary>
    /// Loads any persisted auth record from browser storage. Safe to call multiple times.
    /// </summary>
    public async Task EnsureInitializedAsync()
    {
        if (_initialized)
        {
            return;
        }

        string? stored;
        try
        {
            stored = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        }
        catch (InvalidOperationException)
        {
            // JS interop not available yet; try again on the next attempt.
            return;
        }

        if (string.IsNullOrWhiteSpace(stored))
        {
            _auth = null;
            _initialized = true;
            NotifyStateChanged();
            return;
        }

        try
        {
            var auth = JsonSerializer.Deserialize<AuthResponseDto>(stored, JsonOptions);
            if (auth is not null)
            {
                _auth = auth;
            }
            else
            {
                _logger.LogWarning("Stored auth payload was null – clearing local storage copy.");
                await RemoveStoredAuthAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to deserialize stored auth payload. Clearing local storage copy.");
            await RemoveStoredAuthAsync();
        }

        _initialized = true;
        NotifyStateChanged();
    }

    public async Task SetAuthAsync(AuthResponseDto auth)
    {
        _auth = auth;
        await WriteAuthToStorageAsync(auth);
        _initialized = true;
        NotifyStateChanged();
    }

    public async Task<bool> UpdateUserAsync(UserResponseDto updatedUser)
    {
        if (updatedUser is null) throw new ArgumentNullException(nameof(updatedUser));

        await EnsureInitializedAsync();
        if (_auth is null)
        {
            return false;
        }

        _auth = _auth with { User = updatedUser };
        await WriteAuthToStorageAsync(_auth);
        _initialized = true;
        NotifyStateChanged();
        return true;
    }

    public async Task ClearAsync()
    {
        _auth = null;
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKey);
        _initialized = true;
        NotifyStateChanged();
    }

    public async Task<bool> TryRefreshAsync(Func<Task<AuthResponseDto?>> refreshFactory, bool force = false)
    {
        if (refreshFactory is null) throw new ArgumentNullException(nameof(refreshFactory));

        await EnsureInitializedAsync();
        if (_auth is null)
        {
            return false;
        }

        var threshold = _defaultRefreshLeadTime;
        if (!force && _auth.ExpiresAt > DateTime.UtcNow.Add(threshold))
        {
            return true;
        }

        await _refreshLock.WaitAsync();
        try
        {
            await EnsureInitializedAsync();
            if (_auth is null)
            {
                return false;
            }

            if (!force && _auth.ExpiresAt > DateTime.UtcNow.Add(threshold))
            {
                return true;
            }

            try
            {
                var refreshed = await refreshFactory();
                if (refreshed is null)
                {
                    _logger.LogWarning("Token refresh delegate returned null. Clearing auth state.");
                    await ClearAsync();
                    return false;
                }

                await SetAuthAsync(refreshed);
                _logger.LogInformation("Access token refreshed successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token refresh delegate threw an exception. Clearing auth state.");
                await ClearAsync();
                return false;
            }
        }
        finally
        {
            _refreshLock.Release();
        }
    }

    private async Task RemoveStoredAuthAsync()
    {
        _auth = null;
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKey);
    }

    private Task WriteAuthToStorageAsync(AuthResponseDto auth)
    {
        var payload = JsonSerializer.Serialize(auth, JsonOptions);
        return _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, payload).AsTask();
    }

    private void NotifyStateChanged()
    {
        AuthStateChanged?.Invoke();
    }
}
