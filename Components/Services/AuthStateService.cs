using System.Text.Json;
using cse325_team7_project.Api.DTOs;
using Microsoft.JSInterop;

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

    private bool _initialized;
    private AuthResponseDto? _auth;

    public AuthStateService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public event Action? AuthStateChanged;

    public bool IsAuthenticated => _auth is not null && _auth.ExpiresAt > DateTime.UtcNow;

    public UserResponseDto? CurrentUser => _auth?.User;

    public string? AccessToken => _auth?.AccessToken;

    public DateTime? ExpiresAt => _auth?.ExpiresAt;

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
            if (auth is not null && auth.ExpiresAt > DateTime.UtcNow)
            {
                _auth = auth;
            }
            else
            {
                await RemoveStoredAuthAsync();
            }
        }
        catch
        {
            await RemoveStoredAuthAsync();
        }

        _initialized = true;
        NotifyStateChanged();
    }

    public async Task SetAuthAsync(AuthResponseDto auth)
    {
        _auth = auth;
        var payload = JsonSerializer.Serialize(auth, JsonOptions);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, payload);
        _initialized = true;
        NotifyStateChanged();
    }

    public async Task ClearAsync()
    {
        _auth = null;
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKey);
        _initialized = true;
        NotifyStateChanged();
    }

    private async Task RemoveStoredAuthAsync()
    {
        _auth = null;
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKey);
    }

    private void NotifyStateChanged()
    {
        AuthStateChanged?.Invoke();
    }
}
