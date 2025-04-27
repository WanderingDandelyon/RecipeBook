using RB.Core.Model;
using RB.Core.Model.Enums;

namespace RB.Presentation.WebApi.Controllers.ParamObjects
{
  public class IngredientParam
  {
    public int? IngredientId { get; set; }
    public string? Name { get; set; }
    public List<string>? AlternateNames { get; set; } = new List<string>();
    public int? MeasurementUnit { get; set; }
    public List<(int Id, double Amount)>? CommonSubstitutions { get; set; } = new List<(int Id, double Amount)>();
    // TODO: Make this more dynamic relative to region
    public List<int>? MonthsInSeason { get; set; } = new List<int>();
    public int? Season { get; set; }
    public bool? IsVegan { get; set; }
    public bool? IsVegetarian { get; set; }
    public bool? IsDairy { get; set; }
    public bool? IsFodmap { get; set; }
    public bool? IsGlutinous { get; set; }
    public string? SearchTerm { get; set; }

  }
}
