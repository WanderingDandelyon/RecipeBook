using RB.Core.Model;

namespace RB.Presentation.WebApi.Controllers.ParamObjects
{
  public class RecipeParam
  {
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<(int IngredientId, double? Amount)>? Ingredients { get; set; }
    public List<string>? Steps { get; set; }
    public int? RecipeId;
    public string? SearchTerm { get; set; }
    public int? Yield { get; set; }
    
    public RecipeIngredient IngredientToRecipeIngredient((int? IngredientId, double? Amount) ingredientParam)
    {
      return new RecipeIngredient
      {
        IngredientId = ingredientParam.IngredientId ?? throw new ArgumentNullException("Null Ingredient Id"),
        MeasurementAmount = ingredientParam.Amount ?? throw new ArgumentNullException("Null Ingredient Amount")
      };
    }
  }
}
