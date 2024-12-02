// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Aoc;
using Spectre.Console;

var challenges = Assembly.GetExecutingAssembly().GetTypes()
    .Where(x => x.GetInterfaces().Any(i => i == typeof(IChallenge)))
    .Select(x => new { x.Name, Challenge = Activator.CreateInstance(x) as IChallenge })
    .OrderByDescending(x => x.Name)
    .Select(x => x.Challenge!);

// Ask for the user's favorite fruit
var challenge = AnsiConsole.Prompt(
    new SelectionPrompt<IChallenge>()
        .Title("Select [green]puzzle[/]?")
        .PageSize(10)
        .Mode(SelectionMode.Leaf)
        .AddChoices(challenges)
        .UseConverter(challenge => challenge.GetType().Name));

var inputType = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Use which input?")
        .Mode(SelectionMode.Leaf)
        .AddChoices(["Dev", "Real"] ));

if(inputType == "Dev")
{
    challenge.UseDevInput();
}
challenge.Run();

AnsiConsole.MarkupLine("All done! [green]Have a great day![/]");
Console.ReadKey();