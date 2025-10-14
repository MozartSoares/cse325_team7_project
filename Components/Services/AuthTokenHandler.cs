using System.Net.Http.Headers;

namespace cse325_team7_project.Components.Services;

/// <summary>
/// Appends the bearer token from <see cref="AuthStateService"/> to outgoing requests when available.
/// </summary>
public class AuthTokenHandler : DelegatingHandler
{
    private readonly AuthStateService _authState;

    public AuthTokenHandler(AuthStateService authState)
    {
        _authState = authState;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await _authState.EnsureInitializedAsync();
        var token = _authState.AccessToken;
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
