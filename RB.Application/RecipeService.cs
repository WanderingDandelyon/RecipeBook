using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RB.Core.Model;
using RB.Infrastructure.LocalFileStorage;

namespace RB.Application
{
  public class RecipeService
  {
    public static async Task<Recipe?> GetRecipe(int idParam)
    {
      return RecipeBook.Recipes.FirstOrDefault(r => r.Id == idParam);
    }

    public static async Task<int> AddRecipe(string nameParam, List<(int IngredientId, double? Amount)> ingredientsParam,
      string descriptionParam, List<string> stepsParam, int? yieldParam)
    {
      var recipeIgredients = new List<RecipeIngredient>();
      foreach (var ingredient in ingredientsParam)
      {
        recipeIgredients.Add(
          new RecipeIngredient
          {
            IngredientId = ingredient.IngredientId,
            MeasurementAmount = ingredient.Amount ?? throw new ArgumentNullException("Null ingredient amount.")
          });
      }

      var newRecipe = new Recipe
      {
        Name = nameParam,
        Description = descriptionParam ?? string.Empty,
        Ingredients = recipeIgredients,
        Steps = stepsParam,
        Yield = yieldParam
      };

      RecipeBook.Recipes.Add(newRecipe);
      JsonFileService.WriteRecipeBook();
      return RecipeBook.Recipes.Last().Id; // TODO: This is not threadsafe. See ingredient creation, also. Ticket 24

      /*
      var modifiedRecipe = RecipeBook.Recipes.FirstOrDefault(r => r.Id == recipeParam.RecipeId);
      modifiedRecipe = newRecipe;
      JsonFileService.WriteRecipeBook();
      return modifiedRecipe.Id;
      */
    }
  }
}
