using CodingTracker.UserInterface;

namespace CodingTracker;

class Program
{
    static void Main(string[] args)
    {
        Database.CreateDatabase();

        var menu = new UserMenu();

        menu.ShowMenu();
    }
}