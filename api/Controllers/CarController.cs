using api.Models;
using api.Settings;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private readonly IMongoCollection<Car> _collection;

    public CarController(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<Car>("cars");
    }

    [HttpPost("add-car")]
    public ActionResult<Car> Create(Car userInput)
    {
        Car car = new Car(
            DriverId: null,
            DriverName: userInput.DriverName,
            TypeCar: userInput.TypeCar,
            DateOfFirstFuel: userInput.DateOfFirstFuel,
            AmountFuel: userInput.AmountFuel,
            TypeFuel: userInput.TypeFuel
        );

        _collection.InsertOne(car);

        return car;
    }

    [HttpGet("get-by-id/{userInput}")]
    public ActionResult<Car> GetById(string userInput)
    {
        Car result = _collection.Find(results => results.DriverId == userInput.Trim()).FirstOrDefault();

        if (result is null)
        {
            return NotFound("No result with this id was found.");
        }

        return result;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Car>> GetAll()
    {
        List<Car> cars = _collection.Find<Car>(new BsonDocument()).ToList();

        if (!cars.Any())
        {
            return Ok("Your carlist is empty.");
        }

        return cars;
    }
}