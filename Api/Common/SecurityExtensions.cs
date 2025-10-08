using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using cse325_team7_project.Api.Common;
using cse325_team7_project.Api.Services.Interfaces;
using cse325_team7_project.Domain.Enums;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Security;

public static class SecurityExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal principal, out ObjectId userId)
    {
        userId = ObjectId.Empty;
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return !string.IsNullOrWhiteSpace(value) && ObjectId.TryParse(value, out userId);
    }

    public static ObjectId GetUserIdOrThrow(this ClaimsPrincipal principal)
    {
        if (!principal.TryGetUserId(out var userId))
        {
            throw new UnauthorizedException("Invalid user identity.");
        }
        return userId;
    }

    public static bool IsAdmin(this ClaimsPrincipal principal)
        => principal.IsInRole(nameof(UserRole.Admin));

    public static bool IsSelfOrAdmin(this ClaimsPrincipal principal, ObjectId targetUserId)
    {
        if (principal.IsAdmin()) return true;
        return principal.TryGetUserId(out var userId) && userId == targetUserId;
    }

    public static async Task<bool> OwnsListOrAdmin(this ClaimsPrincipal principal, ObjectId listId, IUserService users)
    {
        if (principal.IsAdmin()) return true;
        if (!principal.TryGetUserId(out var userId)) return false;
        var owner = await users.Get(userId);
        return owner.Lists.Contains(listId);
    }
}

