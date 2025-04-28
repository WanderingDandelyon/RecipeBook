using RB.Application;
using RB.Core.Model;
using RB.Core.Model.Enums;

namespace RB.Tests
{
  public class IngredientServiceTests
  {
    [SetUp]
    public void SetUp()
    {
      IngredientCatalog.Ingredients = new List<Ingredient>
      {
        new Ingredient
        {
          Name = "Banana"
        }
      };
    }

    [Test]
    public async Task CreateNewIngredientTest()
    {
      var bolognaId = await IngredientService.CreateIngredient("Bologna", new List<string>(), MeasurementUnit.Gram, new List<(int Id, double Amount)>(), new List<int>(), false, false, false, false, false);
      var magicBeanId = await IngredientService.CreateIngredient("Magic Bean", new List<string> { "WonderBean" }, MeasurementUnit.Gram, new List<(int Id, double Amount)> { (bolognaId, 1.0D) }, new List<int> { 2, 3, 4 }, true, true, true, true, true);
      
      Assert.That(bolognaId, Is.EqualTo(1));
      Assert.That(IngredientCatalog.Ingredients.Any(i => i.Name.Equals("Bologna")));
      var bologna = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Name.Equals("Bologna"));
      Assert.IsFalse(bologna?.IsVegan);
      Assert.IsFalse(bologna?.IsVegetarian);
      Assert.IsFalse(bologna?.IsDairy);
      Assert.IsFalse(bologna?.IsFodmap);
      Assert.IsFalse(bologna?.IsGlutinous);

      Assert.That(magicBeanId, Is.EqualTo(2));
      Assert.That(IngredientCatalog.Ingredients.Any(i => i.Name.Equals("Magic Bean")));
      var magicBean = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Name.Equals("Magic Bean"));
      Assert.IsTrue(magicBean?.IsVegan);
      Assert.IsTrue(magicBean?.IsVegetarian);
      Assert.IsTrue(magicBean?.IsDairy);
      Assert.IsTrue(magicBean?.IsFodmap);
      Assert.IsTrue(magicBean?.IsGlutinous);
      Assert.That(magicBean.AlternateNames.Any(n => n.Equals("WonderBean")));
      Assert.That(magicBean.CommonSubstitutions.Any(s => s.IngredientId == 1 && s.MeasurementAmount == 1.0D));
      Assert.That(magicBean.MeasurementUnit.Equals(MeasurementUnit.Gram));
      Assert.That(magicBean.MonthsInSeason.Any(m => m.Equals(Month.March)));
    }

    /*
    public async Task ModifyIngredientTest()
    {
      var bananaId = await IngredientService.ModifyIngredient(0, isDairyParam: false, isVeganParam: true);

      Assert.That(bananaId = 0);
      var banana = IngredientCatalog.Ingredients.FirstOrDefault(i => i.Name.Equals("Banana"));
      Assert.IsFalse(banana?.IsDairy);
      Assert.IsTrue(banana?.IsVegan);
    }
    */
  }
}
