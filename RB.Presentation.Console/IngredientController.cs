using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RB.Core.Model;
using RB.Core.Model.Enums;
using RB.Infrastructure.LocalFileStorage;

namespace RB.Presentation.ConsoleInterface
{
  public static class IngredientController
  {
    private static string input = string.Empty;
    private static string ingredientInputInfo = "At any time, type RETURN to cancel recipe creation, NEXT to " +
      "jump to the next part of recipe creation, or BACK to re-do the previously entered part of the recipe";

    public static void ViewIngredient(Ingredient ingredientParam)
    {
      throw new NotImplementedException();
    }

    public static void AddNewIngredient()
    {
      throw new NotImplementedException();
    }
  }
}
