using System.Windows.Controls;
using PrintCalc.Data;

namespace PrintCalc.Views
{
    public partial class SummaryView : UserControl
    {
        public SummaryView()
        {
            InitializeComponent();
            LoadSummary();
        }

        // считаем итоги по базе и показываем в карточках
        private void LoadSummary()
        {
            var s = OrdersRepo.GetSummary();

            TxtCount.Text = s.Count.ToString();
            TxtRevenue.Text = $"{s.Revenue:F2} ₽";
            TxtTotalCost.Text = $"{s.Cost:F2} ₽";
            TxtTotalProfit.Text = $"{s.Profit:F2} ₽";
        }
    }
}