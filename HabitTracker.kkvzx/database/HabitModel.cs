namespace HabitTracker.kkvzx.database;

public class HabitModel(string date, int quantity)
{
        public string Date { get; set; } = date;
        public int Quantity { get; set; } = quantity;
}