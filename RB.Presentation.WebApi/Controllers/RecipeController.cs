using Microsoft.AspNetCore.Mvc;
using RB.Application;
using RB.Core.Model;
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
    public async Task<Recipe?> GetRecipe(string id)
    {
      return await RecipeService.GetRecipe(id);
    }

    [HttpPost]
    [Route("/[controller]/[action]")]
    public async Task<List<RecipeSearchResult>> SearchRecipes([FromBody] RecipeSearchParam searchParam)
    {
        return await RecipeService.SearchRecipes(
            searchParam.SearchText,
            searchParam.IngredientNames,
            searchParam.InSeason,
            searchParam.IsVegan,
            searchParam.IsVegetarian,
            searchParam.IsLactoseFree,
            searchParam.IsGlutenFree);
    }

    // TODO: Add logging
    [HttpPut]
    [Route("/[controller]/[action]")]
    public async Task<string> PutRecipe([FromBody] RecipeParam recipeParam)
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
      if (recipeParam.Ingredients == null || !recipeParam.Ingredients.Any() || recipeParam.Ingredients.All(i => string.IsNullOrWhiteSpace(i.IngredientId)))
      {
        throw new ArgumentNullException("Cannot add a recipe without ingredients");
      }
      if (recipeParam.Steps == null || !recipeParam.Steps.Any() || recipeParam.Steps.All(s => string.IsNullOrWhiteSpace(s))) 
      { 
        throw new ArgumentNullException("Cannot add a recipe without any steps."); 
      }

      if (recipeParam.Ingredients.Any(i => i.Amount == null))
      {
        throw new ArgumentNullException("Null ingredient amount.");
      }

      return await RecipeService.AddRecipe(recipeParam.Name, recipeParam.Ingredients, recipeParam.Description, recipeParam.Steps, recipeParam.Yield);
    }
  }
}
