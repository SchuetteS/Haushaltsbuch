using FinanzenLib.Data;
using FinanzenLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace FinanzenUI_WPF.UserControls
{
    /// <summary>
    /// Interaktionslogik für SolidGaugeAvailableMoney.xaml
    /// </summary>
    public partial class SolidGaugeAvailableMoney : UserControl, INotifyPropertyChanged
    {
        // Properties
        private int _income;
        public int Income
        {
            get { return _income; }
            set
            {
                _income = value;
                OnPropertyChanged("Income");
            }
        }


        private int _remainder;
        public int Remainder 
        { 
            get { return _remainder; }
            set 
            {
                _remainder = value;
                OnPropertyChanged("Remainder");
            }
        }

        // Constructor
        public SolidGaugeAvailableMoney()
        {
            InitializeComponent();
        }

        // Functions
        public Func<Double, String> LabelFormatter { get; set; }

        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        // Methods
        public void update(DateTime dateVon, DateTime dateBis, string indexKonto)
        {
            // Summe der Einkommen  und Ausgaben abrufen
            List<BookingModel> IncomeAndSpending = new List<BookingModel>();
            IncomeAndSpending = SqlAnalysis.GetIncomeAndSpending(dateVon, dateBis, indexKonto);
            int Spending = 0;

            // Werte zuordnen
            foreach (BookingModel element in IncomeAndSpending)
            {
                if (element.Kind == "Einnahme")
                {
                    Income = Convert.ToInt32(element.Amount);   // Convert
                }
                if (element.Kind == "Ausgabe")
                {
                    Spending = (int)element.Amount;            // Cast
                }
            }

            // Restliches verfügbares Einkommen berechnen
            if (Income < Remainder)
            {
                Remainder = 0;
            }
            else
            {
                Remainder = Income - Spending;
            }
            
            // Anzeige der Daten
            //LabelFormatter = val => val.ToString("C");
            LabelFormatter = val => val + " €";

            DataContext = this;
        }
    }
}
