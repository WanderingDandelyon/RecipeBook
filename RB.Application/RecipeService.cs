using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RB.Core.Model;

namespace RB.Application
{
  public class RecipeService
  {
    public static async Task<Recipe?> GetRecipe(int idParam)
    {
      return RecipeBook.Recipes.FirstOrDefault(r => r.Id == idParam);
    }
  }
}
