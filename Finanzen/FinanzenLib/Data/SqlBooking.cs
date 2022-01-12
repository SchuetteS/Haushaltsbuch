using FinanzenLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzenLib.Data
{
    public static class SqlBooking
    {
        //create
        public static void CreateBooking(BookingModel booking)
        {
            var sqlStatement = new StringBuilder();

            sqlStatement.Append("INSERT INTO Bookings (BookingDate, Purpose, Amount, CategoryName, CategoryID, Notice, Kind, IsIncome, UserID, RegularBookingID, BankAccountID, DocumentID, IsOffsetBooking, IsSplitBooking, OffsetBookingID, OffsetBankAccountID) ");
            sqlStatement.Append("VALUES (@BookingDate, @Purpose, @Amount, @CategoryName, @CategoryID, @Notice, @Kind, @IsIncome, @UserID, @RegularBookingID, @BankAccountID, @DocumentID, @IsOffsetBooking, @IsSplitBooking, @OffsetBookingID, @OffsetBankAccountID); ");

            SqliteDataAccess.SaveData<BookingModel>(sqlStatement.ToString(), booking);
        }

        /// <summary>
        /// Returns the ID of the created dataset.
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public static int CreateAndReturnBooking(BookingModel booking)
        {
            var sqlStatement = new StringBuilder();

            sqlStatement.Append("INSERT INTO Bookings (BookingDate, Purpose, Amount, CategoryName, CategoryID, Notice, Kind, IsIncome, UserID, RegularBookingID, BankAccountID, DocumentID, IsOffsetBooking, IsSplitBooking, OffsetBookingID, OffsetBankAccountID) ");
            sqlStatement.Append("VALUES (@BookingDate, @Purpose, @Amount, @CategoryName, @CategoryID, @Notice, @Kind, @IsIncome, @UserID, @RegularBookingID, @BankAccountID, @DocumentID, @IsOffsetBooking, @IsSplitBooking, @OffsetBookingID, @OffsetBankAccountID); ");
            sqlStatement.Append("SELECT last_insert_rowid();");

            return SqliteDataAccess.SaveAndReturnSingleData<BookingModel>(sqlStatement.ToString(), booking);
        }

        //read
        public static List<BookingModel> GetAllBookings(DateTime dateVon, DateTime dateBis, string indexKonto)
        {            
            var sqlStatement = new StringBuilder();

            sqlStatement.Append("SELECT b.*, ba.name as BankAccountName ");
            sqlStatement.Append("FROM Bookings b ");
            sqlStatement.Append("JOIN BankAccounts ba on b.BankAccountID = ba.ID ");
            sqlStatement.Append($"WHERE BookingDate >= datetime('{ dateVon.ToString("yyyy-MM-dd") }') ");
            sqlStatement.Append($"AND BookingDate <= datetime('{ dateBis.ToString("yyyy-MM-dd") }') ");
            sqlStatement.Append($"AND BankAccountID LIKE \"{ indexKonto }\" ");
            sqlStatement.Append("ORDER BY BookingDate DESC; ");

            var output = SqliteDataAccess.LoadData<BookingModel>(sqlStatement.ToString());
            return output;
        }

        //update
        public static void updateBooking(BookingModel booking)
        {
            var sqlStatement = new StringBuilder();

            sqlStatement.Append("UPDATE Bookings ");

            sqlStatement.Append($"SET BookingDate = '{ booking.BookingDate }', ");
            sqlStatement.Append($"Purpose = '{ booking.Purpose }', ");
            sqlStatement.Append($"Amount = '{ booking.Amount }', ");
            sqlStatement.Append($"CategoryName = '{ booking.CategoryName }', ");
            sqlStatement.Append($"CategoryID = { booking.CategoryID }, ");
            sqlStatement.Append($"Notice = '{ booking.Notice }', ");
            sqlStatement.Append($"Kind = '{ booking.Kind }', ");
            sqlStatement.Append($"IsIncome = '{ booking.IsIncome }', ");
            sqlStatement.Append($"UserID = { booking.UserID }, ");
            sqlStatement.Append($"RegularBookingID = { booking.RegularBookingID }, ");
            sqlStatement.Append($"BankAccountID = { booking.BankAccountID }, ");
            sqlStatement.Append($"DocumentID = { booking.DocumentID }, ");
            sqlStatement.Append($"IsOffsetBooking = { booking.IsOffsetBooking }, "); 
            sqlStatement.Append($"IsSplitBooking = { booking.IsSplitBooking }, ");
            sqlStatement.Append($"OffsetBookingID = { booking.OffsetBookingID }, "); 
            sqlStatement.Append($"OffsetBankAccountID = { booking.OffsetBankAccountID } "); 

            sqlStatement.Append($"WHERE ID = { booking.ID };");

            SqliteDataAccess.DbDataOperation(sqlStatement.ToString());
        }

        //delete
        public static void deleteBooking(BookingModel booking)
        {
            var sqlStatement = new StringBuilder();

            sqlStatement.Append("DELETE FROM Bookings ");
            sqlStatement.Append($"WHERE ID = { booking.ID };");

            SqliteDataAccess.DbDataOperation(sqlStatement.ToString());
        }

        public static void deleteBookings(List<BookingModel> bookings)
        {
            var sqlStatement = new StringBuilder();

            sqlStatement.Append("DELETE FROM Bookings ");
            sqlStatement.Append($"WHERE ID = { bookings[0].ID } ");

            for (int i = 1; i < bookings.Count; i++)
            {
                sqlStatement.Append($"OR ID = { bookings[i].ID } ");
            }

            SqliteDataAccess.DbDataOperation(sqlStatement.ToString());
        }
    }
}
