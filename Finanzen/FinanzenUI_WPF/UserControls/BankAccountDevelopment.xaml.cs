using FinanzenLib.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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

namespace FinanzenUI_WPF.UserControls
{
    /// <summary>
    /// Interaktionslogik für BankAccountDevelopment.xaml
    /// </summary>
    public partial class BankAccountDevelopment : UserControl
    {
        // Basic Line Chart: https://www.lvcharts.net/App/examples/v1/wpf/Basic%20Line%20Chart

        // Properties
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        // Constructor
        public BankAccountDevelopment()
        {
            InitializeComponent();

            updateBankAccountDevelopment();

            DataContext = this;
        }

        public void updateBankAccountDevelopment()
        {
            SeriesCollection = new SeriesCollection();
            List<List<BankAccountModel>> BankAccountDevelopment = FinanzenLib.Data.SqlAnalysis.GetBankAccountsDevelopment();

            foreach (var list in BankAccountDevelopment)
            {
                // Daten für ChartValues erstellen
                decimal[] temporalCv = new decimal[list.Count()];
                int i = 0;
                foreach (var item in list)
                {
                    temporalCv[i++] = item.StartBalance;
                }

                // Daten in ChartValues umwandeln
                var cv = new ChartValues<decimal>();
                cv.AddRange(temporalCv);

                // neue Series Collection je Bank Account hinzufügen
                SeriesCollection.Add(new StackedAreaSeries
                {
                    Title = list[0].Name,
                    Values = cv
                }) ;
            }

            // Labels für die X-Achse ergänzen
            List<string> labelsList = new List<string>();

            foreach (BankAccountModel bankAccount in BankAccountDevelopment[0])
            {
                labelsList.Add(bankAccount.StartDate.ToString("MMM yy"));
            }

            //for (int i = 11; i >= 0; i--)
            //{
            //    DateTime date = DateTime.Now.AddMonths(-i);
            //    labelsList.Add(date.ToString("MMM yy"));
            //}

            Labels = labelsList.ToArray();
            YFormatter = value => value.ToString("C");
        }
    }
}
