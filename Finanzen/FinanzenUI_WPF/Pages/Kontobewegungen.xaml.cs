using FinanzenLib;
using FinanzenLib.Data;
using FinanzenLib.Models;
using FinanzenUI_WPF.Windows;
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

namespace FinanzenUI_WPF.Pages
{
    /// <summary>
    /// Interaktionslogik für Kontobewegungen.xaml
    /// </summary>
    public partial class Kontobewegungen : Page
    {
        public Kontobewegungen()
        {
            InitializeComponent();

            SetUpFilters();

            DataContext = this;
        }

        public List<BankAccountModel> Konten { get; set; }
        public DateTime dateVon { get; set; }
        public DateTime dateBis { get; set; }
        private List<BookingModel> bookings { get; set; }

        private void SetUpFilters()
        {
            Konten = SqlAnalysis.GetAllActiveBankAccounts();
            Konten.Insert(0, new BankAccountModel { Name = "Bilanz" });

            var FirstDayOfThisMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            dateVon = FirstDayOfThisMonth.AddMonths(-1);
            dateBis = FirstDayOfThisMonth.AddDays(-1);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            updateDashboard();
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

            bookings = FinanzenLib.Data.SqlBooking.GetAllBookings(dateVon, dateBis, indexKonto);

            dgBookings.DataContext = null;
            dgBookings.DataContext = bookings;

            // Fußzeile
            lblNumber.Text = "Anzahl: " + bookings.Count;

            decimal sum = 0;
            for (int i = 0; i < bookings.Count; i++)
            {
                if (bookings[i].Kind == "Einnahme")
                {
                    sum += bookings[i].Amount;
                }
                else
                {
                    sum -= bookings[i].Amount;
                }
            }
            lblSum.Text = "Summe: " + sum.ToString("C");
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

        private void btnAddBooking_Click(object sender, RoutedEventArgs e)
        {
            BookingWindow frm = new BookingWindow();
            frm.Show();

            updateDashboard();
        }

        private void btnEditBooking_Click(object sender, RoutedEventArgs e)
        {
            BookingModel b;
            b = (BookingModel)dgBookings.SelectedItem;

            if (b != null)
            {
                BookingWindow frm = new BookingWindow(b);
                frm.Show();
                updateDashboard();
            }
            else
            {
                MessageBox.Show("Es ist kein Eintrag markiert.", "Achtung", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCopyBooking_Click(object sender, RoutedEventArgs e)
        {
            BookingModel b;
            b = (BookingModel)dgBookings.SelectedItem;

            if (b != null)
            {
                BookingWindow frm = new BookingWindow(b, true);
                frm.Show();
                updateDashboard();
            }
            else
            {
                MessageBox.Show("Es ist kein Eintrag markiert.", "Achtung", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }

        private void btnDeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookings.SelectedItems.Count > 0)
            {
                List<BookingModel> bookings = new List<BookingModel>();
                for (int i = 0; i < dgBookings.SelectedItems.Count; i++)
                {
                    bookings.Add((BookingModel)dgBookings.SelectedItems[i]);
                }

                string anzahl = "Anzahl: " + bookings.Count;

                MessageBoxResult result = MessageBox.Show("Markierte Einträge wirklich löschen?" + Environment.NewLine + anzahl,
                                          "Achtung",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    return;
                }

                SqlBooking.deleteBookings(bookings);

                MessageBox.Show("Die markierten Einträge wurden gelöscht." + Environment.NewLine + anzahl, 
                                "Gelöscht", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Information);

                updateDashboard();
            }
        }

        private void btnConnectBookings_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookings.SelectedItems.Count == 2)
            {
                BookingModel booking = (BookingModel)dgBookings.SelectedItems[0];
                BookingModel offsetBooking = (BookingModel)dgBookings.SelectedItems[1];

                bool connected = connectOffsetBookings(booking, offsetBooking);

                if (connected)
                {
                    MessageBox.Show("Buchungen wurden zu Umbuchung verknüpft.",
                                    "Umbuchung erstellt",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                    updateDashboard();
                }
                else
                {
                    MessageBox.Show("Buchungen wurden nicht zu Umbuchung verknüpft.",
                                    "Umbuchung nicht erstellt",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                }
                
            }
            else
            {
                MessageBox.Show("Es können immer nur zwei Buchungen zu einer Umbuchung verknüpft werden. Bitte zwei Buchungen auswählen.",
                                "Umbuchung erstellen fehlgeschlagen",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
            }
            
        }

        private bool connectOffsetBookings(BookingModel booking, BookingModel offsetBooking)
        {
            bool areOpposite = true;

            // Check if one is Income and the other ist expense
            if (booking.Kind == offsetBooking.Kind)
            {
                areOpposite = false;
            }
            // Check if the amount is the same
            if (booking.Amount != offsetBooking.Amount)
            {
                areOpposite = false;
            }
            // Check if the BankAccounts are not the same
            if (booking.BankAccountID != offsetBooking.BankAccountID)
            {
                areOpposite = false;
            }
            // Check if none is already OffsetBooking
            if (booking.IsOffsetBooking == true || offsetBooking.IsOffsetBooking == true)
            {
                areOpposite = false;
            }

            if (areOpposite)
            {
                booking.IsOffsetBooking = true;
                offsetBooking.IsOffsetBooking = true;

                booking.OffsetBookingID = offsetBooking.ID;
                offsetBooking.OffsetBookingID = booking.ID;

                booking.OffsetBankAccountID = offsetBooking.BankAccountID;
                offsetBooking.OffsetBankAccountID = booking.BankAccountID;

                FinanzenLib.Data.SqlBooking.updateBooking(booking);
                FinanzenLib.Data.SqlBooking.updateBooking(offsetBooking);

                return true;
            }

            return false;
        }
    }
}
