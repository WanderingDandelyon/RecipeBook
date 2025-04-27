using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RB.Core.Model;

namespace RB.Presentation.ConsoleInterface
{
  // TODO: Decouple, add API, and have console go through API as an alternative to web interface
  // TODO: Design a menu object to dynamically handle menus instead of hardcoded
  // TODO: Make things a little prettier
  // TODO: Implement a ticketing system instead of all of these TODO comments
  public static class Menus
  {
    public static string input = string.Empty;

    public static void MainMenu()
    {
      while (true)
      {
        Console.Clear();
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("---------");
        Console.WriteLine("1) View Recipe Book");
        Console.WriteLine("2) Search Recipe Book");
        Console.WriteLine("3) Add New Recipe");
        Console.WriteLine("4) Exit");

        Console.Write("? ");
        input = Console.ReadLine() ?? string.Empty;

        switch (input)
        {
          case "1":
            ViewRecipeBookMenu();
            break;
          case "2":
            throw new NotImplementedException();
          case "3":
            RecipeController.AddNewRecipe();
            break;
          case "4":
            return;
          default:
            break;
        }
      }
    }

    public static void ViewRecipeBookMenu()
    {
      while (true)
      {
        Dictionary<int, Recipe> recipeDictionary = new Dictionary<int, Recipe>();
        int recipeNumber = 0;
        foreach (var recipe in RecipeBook.Recipes)
        {
          recipeDictionary.Add(recipeNumber++, recipe);
        }

        Console.Clear();
        Console.WriteLine("RECIPE BOOK CONTENTS");
        Console.WriteLine("--------------------");
        // TODO: Add pagination to results
        foreach (var recipe in recipeDictionary)
        {
          Console.WriteLine($"{recipe.Key}) {recipe.Value.Name}");
        }
        Console.WriteLine($"A) Return");

        Console.Write("? ");
        input = Console.ReadLine() ?? string.Empty;

        if (string.Equals(input.ToLower(), "a")) return;
        int inputNumber = int.Parse(input);
        if (recipeDictionary.ContainsKey(inputNumber))
        {
          ViewRecipeMenu(recipeDictionary[inputNumber]);
        }
      }
    }

    public static void ViewRecipeMenu(Recipe recipeParam)
    {
      RecipeController.ViewRecipe(recipeParam);
      Console.WriteLine();
      Console.WriteLine("1) Return");
      Console.WriteLine("2) Edit Recipe");
      Console.WriteLine("3) Delete Recipe");
      Console.Write("? ");

      input = Console.ReadLine() ?? string.Empty;

      switch (input)
      {
        case "1":
          return;
        case "2":
          throw new NotImplementedException();
        case "3":
          throw new NotImplementedException();
        default:
          break;
      }
    }
  }
}
