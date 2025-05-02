using Microsoft.AspNetCore.Mvc;
using RB.Application;
using RB.Core.Model;
using RB.Core.Model.Enums;
using RB.Presentation.WebApi.Controllers.ParamObjects;

namespace RB.Presentation.WebApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class IngredientController : ControllerBase
  {
    // TODO: Split logic into Application layer
    private readonly ILogger<IngredientController> _logger;

    public IngredientController(ILogger<IngredientController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    [Route("/[controller]/[action]/{id}")]
    public async Task<Ingredient?> GetIngredient(int id)
    {
      return await IngredientService.GetIngredient(id);
    }

    [HttpGet]
    [Route("/[controller]/[action]")]
    public List<Ingredient>? SearchIngredients()
    {
      // TODO: Implement search/filter criteria and params
      /*
         optional string Search
         optional bool InSeason
         optional List<Season> Seasons
         optional vegan/vege/lactose/fodmap/gluten/etc. filters
       */
      // TODO: Move this to Application
      return IngredientCatalog.Ingredients;
    }

    [HttpPut]
    [Route("/[controller]/[action]")]
    public async Task<int> PutIngredient([FromBody] IngredientParam ingredientParam)
    {
      // TODO: Make this accessible only to admins
      if (ingredientParam == null)
      {
        throw new ArgumentNullException("Cannot add a blank ingredient");
      }
      if (string.IsNullOrWhiteSpace(ingredientParam.Name))
      {
        throw new ArgumentNullException("Ingredient needs a name");
      }

      if (ingredientParam.MonthsInSeason == null || !ingredientParam.MonthsInSeason.Any())
      {
        throw new ArgumentNullException("An ingredient needs to be in season some time or another");
      }

      var measurementUnit = (MeasurementUnit)Enum.ToObject(typeof(MeasurementUnit), ingredientParam.MeasurementUnit ?? 
        throw new ArgumentNullException("Ingredient needs a measurement unit"));

      var isVegan = ingredientParam.IsVegan ?? throw new ArgumentNullException("Must specify whether or not ingredient is vegan");
      var isVegetarian = ingredientParam.IsVegetarian ?? throw new ArgumentNullException("Must specify whether or not ingredient is vegetarian");
      var isDairy = ingredientParam.IsDairy ?? throw new ArgumentNullException("Must specify whether or not ingredient contains lactose");
      var isFodmap = ingredientParam.IsFodmap ?? throw new ArgumentNullException("Must specify whether or not ingredient is a fodmap irritant");
      var isGlutinous = ingredientParam.IsGlutinous ?? throw new ArgumentNullException("Must specify whether or not ingredient contains gluten");

      return await IngredientService.CreateIngredient(ingredientParam.Name, ingredientParam.AlternateNames, measurementUnit,
      ingredientParam.CommonSubstitutions, ingredientParam.MonthsInSeason, isVegan,
      isVegetarian, isDairy, isFodmap, isGlutinous);
    }
  }
}
