using System.Text.Json;
using cse325_team7_project.Api.DTOs;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace cse325_team7_project.Components.Services;

public class AuthApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigation;

    public AuthApiClient(HttpClient httpClient, NavigationManager navigation)
    {
        _httpClient = httpClient;
        _navigation = navigation;
    }

    public Task<AuthResponseDto> LoginAsync(AuthLoginDto dto) =>
        SendAuthRequest("api/auth/login", dto, "log in");

    public Task<AuthResponseDto> RegisterAsync(AuthRegisterDto dto) =>
        SendAuthRequest("api/auth/register", dto, "register");

    private async Task<AuthResponseDto> SendAuthRequest<TBody>(string relativeUri, TBody body, string actionDescription)
    {
        var response = await _httpClient.PostAsJsonAsync(_navigation.ToAbsoluteUri(relativeUri), body);
        return await ParseAuthResponse(response, actionDescription);
    }

    private static async Task<AuthResponseDto> ParseAuthResponse(HttpResponseMessage response, string actionDescription)
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var message = TryExtractMessage(content) ?? $"Unable to {actionDescription}.";
            throw new ApiException(response.StatusCode, message, content);
        }

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>(JsonOptions);
        if (authResponse is null)
        {
            throw new ApiException(response.StatusCode, $"Empty response while attempting to {actionDescription}.");
        }

        return authResponse;
    }

    private static string? TryExtractMessage(string? rawContent)
    {
        if (string.IsNullOrWhiteSpace(rawContent))
        {
            return null;
        }

        try
        {
            using var doc = JsonDocument.Parse(rawContent);
            if (doc.RootElement.ValueKind == JsonValueKind.Object)
            {
                if (doc.RootElement.TryGetProperty("message", out var messageProp))
                {
                    return messageProp.GetString();
                }

                if (doc.RootElement.TryGetProperty("title", out var titleProp))
                {
                    return titleProp.GetString();
                }

                if (doc.RootElement.TryGetProperty("error", out var errorProp))
                {
                    return errorProp.GetString();
                }
            }
        }
        catch
        {
            // Ignore parse errors and fall back to default messaging.
        }

        return null;
    }
}
