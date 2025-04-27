using RB.Core.Model.Enums;

namespace RB.Core.Model
{
  public class Ingredient
  {
    private readonly int id;
    public int Id { get => id; }
    public string Name { get; set; } = string.Empty;
    public List<string> AlternateNames { get; set; } = new List<string>();
    public MeasurementUnit MeasurementUnit { get; set; }
    public List<RecipeIngredient> CommonSubstitutions { get; set; } = new List<RecipeIngredient>();
    // TODO: Make this more dynamic relative to region
    public List<Month> MonthsInSeason { get; set; } = new List<Month>();
    public bool IsVegan { get; set; }
    public bool IsVegetarian { get; set; }
    public bool IsDairy { get; set; }
    public bool IsFodmap { get; set; }
    public bool IsGlutinous { get; set; }
    // TODO: Add additional dietary information (asian vegetarianism, pescatarianism, processed, caveman)
    // TODO: Add nutritional information (calories, fat, carb, etc. per count)
    public bool IsInSeason()
    {
      if (!MonthsInSeason.Any())
      {
        return true;
      }

      var currentMonth = DateTime.Now.Month;
      return MonthsInSeason.Contains((Month)currentMonth) || MonthsInSeason.Contains(Month.YearRound);
    }

    public Ingredient()
    {
      id = IngredientCatalog.Ingredients.Count;
    }
  }
}
