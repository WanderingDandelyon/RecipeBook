using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RB.Core.Enums;

namespace RB.Core.Model
{
  public class RecipeIngredient
  {
    public int IngredientId { get; set; }
    public double MeasurementAmount { get; set; }
    public PreparationMethod PreparationMethod { get; set; } = PreparationMethod.Unspecified;
    [JsonIgnore]
    public Ingredient Ingredient => IngredientCatalog.Ingredients.First(i => i.Id == IngredientId);
    // TODO: Support multiple measurement units per ingredient (2 onions, 2g of onion)
  }
}
