namespace PrintCalc.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Client { get; set; } = "";
        public string PlasticName { get; set; } = "";
        public double Cost { get; set; }      // себестоимость
        public double Price { get; set; }     // цена для клиента
        public double Profit { get; set; }    // чистая прибыль
        public string Status { get; set; } = "Новый"; // Новый / Оплачен
        public string Date { get; set; } = "";
    }
}
