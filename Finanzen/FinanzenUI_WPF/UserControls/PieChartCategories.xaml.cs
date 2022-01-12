using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FinanzenLib.Data;
using FinanzenLib.Models;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace FinanzenUI_WPF.UserControls
{
    /// <summary>
    /// Interaktionslogik für PieChartCategories.xaml
    /// </summary>
    public partial class PieChartCategories : UserControl
    {

        public PieChartCategories()
        {
            InitializeComponent();

            //setPieChartCategories();

            DataContext = this;
        }

        public SeriesCollection Serie { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }

        //
        public void setPieChartCategories(DateTime dateVon, DateTime dateBis, string indexKonto)
        {
            // Daten aus der Datenbank abrufen
            List<BookingModel> InOutcome = new List<BookingModel>();
            InOutcome = SqlAnalysis.GetTopBookingCategoriesSums(dateVon.ToString("yyyy-MM-dd"), dateBis.ToString("yyyy-MM-dd"), indexKonto);

            // ggf. bestehende SeriesCollection leeren
            Serie = null;
            if (Serie != null)
            {
                Serie.Clear();
            }

            // Neue SeriesCollection aus den abgerufenen Daten erstellen
            Serie = new SeriesCollection();
            foreach (var item in InOutcome)
            {
                Serie.Add(new PieSeries
                {
                    Title = item.CategoryName,
                    Values = new ChartValues<decimal> { item.Amount },
                    DataLabels = true,
                    LabelPoint = point => point.Y + " €"
                });
            }

            // Chart updaten
            TopCategoriesPieChart.Update(true, true);
        }




        public void updatePieChartCategories(DateTime dateVon, DateTime dateBis, string indexKonto)
        {
            // Daten aus der Datenbank abrufen
            List<BookingModel> InOutcome = new List<BookingModel>();
            InOutcome = SqlAnalysis.GetTopBookingCategoriesSums(dateVon.ToString("yyyy-MM-dd"), dateBis.ToString("yyyy-MM-dd"), indexKonto);

            // ggf. bestehende SeriesCollection leeren
            TopCategoriesPieChart.Series = null;

            // Neue SeriesCollection aus den abgerufenen Daten erstellen
            TopCategoriesPieChart.Series = new SeriesCollection();
            foreach (var item in InOutcome)
            {
                TopCategoriesPieChart.Series.Add(new PieSeries
                {
                    Title = item.CategoryName,
                    Values = new ChartValues<decimal> { item.Amount },
                    DataLabels = true,
                    LabelPoint = point => point.Y + " €"
                });
            }

            // Chart updaten
            TopCategoriesPieChart.Update(true, true);
        }





        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
            {
                series.PushOut = 0;
            }

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        private void TopCategoriesPieChart_DataClick(object sender, ChartPoint chartPoint)
        {
            MessageBox.Show(chartPoint.SeriesView.Title.ToString());
        }
    }
}
