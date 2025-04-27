using Microsoft.AspNetCore.Mvc;
using RB.Core.Model;
using RB.Core.Model.Enums;
using RB.Infrastructure.LocalFileStorage;
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
    public Ingredient? GetIngredient(int id)
    {
      return IngredientCatalog.Ingredients.FirstOrDefault(i => i.Id == id);
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
    public int PutIngredient([FromBody] IngredientParam ingredientParam)
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

      var commonSubstitutions = new List<RecipeIngredient>();
      foreach (var sub in ingredientParam.CommonSubstitutions)
      {
        commonSubstitutions.Add(new RecipeIngredient
        {
          IngredientId = sub.Id,
          MeasurementAmount = sub.Amount,
        });
      }

      var monthsInSeason = new List<Month>();
      foreach (var month in ingredientParam.MonthsInSeason)
      {
        monthsInSeason.Add((Month)Enum.ToObject(typeof(Month), month));
      }

      var newIngredient = new Ingredient
      {
        Name = ingredientParam.Name,
        AlternateNames = ingredientParam.AlternateNames ?? new List<string>(),
        MeasurementUnit = (MeasurementUnit)Enum.ToObject(typeof(MeasurementUnit), ingredientParam.MeasurementUnit ?? throw new ArgumentNullException("Ingredient needs a measurement unit")),
        CommonSubstitutions = commonSubstitutions,
        MonthsInSeason = monthsInSeason,
        IsVegan = ingredientParam.IsVegan ?? throw new ArgumentNullException("Must specify whether or not ingredient is vegan"),
        IsVegetarian = ingredientParam.IsVegetarian ?? throw new ArgumentNullException("Must specify whether or not ingredient is vegetarian"),
        IsDairy = ingredientParam.IsDairy ?? throw new ArgumentNullException("Must specify whether or not ingredient contains lactose"),
        IsFodmap = ingredientParam.IsFodmap ?? throw new ArgumentNullException("Must specify whether or not ingredient is a fodmap irritant"),
        IsGlutinous = ingredientParam.IsGlutinous ?? throw new ArgumentNullException("Must specify whether or not ingredient contains gluten")
      };

      if (ingredientParam.IngredientId == null)
      {
        IngredientCatalog.Ingredients.Add(newIngredient);
        JsonFileService.WriteIngredientCatalog();
        return IngredientCatalog.Ingredients.Last().Id;
      }

      var modifiedIngredient = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Id == ingredientParam.IngredientId);
      modifiedIngredient = newIngredient;
      JsonFileService.WriteIngredientCatalog();
      return modifiedIngredient.Id;
    }
  }
}
