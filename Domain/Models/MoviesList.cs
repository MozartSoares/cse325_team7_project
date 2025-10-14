using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace cse325_team7_project.Domain.Models;

[BsonIgnoreExtraElements]
public class MoviesList : BaseDocument
{
    [BsonElement("title")]
    [BsonRequired]
    public string Title { get; set; } = string.Empty;

    [BsonElement("movies")]
    public List<ObjectId> Movies { get; set; } = new();
}
