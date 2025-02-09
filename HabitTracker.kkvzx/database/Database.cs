using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using HabitTracker.kkvzx.database;
using Microsoft.Data.Sqlite;

namespace HabitTracker.kkvzx;

public static class Database
{
    private const string ConnectionString = @"Data Source=habit-tracker.db";

    public static void CreateTable()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS skill_workout (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Date TEXT,
Quantity INTEGER )";
            tableCommand.ExecuteNonQuery();

            connection.Close();
        }
    }

    public static void InsertModel(HabitModel habitModel)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            try
            {
                connection.Open();
                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText =
                    $"INSERT INTO skill_workout(Date, Quantity) VALUES('{habitModel.Date}', {habitModel.Quantity})";
                tableCommand.ExecuteNonQuery();

                Console.WriteLine($"Successfully added record to skill_workout");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static void ShowAllEntries()
    {
        Console.Clear();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            List<HabitDto> habitDtos = new List<HabitDto>();

            try
            {
                connection.Open();

                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText = $"SELECT * FROM skill_workout";

                var reader = tableCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        DateTime date =
                            DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US"));
                        int quantity = reader.GetInt32(2);

                        habitDtos.Add(new HabitDto(id, date, quantity));
                    }

                    DisplayHabits(habitDtos);
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            finally
            {
                connection.Close();
            }
        }

        void DisplayHabits(List<HabitDto> habitDtos)
        {
            Console.WriteLine($"ID\tDate\t\tQuantity");

            foreach (HabitDto habit in habitDtos)
            {
                Console.WriteLine($"{habit.Id}\t{habit.Date.ToString("dd/MMM/yyyy")}\t{habit.Quantity}");
            }
        }
    }

    public static void Delete(string recordId)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            try
            {
                connection.Open();
                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText =
                    $"DELETE FROM skill_workout WHERE Id='{recordId}'";
                int affectedRows = tableCommand.ExecuteNonQuery();

                if (affectedRows == 0)
                {
                    Console.WriteLine($"Faile to delete record with {recordId} id.");
                }
                else
                {
                    Console.WriteLine($"Successfully deleted {recordId} from skill_workout table");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static void Update(string recordId, HabitModel habitModel)
    {
        if (!CheckIfRecordExists(recordId))
        {
            Console.WriteLine($"Record with Id {recordId} does not exist. Try again with different Id.");

            return;
        }

        using (var connection = new SqliteConnection(ConnectionString))
        {
            try
            {
                connection.Open();

                var tableCommand = connection.CreateCommand();
                tableCommand.CommandText =
                    $"UPDATE skill_workout SET Date = '{habitModel.Date}', Quantity = '{habitModel.Quantity}' WHERE Id='{recordId}'";


                Console.WriteLine($"Successfully updated record with {recordId} id in skill_workout table");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static bool CheckIfRecordExists(string recordId)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var checkCommand = connection.CreateCommand();

            checkCommand.CommandText = $"SELECT * FROM skill_workout WHERE Id='{recordId}'";
            int checkQuery = Convert.ToInt32(checkCommand.ExecuteScalar());

            return checkQuery > 0;
        }
    }
}