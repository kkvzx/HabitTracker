using System.Data.SqlTypes;
using HabitTracker.kkvzx.database;
using HabitTracker.kkvzx.enums;
using Microsoft.Data.Sqlite;

namespace HabitTracker.kkvzx;

public class Menu()
{
    private const string EscapeChar = "x";
    private const string ConnectionString = @"Data Source=habit-tracker.db";

    private SqliteConnection Connection { get; } = new(ConnectionString);
    private bool IsAppRunning { get; set; } = true;
    private MenuOption SelectedOption { get; set; }

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

        SelectedOption = GetMenuOption();
    }

    private void HandleMenuSelection()
    {
        switch (SelectedOption)
        {
            case MenuOption.ShowAll:
            {
                Database.ShowAllEntries();
                break;
            }
            case MenuOption.Insert:
            {
                HabitModel model = GetHabitModel();
                Database.InsertModel(model);
                break;
            }
            case MenuOption.Delete:
            {
                Database.ShowAllEntries();

                string recordId = GetExistingRecordId();
                if (recordId.Trim().ToLower() == EscapeChar)
                {
                    break;
                }

                Database.Delete(recordId);
                break;
            }
            case MenuOption.Update:
            {
                Database.ShowAllEntries();

                string recordId = GetExistingRecordId();

                if (recordId.Trim().ToLower() == EscapeChar)
                {
                    break;
                }

                Console.WriteLine("Enter updated properties");
                HabitModel model = GetHabitModel();
                Database.Update(recordId, model);
                break;
            }
            case MenuOption.Exit:
            {
                IsAppRunning = false;
                break;
            }
        }

        if (SelectedOption != MenuOption.Exit)
        {
            PressKeyToContinue();
        }
    }

    private static void PressKeyToContinue()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    static string GetExistingRecordId()
    {
        Console.WriteLine("Enter the Id of the record (or press x to go back to menu): ");

        string recordId = GetUserInput();
        while (!Database.CheckIfRecordExists(recordId) && recordId != "x")
        {
            Console.WriteLine("Record with such id doesn't exist");
            recordId = GetUserInput();

            if (recordId == "x")
            {
                break;
            }
        }

        return recordId;
    }

    private static HabitModel GetHabitModel()
    {
        string date = GetDateInput();
        int quantity = GetQuantityInput();

        return new HabitModel(date, quantity);
    }

    private static MenuOption GetMenuOption()
    {
        while (true)
        {
            if (IsValidMenuOption(GetUserInput(), out var menuOption))
            {
                return menuOption;
            }
            
            Console.WriteLine("Enter valid menu option: ");
        }
    }
    
    private static bool IsValidMenuOption(string input, out MenuOption menuOption)
    {
        return Enum.TryParse(input, out menuOption) && Enum.IsDefined(typeof(MenuOption), menuOption);
    }

    private static string GetUserInput()
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