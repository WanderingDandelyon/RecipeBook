using RB.Application;
using RB.Core.Model;
using RB.Core.Model.Enums;

namespace RB.Tests
{
  public class RecipeServiceSearchTests
  {
    Ingredient Tagliatelle;
    Ingredient Tomato;
    Ingredient ImmortalSquash;
    Ingredient Tofu;
    Ingredient ChickenThigh;
    Recipe PastaDish;
    Recipe TofuStirFry;
    Recipe ChickenStirFry;

    [SetUp]
    public void SetUp() 
    {
      Tagliatelle = new Ingredient
      {
        Name = "Tagliatelle",
        IsVegan = true,
        IsVegetarian = true,
        IsGlutinous = true,
      };
      
      Tomato = new Ingredient
      {
        Name = "Tomato",
        IsVegan = true,
        IsVegetarian = true,
      };
      
      ImmortalSquash = new Ingredient
      {
        Name = "Immortal Squash",
        IsVegan = true,
        IsVegetarian = true,
        MonthsInSeason = new List<Month>
          {
            Month.YearRound,
          },
      };
      
      Tofu = new Ingredient
      {
        Name = "Tofu",
        IsVegan = true,
        IsVegetarian = true,
      };
      
      ChickenThigh = new Ingredient
      {
        Name = "Chicken Thigh",
        IsVegan = false,
        IsVegetarian = false,
      };

      IngredientCatalog.Ingredients.Clear();
      IngredientCatalog.Ingredients = new List<Ingredient>
      {
        Tagliatelle,
        Tomato,
        ImmortalSquash,
        Tofu,
        ChickenThigh
      };

      PastaDish = new Recipe
      {
        Name = "Claude's Pasta Dish",
        Description = "A delicious pasta dish, according to Claude",
        Ingredients =
        {
          new RecipeIngredient
          {
            IngredientId = Tagliatelle.Id,
          },
          new RecipeIngredient
          {
            IngredientId = Tomato.Id,
          },
          new RecipeIngredient
          {
            IngredientId = ImmortalSquash.Id,
          },
        },
      };

      TofuStirFry = new Recipe
      {
        Name = "Tofu Stir Fry",
        Description = "A delicious vegan meal",
        Ingredients =
        {
          new RecipeIngredient
          {
            IngredientId = Tofu.Id,
          },
        },
      };
      
      ChickenStirFry = new Recipe
      {
        Name = "Chicken Stir Fry",
        Description = "A delicious chicken meal",
        Ingredients =
        {
          new RecipeIngredient
          {
            IngredientId = ChickenThigh.Id,
          },
        },
      };

      RecipeBook.Recipes.Clear();
      RecipeBook.Recipes = new List<Recipe>
      {
        PastaDish,
        TofuStirFry,
        ChickenStirFry,
      };
    }

    [Test]
    public async Task SearchRecipes_WithTextSearch_ReturnsMatchingRecipes()
    {
      var results = await RecipeService.SearchRecipes(searchText: "pasta");

      Assert.That(results.Count == 1);
      Assert.That(1.0 == results[0].MatchScore);
      Assert.That(PastaDish.Id.Equals(results[0].Recipe.Id));
    }

    [Test]
    public async Task SearchRecipes_WithIngredients_ReturnsMatchingRecipes()
    {
      var results = await RecipeService.SearchRecipes(ingredientNames: new List<string> { "Tomato" });

      Assert.That(results.Count == 1);
      Assert.That(1.0 == results[0].MatchScore);
      Assert.That(PastaDish.Id.Equals(results[0].Recipe.Id));
    }

    [Test]
    public async Task SearchRecipes_WithSeason_ReturnsInSeasonRecipes()
    {
      var results = await RecipeService.SearchRecipes(inSeason: true);

      Assert.That(results.Count == 1);
      Assert.That(0.5 == results[0].MatchScore);
      Assert.That(PastaDish.Id.Equals(results[0].Recipe.Id));
    }

    [Test]
    public async Task SearchRecipes_WithDietaryRestriction_ReturnsMatchingRecipes()
    {
      var results = await RecipeService.SearchRecipes(isVegan: true);

      Assert.That(results.Count == 2);
      Assert.That(1.0 == results[0].MatchScore);
      Assert.That(1.0 == results[1].MatchScore);
      Assert.That(results.Any(r => r.Recipe.Id.Equals(PastaDish.Id)));
      Assert.That(results.Any(r => r.Recipe.Id.Equals(TofuStirFry.Id)));
    }

    [Test]
    public async Task SearchRecipes_WithMultipleCriteria_ReturnsSortedResults()
    {
      var results = await RecipeService.SearchRecipes(
          searchText: "Stir Fry",
          isVegan: true
      );

      // TODO: Make dietary more into a filter after getting results that can be applied to either
      // a search or getting a full list
      Assert.That(results.Count == 2);
      Assert.That(1.0 == results[0].MatchScore);
      Assert.That(0.5 == results[1].MatchScore);
      Assert.That(TofuStirFry.Id.Equals(results[0].Recipe.Id));
      Assert.That(PastaDish.Id.Equals(results[1].Recipe.Id));
    }
  }
}
