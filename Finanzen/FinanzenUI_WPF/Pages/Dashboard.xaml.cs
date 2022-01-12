using FinanzenLib;
using FinanzenLib.Data;
using FinanzenLib.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FinanzenUI_WPF.Pages
{
    /// <summary>
    /// Interaktionslogik für Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        // Constructor
        public Dashboard()
        {
            InitializeComponent();

            SetUpFilters();

            DataContext = this;

            updateDashboard();
        }

        // Properties
        public List<BankAccountModel> BankAccounts { get; set; }
        public DateTime dateVon { get; set; }
        public DateTime dateBis { get; set; }

        // Methods
        private void SetUpFilters()
        {
            BankAccounts = SqlAnalysis.GetAllActiveBankAccounts();
            BankAccounts.Insert(0, new BankAccountModel { Name = "Bilanz" });

            var FirstDayOfThisMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            dateVon = FirstDayOfThisMonth.AddMonths(-1);
            dateBis = FirstDayOfThisMonth.AddDays(-1);
        }

        private void updateDashboard()
        {
            string indexKonto = "%";

            if (KontoComboBox.SelectedIndex <= 0)
            {
                indexKonto = "%";
            }

            if (KontoComboBox.SelectedIndex > 0)
            {
                indexKonto = KontoComboBox.SelectedIndex.ToString();
            }

            TopFive.updatePieChartCategories(dateVon, dateBis, indexKonto);
            AvailableMoney.update(dateVon, dateBis, indexKonto);
        }

        // Buttons
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            updateDashboard();
        }

        private void btnMonthBack_Click(object sender, RoutedEventArgs e)
        {            
            datePickerVon.SelectedDate = dateVon.AddMonths(-1);
            datePickerBis.SelectedDate = dateBis.lastDayOfLastMonth();
        }

        private void btnMonthNext_Click(object sender, RoutedEventArgs e)
        {
            datePickerVon.SelectedDate = dateVon.AddMonths(1);
            datePickerBis.SelectedDate = dateBis.lastDayOfNextMonth();
        }
    }
}
