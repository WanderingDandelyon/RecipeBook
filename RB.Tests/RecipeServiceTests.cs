using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RB.Application;
using RB.Core.Model;

namespace RB.Tests
{
  public class RecipeServiceTests
  {
    private string bananaId;
    private string apricotId;
    private string evooId;
    private string friedBananaId;
    private string friedApricotId;

    [SetUp]
    public void SetUp()
    {
      IngredientCatalog.Ingredients = new List<Ingredient>
      {
        new Ingredient
        {
          Name = "Banana"
        },
        new Ingredient
        {
          Name = "Apricot"
        },
        new Ingredient
        {
          Name = "Extra Virgin Olive Oil",
        }
      };

      bananaId = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Name.Equals("Banana"))?.Id;
      apricotId = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Name.Equals("Apricot"))?.Id;
      evooId = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Name.Equals("Extra Virgin Olive Oil"))?.Id;

      RecipeBook.Recipes = new List<Recipe>
      {
        new Recipe
        {
          Name = "Fried Banana",
          Ingredients = new List<RecipeIngredient>
          {
            new RecipeIngredient
            {
              IngredientId = bananaId,
              MeasurementAmount = 1,
            },
            new RecipeIngredient
            {
              IngredientId = evooId,
              MeasurementAmount = 1,
            }
          },
          Steps = new List<string>
          {
            "Fry the banana"
          }
        },
        new Recipe
        {
          Name = "Fried Apricot",
          Ingredients = new List<RecipeIngredient>
          {
            new RecipeIngredient
            {
              IngredientId = apricotId,
              MeasurementAmount = 1,
            },
            new RecipeIngredient
            {
              IngredientId = evooId,
              MeasurementAmount = 1,
            }
          },
          Steps = new List<string>
          {
            "Fry the apricot"
          }
        }
      };

      friedBananaId = RecipeBook.Recipes.FirstOrDefault(r => r.Name.Equals("Fried Banana"))?.Id;
      friedApricotId = RecipeBook.Recipes.FirstOrDefault(r => r.Name.Equals("Fried Apricot"))?.Id;
    }

    [Test]
    public async Task SetUpTest()
    {
      Assert.That(IngredientCatalog.Ingredients.Count, Is.EqualTo(3));
      Assert.That(!string.IsNullOrWhiteSpace(bananaId));
      Assert.That(!string.IsNullOrWhiteSpace(apricotId));
      Assert.That(!string.IsNullOrWhiteSpace(evooId));

      Assert.That(RecipeBook.Recipes.Count, Is.EqualTo(2));
      Assert.That(!string.IsNullOrWhiteSpace(friedBananaId));
      Assert.That(!string.IsNullOrWhiteSpace(friedApricotId));
    }

    [Test]
    public async Task SearchRecipesTest()
    {
      var recipes = await RecipeService.SearchRecipes();

      Assert.That(recipes.Count() == RecipeBook.Recipes.Count());
    }

    [Test]
    public async Task GetRecipeTest()
    {
      var friedBanana = await RecipeService.GetRecipe(friedBananaId);

      Assert.IsNotNull(friedBanana);
      Assert.That(friedBanana.Name.Equals("Fried Banana"));
    }

    public async Task CreateRecipeTest()
    {
      var banapricotSmoothieId = await RecipeService.AddRecipe(
        "Banapricot Smoothie", 
        new List<(string, double?)> 
        { 
          (bananaId, 1.0D), 
          (apricotId, 1.0D) 
        }, 
        "A banana and apricot smoothie but without the milk and honey?!", 
        new List<string> 
        { 
          "Blend the banana and the apricot in a blender." 
        }, 
        1);

      Assert.That(RecipeBook.Recipes.Count() == 3);
      Assert.That(RecipeBook.Recipes.Any(r => r.Name.Equals("Banapricot Smoothie")));
      Assert.That(RecipeBook.Recipes.FirstOrDefault(r => r.Id == banapricotSmoothieId).Ingredients.Any(i => i.IngredientId == bananaId && i.MeasurementAmount == 1.0D));
    }
  }
}
