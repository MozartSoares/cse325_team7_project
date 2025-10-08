namespace cse325_team7_project.Api.Options;

/// <summary>
/// Strongly typed configuration object bound to the <c>Jwt</c> section.
/// </summary>
public class JwtOptions
{
    public string Issuer { get; init; } = "local";

    public string Audience { get; init; } = "local";

    public string Key { get; init; } = string.Empty;

    public int AccessTokenMinutes { get; init; } = 60;
}
