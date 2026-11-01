using Microsoft.Data.Sqlite;

namespace PrintCalc.Data;

public static class Db
{
    // файл базы лежит рядом с exe
    public const string ConnString = "Data Source=printcalc.db";

    public static void Init()
    {
        using var conn = new SqliteConnection(ConnString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Plastics (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                PricePerKg REAL NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Orders (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Client TEXT NOT NULL,
                PlasticName TEXT NOT NULL,
                Cost REAL NOT NULL,
                Price REAL NOT NULL,
                Profit REAL NOT NULL,
                Status TEXT NOT NULL,
                Date TEXT NOT NULL
            );";
        cmd.ExecuteNonQuery();

        // если пластиков нет - добавляем по умолчанию
        cmd.CommandText = "SELECT COUNT(*) FROM Plastics";
        long count = (long)cmd.ExecuteScalar()!;
        if (count == 0)
        {
            cmd.CommandText = @"
                INSERT INTO Plastics (Name, PricePerKg) VALUES
                ('PLA', 1500), ('PETG', 1700), ('ABS', 1600), ('TPU', 2500);";
            cmd.ExecuteNonQuery();
        }
    }
}
