using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models;

public record CalcCost(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)] string? Id,
    string DateRefueling,
    string DriverName,
    int Amount,
    int Rate,
    string TypeCar,
    int Payable
);