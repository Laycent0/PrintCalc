using System.Windows;
using System.Windows.Controls;
using PrintCalc.Data;
using PrintCalc.Helpers;
using PrintCalc.Models;

namespace PrintCalc.Views;

public partial class CalcView : UserControl
{
    // результаты последнего расчёта
    private double _cost, _price, _profit;
    private bool _calculated;

    public CalcView()
    {
        InitializeComponent();

        // подгружаем пластики в выпадающий список
        CmbPlastic.ItemsSource = PlasticsRepo.GetAll();
        CmbPlastic.SelectedIndex = 0;

        BtnCalc.Click += BtnCalc_Click;
        BtnSave.Click += BtnSave_Click;
    }

    private void BtnCalc_Click(object sender, RoutedEventArgs e)
    {
        var plastic = CmbPlastic.SelectedItem as Plastic;
        if (plastic is null)
        {
            MessageBox.Show("Выберите пластик");
            return;
        }

        if (!NumberParser.TryParse(TxtWeight.Text, out double weight) ||
            !NumberParser.TryParse(TxtHours.Text, out double hours) ||
            !NumberParser.TryParse(TxtPrinterRate.Text, out double rate) ||
            !NumberParser.TryParse(TxtDefect.Text, out double defect) ||
            !NumberParser.TryParse(TxtPost.Text, out double post) ||
            !NumberParser.TryParse(TxtPack.Text, out double pack) ||
            !NumberParser.TryParse(TxtMarkup.Text, out double markup) ||
            !NumberParser.TryParse(TxtCommission.Text, out double commission))
        {
            MessageBox.Show("Проверьте, что все поля заполнены числами");
            return;
        }

        double plasticCost = weight / 1000 * plastic.PricePerKg; // материал
        double printerCost = hours * rate;                       // работа принтера
        double defectCost = (plasticCost + printerCost) * defect / 100; // брак

        _cost = plasticCost + printerCost + defectCost + post + pack; // себестоимость
        _price = _cost * (1 + markup / 100);                          // цена клиенту
        double fee = _price * commission / 100;                       // комиссия площадки
        _profit = _price - _cost - fee;                               // чистая прибыль

        TxtCost.Text = $"{_cost:F2} ₽";
        TxtPrice.Text = $"{_price:F2} ₽";
        TxtFee.Text = $"{fee:F2} ₽";
        TxtProfit.Text = $"{_profit:F2} ₽";

        _calculated = true;
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (!_calculated)
        {
            MessageBox.Show("Сначала нажмите «Рассчитать стоимость»");
            return;
        }

        var plastic = (Plastic)CmbPlastic.SelectedItem;

        OrdersRepo.Add(new Order
        {
            Client = TxtClient.Text.Trim() == "" ? "Без имени" : TxtClient.Text.Trim(),
            PlasticName = plastic.Name,
            Cost = Math.Round(_cost, 2),
            Price = Math.Round(_price, 2),
            Profit = Math.Round(_profit, 2),
            Status = OrdersRepo.StatusNew,
            Date = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
        });

        MessageBox.Show("Заказ сохранён");
        _calculated = false;
    }
}
