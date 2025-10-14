using cse325_team7_project.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace cse325_team7_project.Domain.ValueObjects;

public record CastMember
{
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public CastRole Role { get; init; }

    public string Name { get; init; } = string.Empty;
}
