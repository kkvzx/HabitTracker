using System.Data.SqlTypes;
using HabitTracker.kkvzx.database;
using Microsoft.Data.Sqlite;

namespace HabitTracker.kkvzx;

public class Menu()
{
    private const string ConnectionString = @"Data Source=habit-tracker.db";

    private SqliteConnection Connection { get; } = new(ConnectionString);
    private bool IsAppRunning { get; set; } = true;
    private string? SelectedOption { get; set; }

    public void Start()
    {
        Database.CreateTable();

        while (IsAppRunning)
        {
            GetUserInputFromMenu();
            HandleMenuSelection();
        }
    }

    private void GetUserInputFromMenu()
    {
        Console.Clear();
        Console.WriteLine("Welcome to HabitTracker App!");
        Console.WriteLine("----------------------------");
        Console.WriteLine();
        Console.WriteLine("1. Show records");
        Console.WriteLine("2. Insert record");
        Console.WriteLine("3. Delete record");
        Console.WriteLine("4. Update record");
        Console.WriteLine("0. Exit");
        Console.WriteLine("----------------------------");
        Console.WriteLine("What option do you select?: ");

        SelectedOption = GetUserInput();
    }

    private void HandleMenuSelection()
    {
        switch (SelectedOption)
        {
            case "1":
            {
                Database.ShowAllEntries();
                break;
            }
            case "2":
            {
                HabitModel model = GetHabitModel();
                Database.InsertModel(model);
                break;
            }
            case "3":
            {
                Database.ShowAllEntries();
                Console.WriteLine("Enter the Id of the record you want to delete: ");
                string recordId = GetUserInput();
                
                Database.Delete(recordId);
                break;
            }
            case "4":
            {
                //Edit only desired fields
                Database.ShowAllEntries();
                
                Console.WriteLine("Enter the Id of the record you want to update: ");
                string recordId = GetUserInput();
                Console.WriteLine("Enter updated properties");
                HabitModel model = GetHabitModel();
                Database.Update(recordId, model);
                break;
            }
            case "0":
            {
                IsAppRunning = false;
                break;
            }
        }

        if (SelectedOption != "0")
        {
            PressKeyToContinue();
        }
    }

    static void PressKeyToContinue()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
        Console.WriteLine();
    }

    static HabitModel GetHabitModel()
    {
        string date = GetDateInput();
        int quantity = GetQuantityInput();

        return new HabitModel(date, quantity);
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

    private static string GetDateInput()
    {
        Console.WriteLine("Please insert the date: (Format: dd-mm-yyyy): ");
        return GetUserInput();
    }

    private static int GetQuantityInput()
    {
        int quantity;
        
        Console.WriteLine("Please insert the quantity (integer only): ");
        while (!int.TryParse(GetUserInput(), out quantity))
        {
        }

        return quantity;
    }
}