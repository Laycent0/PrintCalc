using Microsoft.Data.Sqlite;
using PrintCalc.Models;

namespace PrintCalc.Data
{
    public static class OrdersRepo
    {
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
    }
}