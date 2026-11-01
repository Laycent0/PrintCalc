using System.Windows;
using System.Windows.Controls;
using PrintCalc.Data;
using PrintCalc.Helpers;
using PrintCalc.Models;

namespace PrintCalc.Views;

public partial class PlasticsView : UserControl
{
    public PlasticsView()
    {
        InitializeComponent();

        BtnAddPlastic.Click += BtnAddPlastic_Click;
        BtnSavePlastics.Click += BtnSavePlastics_Click;

        LoadData();
    }

    // загружаем список пластиков из БД в таблицу
    private void LoadData()
    {
        GridPlastics.ItemsSource = PlasticsRepo.GetAll();
    }

    // добавить новый пластик
    private void BtnAddPlastic_Click(object sender, RoutedEventArgs e)
    {
        string name = TxtNewName.Text.Trim();

        bool priceOk = NumberParser.TryParse(TxtNewPrice.Text, out double price);

        if (name == "" || !priceOk || price <= 0)
        {
            MessageBox.Show("Введите название и цену больше нуля");
            return;
        }

        PlasticsRepo.Add(name, price);

        TxtNewName.Text = "";
        TxtNewPrice.Text = "";
        LoadData();
    }

    // сохранить изменения, сделанные прямо в таблице
    private void BtnSavePlastics_Click(object sender, RoutedEventArgs e)
    {
        // завершаем редактирование ячейки, если оно ещё идёт
        GridPlastics.CommitEdit(DataGridEditingUnit.Row, true);

        var list = (List<Plastic>)GridPlastics.ItemsSource;
        foreach (var p in list)
        {
            PlasticsRepo.Update(p);
        }

        MessageBox.Show("Сохранено");
    }
}
