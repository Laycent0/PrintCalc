using System.Windows;
using System.Windows.Controls;
using PrintCalc.Data;
using PrintCalc.Models;

namespace PrintCalc.Views;

public partial class OrdersView : UserControl
{
    public OrdersView()
    {
        InitializeComponent();

        BtnPaid.Click += BtnPaid_Click;
        BtnDelete.Click += BtnDelete_Click;

        LoadData();
    }

    // загружаем заказы из БД в таблицу
    private void LoadData()
    {
        GridOrders.ItemsSource = OrdersRepo.GetAll();
    }

    // отметить выбранный заказ оплаченным
    private void BtnPaid_Click(object sender, RoutedEventArgs e)
    {
        if (GridOrders.SelectedItem is not Order order)
        {
            MessageBox.Show("Выберите заказ в таблице");
            return;
        }

        if (order.Status == OrdersRepo.StatusPaid)
        {
            MessageBox.Show("Этот заказ уже оплачен");
            return;
        }

        OrdersRepo.MarkPaid(order.Id);
        LoadData();
    }

    // удалить выбранный заказ (с подтверждением)
    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (GridOrders.SelectedItem is not Order order)
        {
            MessageBox.Show("Выберите заказ в таблице");
            return;
        }

        var answer = MessageBox.Show(
            $"Удалить заказ клиента «{order.Client}» от {order.Date}?",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (answer != MessageBoxResult.Yes)
            return;

        OrdersRepo.Delete(order.Id);
        LoadData();
    }
}
