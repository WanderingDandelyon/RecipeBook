using RB.Core.Model.Enums;

namespace RB.Presentation.WebApi.Controllers.ParamObjects
{
    public class RecipeSearchParam
    {
        public string? SearchText { get; set; }
        public List<string>? IngredientNames { get; set; }
        public bool? InSeason { get; set; }
        public List<Month>? Seasons { get; set; }
        public bool? IsVegan { get; set; }
        public bool? IsVegetarian { get; set; }
        public bool? IsLactoseFree { get; set; }
        public bool? IsGlutenFree { get; set; }
    }
}
