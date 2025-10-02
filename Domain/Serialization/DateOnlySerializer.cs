using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace cse325_team7_project.Domain.Serialization;

/// <summary>
/// Serializes DateOnly values as UTC midnight instants.
/// </summary>
public sealed class DateOnlySerializer : StructSerializerBase<DateOnly>
{
    public static readonly DateOnlySerializer Instance = new();

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateOnly value)
    {
        var utcDate = value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        context.Writer.WriteDateTime(BsonUtils.ToMillisecondsSinceEpoch(utcDate));
    }

    public override DateOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var milliseconds = context.Reader.ReadDateTime();
        var utcDate = BsonUtils.ToDateTimeFromMillisecondsSinceEpoch(milliseconds);
        return DateOnly.FromDateTime(DateTime.SpecifyKind(utcDate, DateTimeKind.Utc));
    }
}
