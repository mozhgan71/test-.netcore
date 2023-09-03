using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace api.Models;

public record Car(
   [property: BsonId, BsonRepresentation(BsonType.ObjectId)] string? DriverId,
   string DriverName,
   string TypeCar,
   string DateOfFirstFuel,
   int AmountFuel,
   string TypeFuel
);