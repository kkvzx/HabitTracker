namespace HabitTracker.kkvzx.database;

public static class HabitSeeder
{
    public static List<HabitModel> Seed(int elementsToSeed)
    {
        var random = new Random();
        List<HabitModel> records = new();

        for (int i = 0; i < elementsToSeed; i++)
        {
            var date = $"{random.Next(1, 28):00}-{random.Next(1, 12):00}-{random.Next(2000, 2200):0000}";
            var quantity = random.Next(0, 100);

            records.Add(new HabitModel(date, quantity));
        }

        return records;
    }
}