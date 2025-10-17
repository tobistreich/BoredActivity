using System.Net.Http.Json;
using Microsoft.Data.Sqlite;

namespace HttpClientSample
{
    public class BoredActivity
    {
        public int Id { get; set; }
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
            selectCmd.CommandText = "SELECT Id, Activity, Type, Participants FROM Activities";

            using var reader = selectCmd.ExecuteReader();
            while (reader.Read())
            {
                activities.Add(new BoredActivity
                {
                    Id = reader.GetInt32(0),
                    Activity = reader.GetString(1),
                    Type = reader.GetString(2),
                    Participants = reader.GetInt32(3)
                });
            }

            return activities;
        }

        // Mehrere IDs gleichzeitig löschen
        public void DeleteActivitiesByIds(IEnumerable<int> ids)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var deleteCmd = connection.CreateCommand();
            // Platzhalter dynamisch generieren: $id0, $id1, $id2 ...
            var parameters = ids.Select((id, index) => $"$id{index}").ToList();
            deleteCmd.CommandText = $"DELETE FROM Activities WHERE Id IN ({string.Join(",", parameters)})";

            int i = 0;
            foreach (var id in ids)
            {
                deleteCmd.Parameters.AddWithValue(parameters[i], id);
                i++;
            }

            int rows = deleteCmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} Aktivität(en) gelöscht.");
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

            Console.WriteLine($"Neue Aktivität: {activity.Activity} ({activity.Type}, Teilnehmer: {activity.Participants})");
            repo.SaveActivity(activity);
            Console.WriteLine("Aktivität in DB gespeichert.");

            Console.WriteLine("\nAlle gespeicherten Aktivitäten:");
            foreach (var act in repo.GetAllActivities())
            {
                Console.WriteLine($"- ID: {act.Id} {act.Activity} ({act.Type}, Teilnehmer: {act.Participants})");
            }

            Console.WriteLine("\nMöchten Sie Aktivitäten löschen? (y/n)");
            var input = Console.ReadLine();
            if (input?.ToLower() == "y")
            {
                Console.Write("Bitte geben Sie die ID(s) mit Komma getrennt ein (z.B. 1,3,5): ");
                var idInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(idInput))
                {
                    var ids = idInput
                        .Split(',')
                        .Select(s => s.Trim())
                        .Where(s => int.TryParse(s, out _))
                        .Select(int.Parse)
                        .ToList();

                    if (ids.Any())
                        repo.DeleteActivitiesByIds(ids);
                    else
                        Console.WriteLine("Keine gültigen IDs eingegeben.");
                }
            }
        }
    }
}
