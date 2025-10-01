using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MovieHub7.Models
{
    public class CastType
    {
        [BsonElement("role")]
        public string Role { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        // Use DateTime para data de lançamento
        [BsonElement("release_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime ReleaseDate { get; set; }

        [BsonElement("genre")]
        public string Genre { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty;

        [BsonElement("cast")]
        public List<CastType> Cast { get; set; } = new();


        [BsonElement("image")]
        public string? ImageBase64 { get; set; }

        [BsonElement("budget")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Budget { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
