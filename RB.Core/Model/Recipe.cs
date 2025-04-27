using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.Core.Model
{
  public class Recipe
  {
    private readonly int id;
    public int Id { get => id; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
    public List<string> Steps { get; set; } = new List<string>();
    public int? Yield { get; set; }

    public Recipe()
    {
      id = RecipeBook.Recipes.Count;
    }
  }
}
