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
    private int bananaId;
    private int apricotId;
    private int evooId;
    private int friedBananaId;
    private int friedApricotId;

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

      bananaId = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Name.Equals("Banana"))?.Id ?? -1;
      apricotId = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Name.Equals("Apricot"))?.Id ?? -1;
      evooId = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Name.Equals("Extra Virgin Olive Oil"))?.Id ?? -1;

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

      friedBananaId = RecipeBook.Recipes.FirstOrDefault(r => r.Name.Equals("Fried Banana"))?.Id ?? -1;
      friedApricotId = RecipeBook.Recipes.FirstOrDefault(r => r.Name.Equals("Fried Apricot"))?.Id ?? -1;
    }

    [Test]
    public async Task SetUpTest()
    {
      Assert.That(IngredientCatalog.Ingredients.Count, Is.EqualTo(3));
      Assert.That(bananaId >= 0);
      Assert.That(apricotId >= 0);
      Assert.That(evooId >= 0);

      Assert.That(RecipeBook.Recipes.Count, Is.EqualTo(2));
      Assert.That(friedBananaId >= 0);
      Assert.That(friedApricotId >= 0);
    }

    [Test]
    public async Task GetRecipeTest()
    {
      var friedBanana = await RecipeService.GetRecipe(friedBananaId);

      Assert.IsNotNull(friedBanana);
      Assert.That(friedBanana.Name.Equals("Fried Banana"));
    }
  }
}
