using System.Data.SqlTypes;
using Microsoft.Data.Sqlite;

namespace HabitTracker.kkvzx;

public class Menu()
{
    private const string ConnectionString = @"Data Source=habit-tracker.db";

    private SqliteConnection Connection { get; } = new(ConnectionString);
    private bool IsAppRunning { get; set; } = true;
    private string? SelectedOption { get; set; } = null;

    public void Start()
    {
        CreateDatabaseTable();

        while (IsAppRunning)
        {
            GetUserInputFromMenu();
            HandleMenuSelection();
        }
    }

    private void GetUserInputFromMenu()
    {
        Console.WriteLine("Welcome to HabitTracker App!");
        Console.WriteLine("----------------------------");
        Console.WriteLine();
        Console.WriteLine("1. Show records");
        Console.WriteLine("2. Insert record");
        Console.WriteLine("3. Delete record");
        Console.WriteLine("4. Update record");
        Console.WriteLine("0. Exit");
        Console.WriteLine("What option do you select?: ");

        SelectedOption = GetUserInput();
    }

    private void HandleMenuSelection()
    {
        switch (SelectedOption)
        {
            case "1":
            {
                ReadEntriesFromDatabase();
                PressKeyToContinue();
                break;
            }
            case "2":
            {
                Console.Write("Enter date: ");
                string date = GetUserInput();

                Console.Write("Enter quantity: ");
                int.TryParse(GetUserInput(), out int quantity);

                AddModelToDatabase(date, quantity);
                PressKeyToContinue();
                break;
            }
            case "0":
            {
                IsAppRunning = false;
                break;
            }
        }
    }

    static void PressKeyToContinue()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
        Console.WriteLine();
    }

    static string GetUserInput()
    {
        string? userInput = Console.ReadLine();

        while (userInput is null)
        {
            Console.Write("Invalid entry, try again: ");
            userInput = Console.ReadLine();
        }

        return userInput;
    }


    private void CreateDatabaseTable()
    {
        Connection.Open();
        var tableCommand = Connection.CreateCommand();

        tableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS skill_workout (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Date TEXT,
Quantity INTEGER )";
        tableCommand.ExecuteNonQuery();

        Connection.Close();
    }

    private void AddModelToDatabase(string date, int quantity)
    {
        try
        {
            Connection.Open();
            var tableCommand = Connection.CreateCommand();

            tableCommand.CommandText = @"INSERT INTO skill_workout 
    (Date, Quantity) VALUES (@DateParam, @QuantityParam)";
            tableCommand.Parameters.AddWithValue("@DateParam", date);
            tableCommand.Parameters.AddWithValue("@QuantityParam", quantity);
            tableCommand.ExecuteNonQuery();

            Console.WriteLine($"Successfully added record to skill_workout");
            Connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void ReadEntriesFromDatabase()
    {
        Connection.Open();

        var tableCommand = Connection.CreateCommand();

        tableCommand.CommandText = @"SELECT * FROM skill_workout";

        var reader = tableCommand.ExecuteReader();
        while (reader.Read())
        {
            string id = reader.GetString(0);
            string date = reader.GetString(1);
            int quantity = reader.GetInt32(2);

            Console.WriteLine($"ID: {id}\tDate: {date}\tQuantity: {quantity}");
        }

        Connection.Open();
    }
}