

using Spectre.Console;
using CodingTracker.Enums;

namespace CodingTracker.UserInterface;

internal class UserMenu
{
    public void ShowMenu()
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItems>()
                .Title("Select an option")
                .UseConverter(item =>
                {
                    return item switch
                    {
                        MenuItems.StartCodingSession => "Start Coding Session",
                        MenuItems.ViewAllSessions => "View All Sessions",
                        MenuItems.AddSession => "Add New Session",
                        MenuItems.UpdateSession => "Update a Session",
                        MenuItems.DeleteSession => "Delete a Session",
                        _ => item.ToString()
                    };
                })
                .AddChoices(Enum.GetValues<MenuItems>()));

            switch (choice)
            {
                case MenuItems.StartCodingSession:
                    ClearConsole();
                    UserInput.StartCodingSession();
                    break;

                case MenuItems.ViewAllSessions:
                    ClearConsole();
                    UserInput.ViewAllSessions(true);
                    break;

                case MenuItems.AddSession:
                    ClearConsole();
                    UserInput.AddSession();
                    break;

                case MenuItems.UpdateSession:
                    ClearConsole();
                    UserInput.UpdateSession();
                    break;

                case MenuItems.DeleteSession:
                    ClearConsole();
                    UserInput.DeleteSession();
                    break;

                case MenuItems.Quit:
                    Environment.Exit(0);
                    break;

                default:
                    break;
            }
        }
    }

    private void ClearConsole()
    {
        Console.Clear();
    }
}
