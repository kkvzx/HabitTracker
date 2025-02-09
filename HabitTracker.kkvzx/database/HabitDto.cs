
namespace HabitTracker.kkvzx.database;

public class HabitDto(int id, DateTime date, int quantity)
{
    public int Id { get; set; } = id;
    public DateTime Date { get; set; } = date;
    public int Quantity { get; set; } = quantity;
}