using CodingTracker.Models;
using Microsoft.Data.Sqlite;
using System.Configuration;
using Dapper;

namespace CodingTracker.Controllers;

internal class SessionController
{
    readonly string _connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    public List<CodingSession> GetAllSessions()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM CodingSessions";

            return connection.Query<CodingSession>(query).ToList();
        }
    }

    public CodingSession GetSessionById(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        { 
            connection.Open();

            string query = "SELECT * FROM CodingSessions WHERE id = @Id";

            return connection.Query<CodingSession>(query, new { Id = id }).First();
        }
    }

    public void AddSession(CodingSession session)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            string query = "INSERT INTO CodingSessions (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";

            connection.Execute(query, session);
        }
    }

    public void DeleteSession(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            string query = "DELETE FROM CodingSessions WHERE id = @Id";

            connection.Execute(query, new { Id = id });
        }
    }

    public void UpdateSession(int id, CodingSession session)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
        }
    }

    public bool SessionExists(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            string sql = $"SELECT EXISTS(SELECT 1 FROM CodingSessions WHERE id = @Id)";

            return connection.ExecuteScalar<bool>(sql, new {Id = id});
        }
    }
}
