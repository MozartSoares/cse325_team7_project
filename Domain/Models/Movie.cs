using cse325_team7_project.Domain.Enums;
using cse325_team7_project.Domain.Serialization;
using cse325_team7_project.Domain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace cse325_team7_project.Domain.Models;

[BsonIgnoreExtraElements]
public class Movie : BaseDocument
{
    [BsonElement("title")]
    [BsonRequired]
    public string Title { get; set; } = string.Empty;

    [BsonElement("releaseDate")]
    [BsonSerializer(typeof(DateOnlySerializer))]
    [BsonRequired]
    public DateOnly ReleaseDate { get; set; }

    [BsonElement("genre")]
    [BsonRepresentation(BsonType.String)]
    [BsonRequired]
    public Genre Genre { get; set; }

    [BsonElement("description")]
    [BsonRequired]
    public string Description { get; set; } = string.Empty;

    [BsonElement("studio")]
    public string Studio { get; set; } = string.Empty;

    [BsonElement("cast")]
    public List<CastMember> Cast { get; set; } = new();

    [BsonElement("image")]
    [BsonRequired]
    public string Image { get; set; } = string.Empty;

    [BsonElement("thumbnailImage")]
    [BsonRequired]
    public string ThumbnailImage { get; set; } = string.Empty;

    [BsonElement("budget")]
    [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
    [BsonRequired]
    public decimal Budget { get; set; }
}
