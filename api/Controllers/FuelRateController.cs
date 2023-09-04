using api.Models;
using api.Settings;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FuelRateController : ControllerBase
{
    private readonly IMongoCollection<FuelRate> _collection;

    public FuelRateController(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<FuelRate>("rates");
    }

    [HttpPost("add-rate")]
    public ActionResult<FuelRate> Create(FuelRate userInput)
    {
        FuelRate rate = new FuelRate(
            RateId: null,
            FromDate: userInput.FromDate,
            UntilDate: userInput.UntilDate,
            TypeFuel: userInput.TypeFuel,
            Rate: userInput.Rate
        );

        _collection.InsertOne(rate);

        return rate;

    }
    [HttpGet]
    public ActionResult<IEnumerable<FuelRate>> GetAll()
    {
        List<FuelRate> rates = _collection.Find<FuelRate>(new BsonDocument()).ToList();

        if (!rates.Any())
        {
            return Ok("Your ratelist is empty.");
        }

        return rates;
    }

    [HttpGet("check-date/{userInput}")]
    public ActionResult<FuelRate> GetRate(DateTime userInput)
    {
        FuelRate rate = _collection.Find<FuelRate>(doc => Convert.ToDateTime(doc.FromDate) <= Convert.ToDateTime(userInput) && Convert.ToDateTime(doc.UntilDate) >= Convert.ToDateTime(userInput)).FirstOrDefault();

        if (rate is null)
        {
            return Ok("Your ratelist is empty.");
        }

        return rate;
    }
}