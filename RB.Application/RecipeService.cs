using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RB.Core.Model;
using RB.Core.Model.Enums;
using RB.Infrastructure.LocalFileStorage;

namespace RB.Application
{
  public class RecipeService
  {
    public static async Task<Recipe?> GetRecipe(string idParam)
    {
      return RecipeBook.Recipes.FirstOrDefault(r => r.Id == idParam);
    }

    public static async Task<List<RecipeSearchResult>> SearchRecipes(string? searchText = null, 
        List<string>? ingredientNames = null, bool? inSeason = null, bool? isVegan = null, 
        bool? isVegetarian = null, bool? isLactoseFree = null, bool? isGlutenFree = null)
    {
      var results = new List<RecipeSearchResult>();
        
      foreach (var recipe in RecipeBook.Recipes)
      {
        // TODO: Split this into a separate Recipe Search Service, and make each criteria separate async functions
        // to be run altogether asynchronously
        double score = 0;
        double matches = 0;
        double criteria = 0;

        var searchIngredientTasks = new List<Task<List<Ingredient?>>>();
        foreach (var ingredientName in ingredientNames)
        {
          searchIngredientTasks.Add(IngredientService.SearchIngredients(ingredientName));
        }

        // Text search in name and description
        if (!string.IsNullOrEmpty(searchText))
        {
          criteria++;
          if (recipe.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            matches += 1;
          if (recipe.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            matches += 0.5;
        }

        await Task.WhenAll(searchIngredientTasks);

        var searchIngredients = new List<Ingredient?>();
        foreach (var task in searchIngredientTasks)
        {
          searchIngredients.Add(task.Result.FirstOrDefault());
        }

        // Ingredient search
        if (ingredientNames != null && ingredientNames.Any())
        {
          criteria++;

          double ingredientMatches = searchIngredients.Count(si => recipe.Ingredients.Any(ri => 
            string.Equals(ri.IngredientId, si.Id)));
                
          matches += ingredientMatches / ingredientNames.Count;
        }

        // Season check
        if (inSeason == true)
        {
          criteria++;
          var recipeIngredients = recipe.Ingredients
            .Select(ri => IngredientCatalog.Ingredients.FirstOrDefault(i => i.Id.Equals(ri.IngredientId)))
            .ToList();

          if (inSeason == true)
          {
            var inSeasonIngredients = recipeIngredients
              .Count(i => i.IsInSeason());
            matches += (double)inSeasonIngredients / recipeIngredients.Count;
          }
        }

        // Dietary restrictions
        if (isVegan == true || isVegetarian == true || isLactoseFree == true || isGlutenFree == true)
        {
          criteria++;
          var recipeIngredients = recipe.Ingredients
            .Select(ri => IngredientCatalog.Ingredients.FirstOrDefault(i => i.Id == ri.IngredientId))
            .Where(i => i != null)
            .ToList();

          bool matchesDietary = true;
          if (isVegan == true && recipeIngredients.Any(i => !i.IsVegan))
            matchesDietary = false;
          else if (isVegetarian == true && recipeIngredients.Any(i => !i.IsVegetarian))
            matchesDietary = false;
          else if (isLactoseFree == true && recipeIngredients.Any(i => !i.IsLactoseFree))
            matchesDietary = false;
          else if (isGlutenFree == true && recipeIngredients.Any(i => i.IsGlutinous))
            matchesDietary = false;

          if (matchesDietary)
            matches += 1;
        }

        // Calculate final score
        double finalScore = criteria > 0 ? matches / criteria : 0;
        results.Add(new RecipeSearchResult(recipe, finalScore));
      }

      return results.OrderByDescending(r => r.MatchScore).ToList();
    }

    public static async Task<string> AddRecipe(string nameParam, List<(string IngredientId, double? Amount)> ingredientsParam,
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
      return newRecipe.Id;

      /*
      var modifiedRecipe = RecipeBook.Recipes.FirstOrDefault(r => r.Id == recipeParam.RecipeId);
      modifiedRecipe = newRecipe;
      JsonFileService.WriteRecipeBook();
      return modifiedRecipe.Id;
      */
    }
  }
}
