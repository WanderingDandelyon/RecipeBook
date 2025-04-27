// See https://aka.ms/new-console-template for more information
using RB.Infrastructure.LocalFileStorage;
using RB.Presentation.ConsoleInterface;

Console.WriteLine("RECIPE BOOK");
// TODO: Decouple, add API, and have console go through API as an alternative to web interface
JsonFileService.ReadRecipeBook();
JsonFileService.ReadIngredientCatalog();
Menus.MainMenu();
