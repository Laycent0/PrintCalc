using Microsoft.Data.Sqlite;
using PrintCalc.Models;

namespace PrintCalc.Data;

public static class PlasticsRepo
{
    public static List<Plastic> GetAll()
    {
        var list = new List<Plastic>();
        using var conn = new SqliteConnection(Db.ConnString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, PricePerKg FROM Plastics";
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new Plastic
            {
                Id = r.GetInt32(0),
                Name = r.GetString(1),
                PricePerKg = r.GetDouble(2)
            });
        }
        return list;
    }

    public static void Add(string name, double price)
    {
        using var conn = new SqliteConnection(Db.ConnString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Plastics (Name, PricePerKg) VALUES ($n, $p)";
        cmd.Parameters.AddWithValue("$n", name);
        cmd.Parameters.AddWithValue("$p", price);
        cmd.ExecuteNonQuery();
    }

    public static void Update(Plastic p)
    {
        using var conn = new SqliteConnection(Db.ConnString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Plastics SET Name = $n, PricePerKg = $p WHERE Id = $id";
        cmd.Parameters.AddWithValue("$n", p.Name);
        cmd.Parameters.AddWithValue("$p", p.PricePerKg);
        cmd.Parameters.AddWithValue("$id", p.Id);
        cmd.ExecuteNonQuery();
    }
}
