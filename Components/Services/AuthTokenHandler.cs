using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using cse325_team7_project.Api.DTOs;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace cse325_team7_project.Components.Services;

/// <summary>
/// Appends the bearer token from <see cref="AuthStateService"/> to outgoing requests when available.
/// </summary>
public class AuthTokenHandler : DelegatingHandler
{
    private readonly AuthStateService _authState;
    private readonly ILogger<AuthTokenHandler> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly NavigationManager _navigation;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public AuthTokenHandler(
        AuthStateService authState,
        ILogger<AuthTokenHandler> logger,
        IHttpClientFactory httpClientFactory,
        NavigationManager navigation)
    {
        _authState = authState;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _navigation = navigation;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await EnsureTokenUpToDateAsync(cancellationToken);

        var token = _authState.AccessToken;
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _logger.LogTrace("Attached bearer token to {Method} {Uri}", request.Method, request.RequestUri);
        }
        else
        {
            _logger.LogTrace("No bearer token available for {Method} {Uri}", request.Method, request.RequestUri);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if ((response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden) && _authState.HasAuth)
        {
            _logger.LogInformation("Received {Status} for {Method} {Uri}. Attempting forced token refresh.", (int)response.StatusCode, request.Method, request.RequestUri);
            var refreshed = await _authState.TryRefreshAsync(() => RefreshAccessTokenAsync(cancellationToken), force: true);
            if (refreshed)
            {
                _logger.LogInformation("Token refreshed after {Status} response. Retrying once.", (int)response.StatusCode);
                response.Dispose();
                using var retry = await CloneRequestAsync(request, includeAuthHeader: true);
                return await base.SendAsync(retry, cancellationToken);
            }
        }

        return response;
    }

    private async Task EnsureTokenUpToDateAsync(CancellationToken cancellationToken)
    {
        await _authState.EnsureInitializedAsync();

        if (!_authState.HasAuth)
        {
            return;
        }

        var expiresAt = _authState.ExpiresAt;
        if (!expiresAt.HasValue)
        {
            return;
        }

        if (expiresAt.Value <= DateTime.UtcNow)
        {
            _logger.LogDebug("Access token already expired. Attempting refresh.");
            await _authState.TryRefreshAsync(() => RefreshAccessTokenAsync(cancellationToken), force: true);
        }
        else if (_authState.IsTokenExpiringSoon())
        {
            _logger.LogTrace("Access token expiring soon. Attempting proactive refresh.");
            await _authState.TryRefreshAsync(() => RefreshAccessTokenAsync(cancellationToken));
        }
    }

    private async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage original, bool includeAuthHeader)
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri);

        // Copy content if present
        if (original.Content is not null)
        {
            var bytes = await original.Content.ReadAsByteArrayAsync();
            clone.Content = new ByteArrayContent(bytes);
            // Copy content headers
            foreach (var h in original.Content.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }
        }

        // Copy headers (except Authorization, which we reapply)
        foreach (var header in original.Headers)
        {
            if (!string.Equals(header.Key, "Authorization", StringComparison.OrdinalIgnoreCase))
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        if (includeAuthHeader)
        {
            var token = _authState.AccessToken;
            if (!string.IsNullOrWhiteSpace(token))
            {
                clone.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return clone;
    }

    private async Task<AuthResponseDto?> RefreshAccessTokenAsync(CancellationToken cancellationToken)
    {
        var currentToken = _authState.AccessToken;
        if (string.IsNullOrWhiteSpace(currentToken))
        {
            return null;
        }

        var client = _httpClientFactory.CreateClient("AuthRefresh");
        var response = await client.PostAsJsonAsync(
            _navigation.ToAbsoluteUri("api/auth/refresh"),
            new AuthRefreshDto(currentToken),
            JsonOptions,
            cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Token refresh request failed with status {StatusCode}", response.StatusCode);
            return null;
        }

        try
        {
            var refreshed = await response.Content.ReadFromJsonAsync<AuthResponseDto>(JsonOptions, cancellationToken);
            if (refreshed is null)
            {
                _logger.LogWarning("Token refresh succeeded but body was empty.");
            }

            return refreshed;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to deserialize token refresh response.");
            return null;
        }
    }
}
