using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace api.Models;

public record FuelRate(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)] string? RateId,
    string FromDate,
    string UntilDate,
    string TypeFuel,
    int Rate
);