using RB.Core.Model.Enums;
using RB.Core.Model;
using RB.Infrastructure.LocalFileStorage;

namespace RB.Application
{
  public class IngredientService
  {
    public static async Task<Ingredient?> GetIngredient(string idParam)
    {
      return IngredientCatalog.Ingredients.FirstOrDefault(i => i.Id.Equals(idParam));
    }

    public static async Task<List<Ingredient>> SearchIngredients()
    {
      return IngredientCatalog.Ingredients;
    }

    public static async Task<Ingredient?> SearchIngredient(string searchTokenParam)
    {
      var result = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Name.ToLower().Equals(searchTokenParam.ToLower()));
      if (result != null)
      {
        return result;
      }

      result = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Name.Contains(searchTokenParam.ToLower()));
      if (result != null)
      {
        return result;
      }

      return IngredientCatalog.Ingredients.FirstOrDefault(i => checkAlternateNames(i, searchTokenParam));
    }

    private static bool checkAlternateNames(Ingredient ingredientParam, string searchTermParam)
    {
      foreach (string altName in ingredientParam.AlternateNames)
      {
        if (altName.ToLower().Contains(searchTermParam.ToLower()))
        {
          return true;
        }
      }

      return false;
    }

    public static async Task<string> CreateIngredient(string nameParam, List<string> altNamesParam, MeasurementUnit measurementUnitParam, 
      List<(string Id, double Amount)> commonSubstitutionsParam, List<int> monthsInSeasonParam, bool isVeganParam, 
      bool isVegetarianParam, bool isDairyParam, bool isFodmapParam, bool isGlutinousParam)
    {
      var commonSubstitutions = new List<RecipeIngredient>();
      foreach (var sub in commonSubstitutionsParam)
      {
        commonSubstitutions.Add(new RecipeIngredient
        {
          IngredientId = sub.Id,
          MeasurementAmount = sub.Amount,
        });
      }

      var monthsInSeason = new List<Month>();
      foreach (var month in monthsInSeasonParam)
      {
        monthsInSeason.Add((Month)Enum.ToObject(typeof(Month), month));
      }

      var newIngredient = new Ingredient
      {
        Name = nameParam,
        AlternateNames = altNamesParam ?? new List<string>(),
        MeasurementUnit = measurementUnitParam,//MeasurementUnit = (MeasurementUnit)Enum.ToObject(typeof(MeasurementUnit), ingredientParam.MeasurementUnit ?? throw new ArgumentNullException("Ingredient needs a measurement unit")),
        CommonSubstitutions = commonSubstitutions,
        MonthsInSeason = monthsInSeason,
        IsVegan = isVeganParam,// ?? throw new ArgumentNullException("Must specify whether or not ingredient is vegan"),
        IsVegetarian = isVegetarianParam,// ?? throw new ArgumentNullException("Must specify whether or not ingredient is vegetarian"),
        IsDairy = isDairyParam,// ?? throw new ArgumentNullException("Must specify whether or not ingredient contains lactose"),
        IsFodmap = isFodmapParam,// ?? throw new ArgumentNullException("Must specify whether or not ingredient is a fodmap irritant"),
        IsGlutinous = isGlutinousParam,// ?? throw new ArgumentNullException("Must specify whether or not ingredient contains gluten")
      };

      IngredientCatalog.Ingredients.Add(newIngredient);
      JsonFileService.WriteIngredientCatalog();
      return newIngredient.Id;

      /*
      var modifiedIngredient = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Id == ingredientIdParam);
      modifiedIngredient = newIngredient;
      JsonFileService.WriteIngredientCatalog();
      return modifiedIngredient.Id;
      */
    }
  }
}
