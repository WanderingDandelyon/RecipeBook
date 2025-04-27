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
  public static class RecipeController
  {
    private static string input = string.Empty;
    private static string recipeInputInfo = "At any time, type RETURN to cancel recipe creation, NEXT to " +
      "jump to the next part of recipe creation, or BACK to re-do the previously entered part of the recipe";

    public static void ViewRecipe(Recipe recipeParam)
    {
      if (recipeParam == null) throw new ArgumentNullException("Null Recipe");

      Console.Clear();
      Console.WriteLine(recipeParam.Name);
      Console.WriteLine("--------------");
      Console.WriteLine("Ingredients:");
      foreach (RecipeIngredient ingredient in recipeParam.Ingredients)
      {
        //string plural = ingredient?.Measure?.Amount > 1 ? "s" : string.Empty;
        //Console.WriteLine($"{ingredient?.Measure?.Amount}{MeasurementUnitString.FromEnum(ingredient.Measure.Unit)}{plural} {ingredient.Name}");
      }
      Console.WriteLine(string.Empty);
      Console.WriteLine("Steps:");
      int stepNumber = 0;
      foreach (string step in recipeParam.Steps)
      {
        Console.WriteLine($"{stepNumber++}) {step}");
      }
    }

    public static void AddNewRecipe()
    {
      var newRecipe = new Recipe();
      Console.Clear();
      Console.WriteLine(recipeInputInfo);
      Console.Write("Name? ");
      input = Console.ReadLine() ?? string.Empty;
      string name = input;
      newRecipe.Name = name;
      newRecipe.Ingredients = AddIngredients();
      newRecipe.Steps = AddSteps();
      RecipeBook.Recipes.Add(newRecipe);
      JsonFileService.WriteRecipeBook();
      Console.WriteLine($"Added {newRecipe.Name}");
      Console.ReadKey();
    }

    private static List<string> AddSteps()
    {
      List<string> steps = new List<string>();

      while (true)
      {
        Console.Clear();
        Console.WriteLine(recipeInputInfo);
        Console.WriteLine();
        Console.WriteLine("Steps:");
        foreach (string step in steps)
        {
          Console.WriteLine(step);
        }
        Console.WriteLine("Next Step? ");
        input = Console.ReadLine() ?? string.Empty;
        if (string.Equals(input.ToLower(), "next")) break;
        if (string.Equals(input.ToLower(), "return")) return new List<string>();
        steps.Add(input);
      }
      return steps;
    }

    private static List<RecipeIngredient> AddIngredients()
    {
      // TODO: Add a way to add substitutions
      List<RecipeIngredient> ingredients = new List<RecipeIngredient>();

      while (true)
      {
        Console.Clear();
        Console.WriteLine(recipeInputInfo);
        Console.WriteLine();
        Console.WriteLine("Ingredients:");
        foreach (RecipeIngredient ingredient in ingredients)
        {
          //string plural = ingredient?.Measure?.Amount > 1 ? "s" : string.Empty;
          //Console.WriteLine($"{ingredient?.Measure?.Amount}{MeasurementUnitString.FromEnum(ingredient.Measure.Unit)}{plural} {ingredient.Name}");
        }

        // TODO: Add support for NEXT, BACK, and RETURN
        Console.WriteLine("Ingredient Name? ");
        input = Console.ReadLine() ?? string.Empty;
        if (string.Equals(input.ToLower(), "next")) break;
        string name = input;
        Console.WriteLine("Amount? ");
        input = Console.ReadLine() ?? string.Empty;
        if (string.Equals(input.ToLower(), "next")) break;
        double amount = double.Parse(input);
        Console.WriteLine("Unit (g, l, pinch, dash)? ");
        input = Console.ReadLine() ?? string.Empty;
        if (string.Equals(input.ToLower(), "next")) break;
        MeasurementUnit measurementUnit;
        switch (input.ToLower())
        {
          case "g":
            measurementUnit = MeasurementUnit.Gram;
            break;
          case "l":
            measurementUnit = MeasurementUnit.Liter;
            break;
          case "pinch":
            measurementUnit = MeasurementUnit.Pinch;
            break;
          case "dash":
            measurementUnit = MeasurementUnit.Dash;
            break;
          case "return":
            return new List<RecipeIngredient>();
          default:
            measurementUnit = MeasurementUnit.Unspecified;
            break;
        }
        //ingredients.Add(new RecipeIngredient { Name = name, Measure = new Measure { Amount = amount, Unit = measurementUnit } });
      }
      return ingredients;
    }
  }
}
