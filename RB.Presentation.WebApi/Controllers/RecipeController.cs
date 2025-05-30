﻿using Microsoft.AspNetCore.Mvc;
using RB.Application;
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
    public async Task<Recipe?> GetRecipe(string id)
    {
      return await RecipeService.GetRecipe(id);
    }

    [HttpGet]
    [Route("/[controller]/[action]")]
    public async Task<List<Recipe>?> SearchRecipes()
    {
      // TODO: Implement search/filter criteria and params
      /*
         optional string Search
         optional List<string> IngredientNames
         optional bool InSeason
         optional List<Season> Seasons
         optional vegan/vege/lactose/fodmap/gluten/etc. filters
       */

      return await RecipeService.SearchRecipes();
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

      if (recipeParam.Ingredients.Any(i => i.Amount == null))
      {
        throw new ArgumentNullException("Null ingredient amount.");
      }

      return await RecipeService.AddRecipe(recipeParam.Name, recipeParam.Ingredients, recipeParam.Description, recipeParam.Steps, recipeParam.Yield);
    }
  }
}
