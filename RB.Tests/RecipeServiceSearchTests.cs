using RB.Core.Model;
using RB.Core.Model.Enums;

namespace RB.Tests
{
  public class RecipeServiceSearchTests
  {
    [Fact]
    public async Task SearchRecipes_WithTextSearch_ReturnsMatchingRecipes()
    {
      // Arrange
      var testRecipe = new Recipe
      {
        Name = "Test Pasta Recipe",
        Description = "A delicious pasta dish"
      };
      RecipeBook.Recipes.Clear();
      RecipeBook.Recipes.Add(testRecipe);

      // Act
      var results = await RecipeService.SearchRecipes(searchText: "pasta");

      // Assert
      Assert.Single(results);
      Assert.Equal(1.0, results[0].MatchScore);
      Assert.Equal(testRecipe.Id, results[0].Recipe.Id);
    }

    [Fact]
    public async Task SearchRecipes_WithIngredients_ReturnsMatchingRecipes()
    {
      // Arrange
      var ingredient = new Ingredient { Name = "Tomato" };
      IngredientCatalog.Ingredients.Clear();
      IngredientCatalog.Ingredients.Add(ingredient);

      var testRecipe = new Recipe
      {
        Name = "Test Recipe",
        Ingredients = new List<RecipeIngredient>
                {
                    new RecipeIngredient { IngredientId = ingredient.Id }
                }
      };
      RecipeBook.Recipes.Clear();
      RecipeBook.Recipes.Add(testRecipe);

      // Act
      var results = await RecipeService.SearchRecipes(ingredientNames: new List<string> { "Tomato" });

      // Assert
      Assert.Single(results);
      Assert.Equal(1.0, results[0].MatchScore);
      Assert.Equal(testRecipe.Id, results[0].Recipe.Id);
    }

    [Fact]
    public async Task SearchRecipes_WithSeason_ReturnsInSeasonRecipes()
    {
      // Arrange
      var ingredient = new Ingredient
      {
        Name = "Summer Squash",
        SeasonalityStart = Month.June,
        SeasonalityEnd = Month.August
      };
      IngredientCatalog.Ingredients.Clear();
      IngredientCatalog.Ingredients.Add(ingredient);

      var testRecipe = new Recipe
      {
        Name = "Summer Recipe",
        Ingredients = new List<RecipeIngredient>
                {
                    new RecipeIngredient { IngredientId = ingredient.Id }
                }
      };
      RecipeBook.Recipes.Clear();
      RecipeBook.Recipes.Add(testRecipe);

      // Act
      var results = await RecipeService.SearchRecipes(seasons: new List<Month> { Month.July });

      // Assert
      Assert.Single(results);
      Assert.Equal(1.0, results[0].MatchScore);
      Assert.Equal(testRecipe.Id, results[0].Recipe.Id);
    }

    [Fact]
    public async Task SearchRecipes_WithDietaryRestriction_ReturnsMatchingRecipes()
    {
      // Arrange
      var veganIngredient = new Ingredient
      {
        Name = "Tofu",
        IsVegan = true,
        IsVegetarian = true
      };
      IngredientCatalog.Ingredients.Clear();
      IngredientCatalog.Ingredients.Add(veganIngredient);

      var testRecipe = new Recipe
      {
        Name = "Vegan Recipe",
        Ingredients = new List<RecipeIngredient>
                {
                    new RecipeIngredient { IngredientId = veganIngredient.Id }
                }
      };
      RecipeBook.Recipes.Clear();
      RecipeBook.Recipes.Add(testRecipe);

      // Act
      var results = await RecipeService.SearchRecipes(isVegan: true);

      // Assert
      Assert.Single(results);
      Assert.Equal(1.0, results[0].MatchScore);
      Assert.Equal(testRecipe.Id, results[0].Recipe.Id);
    }

    [Fact]
    public async Task SearchRecipes_WithMultipleCriteria_ReturnsSortedResults()
    {
      // Arrange
      var veganIngredient = new Ingredient
      {
        Name = "Tofu",
        IsVegan = true,
        IsVegetarian = true
      };
      var nonVeganIngredient = new Ingredient
      {
        Name = "Chicken",
        IsVegan = false,
        IsVegetarian = false
      };
      IngredientCatalog.Ingredients.Clear();
      IngredientCatalog.Ingredients.AddRange(new[] { veganIngredient, nonVeganIngredient });

      var veganRecipe = new Recipe
      {
        Name = "Tofu Stir Fry",
        Description = "A delicious vegan meal",
        Ingredients = new List<RecipeIngredient>
                {
                    new RecipeIngredient { IngredientId = veganIngredient.Id }
                }
      };
      var nonVeganRecipe = new Recipe
      {
        Name = "Chicken Stir Fry",
        Description = "A delicious meal",
        Ingredients = new List<RecipeIngredient>
                {
                    new RecipeIngredient { IngredientId = nonVeganIngredient.Id }
                }
      };
      RecipeBook.Recipes.Clear();
      RecipeBook.Recipes.AddRange(new[] { veganRecipe, nonVeganRecipe });

      // Act
      var results = await RecipeService.SearchRecipes(
          searchText: "Stir Fry",
          isVegan: true
      );

      // Assert
      Assert.Equal(2, results.Count);
      Assert.Equal(veganRecipe.Id, results[0].Recipe.Id);
      Assert.True(results[0].MatchScore > results[1].MatchScore);
    }
  }
}
