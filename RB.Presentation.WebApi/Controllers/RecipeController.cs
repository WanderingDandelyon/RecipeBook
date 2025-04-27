using Microsoft.AspNetCore.Mvc;
using RB.Core.Model;
using RB.Infrastructure.LocalFileStorage;
using RB.Presentation.WebApi.Controllers.ParamObjects;

namespace RB.Presentation.WebApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class RecipeController : ControllerBase
  {
    // TODO: Split logic into Application layer
    private readonly ILogger<RecipeController> _logger;

    public RecipeController(ILogger<RecipeController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    [Route("/[controller]/[action]/{id}")]
    public Recipe? GetRecipe(int id)
    {
      return RecipeBook.Recipes.FirstOrDefault(r => r.Id == id);
    }

    [HttpGet]
    [Route("/[controller]/[action]")]
    public List<Recipe>? SearchRecipes()
    {
      // TODO: Implement search/filter criteria and params
      /*
         optional string Search
         optional List<string> IngredientNames
         optional bool InSeason
         optional List<Season> Seasons
         optional vegan/vege/lactose/fodmap/gluten/etc. filters
       */
      // TODO: Move this to Application
      return RecipeBook.Recipes;
    }

    // TODO: Add logging
    [HttpPut]
    [Route("/[controller]/[action]")]
    public int PutRecipe([FromBody] RecipeParam recipeParam)
    {
      // TODO: Do I want separation from Model objects and the objects passed into and out of API
      // or just keep the same? Kinda doing both right now
      if (recipeParam == null)
      {
        throw new ArgumentNullException("Cannot add a blank recipe");
      }
      if (string.IsNullOrWhiteSpace(recipeParam.Name))
      {
        throw new ArgumentNullException("Recipe needs a name");
      }
      // TODO: Account for a List of Empty values
      if (recipeParam.Ingredients == null || !recipeParam.Ingredients.Any())
      {
        throw new ArgumentNullException("Cannot add a recipe without ingredients");
      }
      // TODO: Account for a List of Empty values
      if (recipeParam.Steps == null || !recipeParam.Steps.Any()) 
      { 
        throw new ArgumentNullException("Cannot add a recipe without any steps."); 
      }

      var recipeIgredients = new List<RecipeIngredient>();
      foreach (var ingredient in recipeParam.Ingredients) {
        recipeIgredients.Add(
          new RecipeIngredient 
          { 
            IngredientId = ingredient.IngredientId, 
            MeasurementAmount = ingredient.Amount ?? throw new ArgumentNullException("Null ingredient amount.") 
          });
      }

      var newRecipe = new Recipe
      {
        Name = recipeParam.Name,
        Description = recipeParam.Description ?? string.Empty,
        Ingredients = recipeIgredients,
        Steps = recipeParam.Steps,
        Yield = recipeParam.Yield
      };

      if (recipeParam.RecipeId == null) 
      {
        RecipeBook.Recipes.Add(newRecipe);
        JsonFileService.WriteRecipeBook();
        return RecipeBook.Recipes.Last().Id;
      }

      var modifiedRecipe = RecipeBook.Recipes.FirstOrDefault(r => r.Id == recipeParam.RecipeId);
      modifiedRecipe = newRecipe;
      JsonFileService.WriteRecipeBook();
      return modifiedRecipe.Id;
    }
  }
}
