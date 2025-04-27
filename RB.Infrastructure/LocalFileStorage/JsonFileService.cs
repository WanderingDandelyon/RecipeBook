using System.Text.Json;
using RB.Core.Model;

namespace RB.Infrastructure.LocalFileStorage
{
  // TODO: Make this more generic
  // TODO: Move to a SQL Database?
  public class JsonFileService
  {
    private static string recipeBookFileLocation = Directory.GetCurrentDirectory() + @"\RecipeBook.json";
    private static string ingredientCatalogFileLocation = Directory.GetCurrentDirectory() + @"\IngredientCatalog.json";
    private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

    public static void ReadRecipeBook()
    {
      if (!File.Exists(recipeBookFileLocation))
      {
        List<Recipe> newRecipeBook = new List<Recipe>();
        WriteRecipeBook(newRecipeBook);
        RecipeBook.Recipes = newRecipeBook;
        return;
      }

      string jsonData = File.ReadAllText(recipeBookFileLocation);

      List<Recipe> recipeBook = JsonSerializer.Deserialize<List<Recipe>>(jsonData) ?? new List<Recipe>();
      RecipeBook.Recipes = recipeBook;
    }

    public static void ReadIngredientCatalog()
    {
      if (!File.Exists(ingredientCatalogFileLocation))
      {
        List<Ingredient> newIngredientCatalog = new List<Ingredient>();
        WriteIngredientCatalog(newIngredientCatalog);
        IngredientCatalog.Ingredients = newIngredientCatalog;
        return;
      }

      string jsonData = File.ReadAllText(ingredientCatalogFileLocation);

      List<Ingredient> ingredientCatalog = JsonSerializer.Deserialize<List<Ingredient>>(jsonData) ?? new List<Ingredient>();
      IngredientCatalog.Ingredients = ingredientCatalog;
    }

    public static void WriteRecipeBook()
    {
      string jsonString = JsonSerializer.Serialize(RecipeBook.Recipes, options: jsonSerializerOptions);

      File.WriteAllText(recipeBookFileLocation, jsonString);
    }

    public static void WriteRecipeBook(List<Recipe> recipeBookParam)
    {
      if (recipeBookParam == null)
      {
        WriteRecipeBook();
        return;
      }

      string jsonString = JsonSerializer.Serialize(recipeBookParam, options: jsonSerializerOptions);

      File.WriteAllText(recipeBookFileLocation, jsonString);
    }

    public static void WriteIngredientCatalog()
    {
      string jsonString = JsonSerializer.Serialize(IngredientCatalog.Ingredients, options: jsonSerializerOptions);

      File.WriteAllText(ingredientCatalogFileLocation, jsonString);
    }

    public static void WriteIngredientCatalog(List<Ingredient> ingredientCatalogParam)
    {
      if (ingredientCatalogParam == null)
      {
        WriteIngredientCatalog();
        return;
      }

      string jsonString = JsonSerializer.Serialize(ingredientCatalogParam, options: jsonSerializerOptions);

      File.WriteAllText(ingredientCatalogFileLocation, jsonString);
    }
  }
}
