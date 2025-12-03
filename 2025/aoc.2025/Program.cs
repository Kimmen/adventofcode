// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Aoc;
using Spectre.Console;

var challenges = Assembly.GetExecutingAssembly().GetTypes()
    .Where(x => x.GetInterfaces().Any(i => i == typeof(IChallenge)))
    .Select(x => new { Name = x.FullName, Challenge = Activator.CreateInstance(x) as IChallenge })
    .OrderByDescending(x => x.Name)
    .Select(x => x.Challenge!);

// Ask for the user's favorite fruit
var challenge = AnsiConsole.Prompt(
    new SelectionPrompt<IChallenge>()
        .Title("Select [green]puzzle[/]?")
        .PageSize(10)
        .Mode(SelectionMode.Leaf)
        .AddChoices(challenges)
        .UseConverter(challenge => challenge.GetType().FullName!.Replace("Aoc.", "")));

var inputType = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Use which input?")
        .Mode(SelectionMode.Leaf)
        .AddChoices(["Dev", "Real"] ));

var speedInput = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Which speed?")
        .Mode(SelectionMode.Leaf)
        .AddChoices(["Ultra instinct","Fast", "Medium", "Slow"] ));

if(inputType == "Dev")
{
    challenge.UseDevInput();
}

challenge.RefreshRate(speedInput switch
{
    "Ultra instinct" => long.MaxValue,
    "Fast" => 200,
    "Medium" => 50,
    "Slow" => 1,
    _ => 100
});

try
{
    challenge.Run();
}
catch (Exception e)
{
    AnsiConsole.WriteException(e, ExceptionFormats.Default);
    throw;
}

AnsiConsole.MarkupLine("All done! [green]Have a great day![/]");