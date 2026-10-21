using System.Windows;
using PrintCalc.Views;

namespace PrintCalc
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new CalcView();
        }

        private void BtnCalc_Click(object sender, RoutedEventArgs e)     => MainContent.Content = new CalcView();
        private void BtnPlastics_Click(object sender, RoutedEventArgs e) => MainContent.Content = new PlasticsView();
        private void BtnOrders_Click(object sender, RoutedEventArgs e)   => MainContent.Content = new OrdersView();
        private void BtnSummary_Click(object sender, RoutedEventArgs e)  => MainContent.Content = new SummaryView();
    }
}
