using Microsoft.Data.Sqlite;
using System.Configuration;
namespace CodingTracker;

internal static class Database
{
    readonly static string _connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    internal static void CreateDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var cmd = connection.CreateCommand();

            cmd.CommandText =
                @"CREATE Table IF NOT EXISTS CodingSessions
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    StartTime TEXT NOT NULL,
                    EndTime TEXT NOT NULL,
                    Duration INTEGER NOT NULL
                )";

            cmd.ExecuteNonQuery();
        }
    }
}
