using RB.Core.Model.Enums;

namespace RB.Core.Model
{
  public class Ingredient
  {
    private readonly string id;
    public string Id { get => id; }
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
    // Nutrition information (per standard serving)
    public int Calories { get; set; }
    public int Fat { get; set; }
    public int SaturatedFat { get; set; }
    public int TransFat { get; set; }
    public int Cholesterol { get; set; }
    public int Sodium { get; set; }
    public int TotalCarbohydrate { get; set; }
    public int DietaryFiber { get; set; }
    public int TotalSugars { get; set; }
    public int AddedSugars { get; set; }
    public int Protein { get; set; }
    public int VitaminA { get; set; }
    public int VitaminB { get; set; }
    public int VitaminC { get; set; }
    public int VitaminD { get; set; }
    public int Calcium { get; set; }
    public int Iron { get; set; }
    public int Potassium { get; set; }
    public int Magnesium { get; set; }

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
      id = Guid.NewGuid().ToString();
    }
  }
}
