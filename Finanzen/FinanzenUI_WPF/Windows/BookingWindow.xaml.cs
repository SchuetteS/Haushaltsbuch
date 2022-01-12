using FinanzenLib;
using FinanzenLib.Data;
using FinanzenLib.Models;
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
using System.Windows.Shapes;

namespace FinanzenUI_WPF.Windows
{
    /// <summary>
    /// Interaktionslogik für BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        // Properties
        public BookingModel booking { get; set; }
        public BookingModel offsetBooking { get; set; }
        public List<BankAccountModel> BankAccounts { get; set; }
        public List<BankAccountModel> OffsetBankAccounts { get; set; }
        public List<BookingCategorieModel> BookingCategories { get; set; }

        // Constructors
        public BookingWindow(BookingModel b, bool isDuplicate = false)
        {
            InitializeComponent();

            // Check if b is a copy
            if (!isDuplicate)
            {
                booking = b;
                this.Title = "Buchung bearbeiten";
            }
            else
            {
                booking = new BookingModel();
                this.Title = "Buchung anlegen";
                booking.Date = b.Date;
                booking.Purpose = b.Purpose;
                booking.Amount = b.Amount;
                booking.CategoryID = b.CategoryID;
                booking.CategoryName = b.CategoryName;
                booking.Notice = b.Notice;
                booking.Kind = b.Kind;
            }
            setUpForm();

            DataContext = this;
        }

        public BookingWindow()
        {
            InitializeComponent();

            booking = new BookingModel();
            this.Title = "Buchung anlegen";
            setUpForm();

            booking.Date = DateTime.Now;

            DataContext = this;
        }

        // Methods
        private void setUpForm()
        {
            // alle Listen für Dropdowns füllen
            BankAccounts = SqlAnalysis.GetAllActiveBankAccounts();
            BookingCategories = SqlBookingCategories.GetAllBookingCategories();
            OffsetBankAccounts = SqlAnalysis.GetAllActiveBankAccounts();

            // Offset Bank Account verstecken
            cboOffsetBankAccount.IsEnabled = false;
            cboOffsetBankAccount.Visibility = Visibility.Hidden;
            txtOffsetBankAccount.Visibility = Visibility.Hidden;
        }

        private void updateBookingModel()
        {
            // Kategorie
            int i = cboCategory.SelectedIndex;
            booking.CategoryID = BookingCategories[i].ID;
            booking.CategoryName = BookingCategories[i].Name;

            // Bank Account
            i = cboBankAccount.SelectedIndex;
            booking.BankAccountID = BankAccounts[i].ID;

            // select Offset Bank Account
            if (chbOffsetBooking.IsChecked == true)
            {
                i = cboOffsetBankAccount.SelectedIndex;
                booking.OffsetBankAccountID = OffsetBankAccounts[i].ID;
                booking.IsOffsetBooking = true;
            }

            if (chbEinnahme.IsChecked == true)
            {
                booking.IsIncome = true;
                booking.Kind = "Einnahme";
            }
            else
            {
                booking.IsIncome = false;
                booking.Kind = "Ausgabe";
            }
        }

        private void saveBooking()
        {
            updateBookingModel();

            // Check if booking already exits and save or update it
            if (booking.ID > 0)
            {
                FinanzenLib.Data.SqlBooking.updateBooking(booking);
            }
            else
            {
                booking.ID = FinanzenLib.Data.SqlBooking.CreateAndReturnBooking(booking);
            }

            // Save OffsetBooking
            if (booking.IsOffsetBooking == true)
            {
                // Set up the Property OffsetBooking
                offsetBooking = setUpOffsetBooking();

                // Save OffsetBooking   
                offsetBooking.ID = FinanzenLib.Data.SqlBooking.CreateAndReturnBooking(offsetBooking);

                // update Booking -> add offsetBooking.ID
                booking.OffsetBookingID = offsetBooking.ID;
                FinanzenLib.Data.SqlBooking.updateBooking(booking);
            }
        }

        private BookingModel setUpOffsetBooking()
        {
            offsetBooking = new BookingModel(booking);
            //offsetBooking =  booking;

            // Make the Kind to opposite of booking
            if (booking.IsIncome == true)
            {
                offsetBooking.IsIncome = false;
                offsetBooking.Kind = "Ausgabe";
            }
            else
            {
                offsetBooking.IsIncome = true;
                offsetBooking.Kind = "Einnahme";
            }

            // Change the BookingIDs to opposite
            offsetBooking.OffsetBookingID = booking.ID;
            //booking.OffsetBookingID = offsetBooking.ID; // kann noch nicht gesetzt werden, da sie noch nicht existiert
            offsetBooking.ID = 0; // überschreibt die booking.ID

            // Change the BankAccountIDs to opposite 
            offsetBooking.BankAccountID = booking.OffsetBankAccountID;
            offsetBooking.OffsetBankAccountID = booking.BankAccountID;

            return offsetBooking;
        }

        // Steuerelemente
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            saveBooking();

            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cboOffsetBooking_Checked(object sender, RoutedEventArgs e)
        {
            cboOffsetBankAccount.IsEnabled = true;
            cboOffsetBankAccount.Visibility = Visibility.Visible;
            txtOffsetBankAccount.Visibility = Visibility.Visible;
        }

        private void cboOffsetBooking_Unchecked(object sender, RoutedEventArgs e)
        {
            cboOffsetBankAccount.IsEnabled = false;
            cboOffsetBankAccount.Visibility = Visibility.Hidden;
            txtOffsetBankAccount.Visibility = Visibility.Hidden;
        }
    }
}
