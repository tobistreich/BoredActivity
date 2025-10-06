using System.Net.Http.Json;
using Microsoft.Data.Sqlite;

namespace HttpClientSample
{
    public class BoredActivity
    {
        public string Activity { get; set; } = "";
        public string Type { get; set; } = "";
        public int Participants { get; set; }
    }

    public class ActivityRepository
    {
        private readonly string _connectionString;

        public ActivityRepository(string databaseFile)
        {
            _connectionString = $"Data Source={databaseFile}";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS Activities (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Activity TEXT,
                Type TEXT,
                Participants INTEGER
            );";
            tableCmd.ExecuteNonQuery();
        }

        public void SaveActivity(BoredActivity activity)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
            @"INSERT INTO Activities (Activity, Type, Participants) VALUES ($activity, $type, $participants);";
            insertCmd.Parameters.AddWithValue("$activity", activity.Activity);
            insertCmd.Parameters.AddWithValue("$type", activity.Type);
            insertCmd.Parameters.AddWithValue("$participants", activity.Participants);

            insertCmd.ExecuteNonQuery();
        }

        public List<BoredActivity> GetAllActivities()
        {
            var activities = new List<BoredActivity>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT Activity, Type, Participants FROM Activities";

            using var reader = selectCmd.ExecuteReader();
            while (reader.Read())
            {
                activities.Add(new BoredActivity
                {
                    Activity = reader.GetString(0),
                    Type = reader.GetString(1),
                    Participants = reader.GetInt32(2)
                });
            }

            return activities;
        }
    }

    class Program
    {
        static async Task Main()
        {
            var http = new HttpClient();
            var repo = new ActivityRepository("db.sql");

            Console.WriteLine("Rufe API auf...");
            var activity = await http.GetFromJsonAsync<BoredActivity>(
                "https://bored-api.appbrewery.com/random");

            if (activity is null)
            {
                Console.WriteLine("Keine Daten von der API erhalten.");
                return;
            }

            // Ausgabe
            Console.WriteLine($"Neue Aktivität: {activity.Activity} ({activity.Type}, Teilnehmer: {activity.Participants})");
            repo.SaveActivity(activity);
            Console.WriteLine("✅ Aktivität in DB gespeichert.");

            // Alle Aktivitäten anzeigen
            Console.WriteLine("\n📋 Alle gespeicherten Aktivitäten:");
            foreach (var act in repo.GetAllActivities())
            {
                Console.WriteLine($"- {act.Activity} ({act.Type}, Teilnehmer: {act.Participants})");
            }
        }
    }
}