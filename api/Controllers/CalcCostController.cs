using api.Models;
using api.Settings;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalcCostController : ControllerBase
{
    private readonly IMongoCollection<CalcCost> _collection;

    public CalcCostController(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<CalcCost>("results");
    }

    [HttpPost("add-result")]
    public ActionResult<CalcCost> Create(CalcCost userInput)
    {
        CalcCost result = new CalcCost(
            Id: null,
            DriverName: userInput.DriverName,
            DateRefueling: userInput.DateRefueling,
            Amount: userInput.Amount,
            Rate: userInput.Rate,
            TypeCar: userInput.TypeCar,
            Payable: userInput.Payable
        );

        _collection.InsertOne(result);

        return result;
    }

    [HttpGet("get-by-id/{userInput}")]
    public ActionResult<List<CalcCost>> GetById(string userInput)
    {
        List<CalcCost> results = _collection.Find(results => results.TypeCar == userInput.Trim()).ToList();

        if (results is null)
        {
            return NotFound("No result with this id was found.");
        }

        return results;
    }

    [HttpGet]
    public ActionResult<dynamic> GetAll()
    {
        var cars = _collection.Aggregate().Group(
            doc => doc.TypeCar,
            group => new
            {
                typeCar = group.Key,
                totalAmount = group.Sum(y => y.Amount),
                totalPay = group.Sum(y => y.Payable),
            }
        ).ToList();

        if (!cars.Any())
        {
            return Ok("Your carslist is empty.");
        }

        return cars;
    }
}
