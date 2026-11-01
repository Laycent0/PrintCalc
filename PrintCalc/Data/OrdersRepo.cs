using Microsoft.Data.Sqlite;
using PrintCalc.Models;

namespace PrintCalc.Data;

public static class OrdersRepo
{
    public const string StatusNew = "Новый";
    public const string StatusPaid = "Оплачен";

    // сохраняет заказ из калькулятора в таблицу Orders
    public static void Add(Order o)
    {
        using var conn = new SqliteConnection(Db.ConnString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Orders (Client, PlasticName, Cost, Price, Profit, Status, Date)
            VALUES ($client, $plastic, $cost, $price, $profit, $status, $date)";
        cmd.Parameters.AddWithValue("$client", o.Client);
        cmd.Parameters.AddWithValue("$plastic", o.PlasticName);
        cmd.Parameters.AddWithValue("$cost", o.Cost);
        cmd.Parameters.AddWithValue("$price", o.Price);
        cmd.Parameters.AddWithValue("$profit", o.Profit);
        cmd.Parameters.AddWithValue("$status", o.Status);
        cmd.Parameters.AddWithValue("$date", o.Date);
        cmd.ExecuteNonQuery();
    }

    // все заказы, новые сверху
    public static List<Order> GetAll()
    {
        var list = new List<Order>();
        using var conn = new SqliteConnection(Db.ConnString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT Id, Client, PlasticName, Cost, Price, Profit, Status, Date
            FROM Orders
            ORDER BY Id DESC";
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new Order
            {
                Id = r.GetInt32(0),
                Client = r.GetString(1),
                PlasticName = r.GetString(2),
                Cost = r.GetDouble(3),
                Price = r.GetDouble(4),
                Profit = r.GetDouble(5),
                Status = r.GetString(6),
                Date = r.GetString(7)
            });
        }
        return list;
    }

    // отметить заказ оплаченным
    public static void MarkPaid(int id)
    {
        using var conn = new SqliteConnection(Db.ConnString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Orders SET Status = $status WHERE Id = $id";
        cmd.Parameters.AddWithValue("$status", StatusPaid);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    // удалить заказ
    public static void Delete(int id)
    {
        using var conn = new SqliteConnection(Db.ConnString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Orders WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    // итоги для экрана «Итоги»:
    // количество - все заказы, деньги - только по оплаченным
    public static (int Count, double Revenue, double Cost, double Profit) GetSummary()
    {
        using var conn = new SqliteConnection(Db.ConnString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT
                COUNT(*),
                COALESCE(SUM(CASE WHEN Status = $paid THEN Price  END), 0.0),
                COALESCE(SUM(CASE WHEN Status = $paid THEN Cost   END), 0.0),
                COALESCE(SUM(CASE WHEN Status = $paid THEN Profit END), 0.0)
            FROM Orders";
        cmd.Parameters.AddWithValue("$paid", StatusPaid);
        using var r = cmd.ExecuteReader();
        r.Read();
        return ((int)r.GetInt64(0), r.GetDouble(1), r.GetDouble(2), r.GetDouble(3));
    }
}
