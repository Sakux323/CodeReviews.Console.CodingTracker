
using CodingTracker.Controllers;
using CodingTracker.Models;
using Spectre.Console;
using System.Diagnostics;
using System.Globalization;

namespace CodingTracker.UserInterface;

internal static class UserInput
{
    private static SessionController _controller = new SessionController();
    public static void StartCodingSession()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var startTime = DateTime.Now;

        AnsiConsole.MarkupLine("[green]Press [bold]Enter[/] to stop the session[/]");

        // Use Live display
        AnsiConsole.Live(new Markup("Counting Session Time: [bold yellow]0:00:00[/]"))
            .AutoClear(false)
            .Start(ctx =>
            {
                Console.Clear();
                while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Enter)
                {
                    var elapsed = stopwatch.Elapsed;

                    // Format the time as HH:mm:ss
                    string formattedTime = $"{elapsed.Hours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}";

                    // Update live markup
                    ctx.UpdateTarget(new Markup($"Counting Session Time: [bold yellow]{formattedTime}[/]"));

                    Thread.Sleep(500); // Refresh every 0.5 seconds
                }
            });
        stopwatch.Stop();

        var endTime = DateTime.Now;

        var duration = CalculateDuration(startTime, endTime);

        CodingSession session = new CodingSession
        {
            StartTime = FormatDateToString(startTime),
            EndTime = FormatDateToString(endTime),
            Duration = duration
        };

        _controller.AddSession(session);
    }

    private static double CalculateDuration(DateTime startTime, DateTime endTime)
    {
        var difference = endTime.Subtract(startTime);
        return difference.TotalSeconds;
    }

    private static string FormatDateToString(DateTime date)
    {
        return date.ToString("dd-MM-yy HH:mm");
    }

    private static string FormatDuration(double duration)
    {
        duration = Math.Floor(duration);
        if (duration < 60)
            return $"{duration} second{(duration == 1 ? "" : "s")}";

        if (duration < 3600)
        {
            double minutes = Math.Floor(duration / 60);
            return $"{minutes} minute{(minutes == 1 ? "" : "s")}";
        }

        if (duration < 86400)
        {
            double hours = Math.Floor(duration / 3600);
            return $"{hours} hour{(hours == 1 ? "" : "s")}";
        }

        double days = Math.Floor(duration / 86400);
        return $"{days} day{(days == 1 ? "" : "s")}";
    }

    public static void ViewAllSessions(bool stopConsole = false)
    {
        List<CodingSession> sessions = _controller.GetAllSessions();

        var table = new Table();
        table.AddColumns(["ID", "Start Time", "End Time", "Duration"]);
        
        foreach(var column in table.Columns)
        {
            column.Centered();
        }

        foreach (var session in sessions)
        {
            table.AddRow($"[blue]{session.Id}[/]", $"{session.StartTime}", $"{session.EndTime}", $"{FormatDuration(session.Duration)}");
        }

        table.Centered();

        AnsiConsole.Write(table);

        if (stopConsole) Console.ReadKey();
    }

    private static int GetNumberInput()
    {
        int number = -1;
        string input = Console.ReadLine();
        while (!Int32.TryParse(input, out number))
        {
            Console.WriteLine("Invalid input, please input an integer.");
            input = Console.ReadLine();
        }

        return number;
    }

    private static DateTime GetDateInput(string message = "")
    {
        if (!string.IsNullOrEmpty(message)) Console.WriteLine(message);

        string date = Console.ReadLine();

        DateTime cleanDate;

        while (!DateTime.TryParseExact(date, "dd-MM-yy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out cleanDate))
        {
            Console.WriteLine("Invalid input, please ensure you use the (dd-MM-yy HH:mm) format. Example: 20-01-25 16:30");
            date = Console.ReadLine();
        }

        return cleanDate;
    }
    public static void AddSession()
    {
        CodingSession session = new CodingSession();
        
        DateTime startTime = GetDateInput("Enter the start date of the session in the following format (dd-MM-yy hh:mm)");
        DateTime endTime = GetDateInput("Enter the end date of the session in the following format (dd-MM-yy hh:mm)");

        while(startTime > endTime)
        {
            Console.WriteLine("Start Time cannot exceed End Time.");
            startTime = GetDateInput("Enter the start date of the session in the following format (dd-MM-yy hh:mm)");
            endTime = GetDateInput("Enter the end date of the session in the following format (dd-MM-yy hh:mm)");
        }

        double duration = CalculateDuration(startTime, endTime);

        session.StartTime = FormatDateToString(startTime);
        session.EndTime = FormatDateToString(endTime);
        session.Duration = duration;

        _controller.AddSession(session);
        
    }

    public static void UpdateSession()
    {
        ViewAllSessions();

        Console.WriteLine("Pick a session you'd like to update.");

        int sessionId = GetNumberInput();

        while (!_controller.SessionExists(sessionId))
        {
            Console.WriteLine("Session with that ID does not exist, please try again.");
            sessionId = GetNumberInput();
        }

        DateTime newStartTime = GetDateInput("Enter the start date of the session in the following format (dd-MM-yy hh:mm)");
        DateTime newEndTime = GetDateInput("Enter the end date of the session in the following format (dd-MM-yy hh:mm)");
    }

    public static void DeleteSession()
    {
        ViewAllSessions();

        Console.WriteLine("Pick which session you'd like to delete");

        int sessionId = GetNumberInput();

        while (!_controller.SessionExists(sessionId))
        {
            Console.WriteLine("Session with that ID does not exist, please try again.");
            sessionId = GetNumberInput();
        }

        _controller.DeleteSession(sessionId);

        Console.Clear();
        ViewAllSessions(); // show the update table again
        Console.WriteLine("Session Deleted");
    }
}
