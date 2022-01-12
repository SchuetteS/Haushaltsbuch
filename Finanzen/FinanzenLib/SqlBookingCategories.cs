using FinanzenLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzenLib
{
    public static class SqlBookingCategories
    {
        // Create

        // Read

        /// <summary>
        /// Returns a list of all Booking Categories.
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public static List<BookingCategorieModel> GetAllBookingCategories(string parentID = "%")
        {
            var sqlStatement = new StringBuilder();

            sqlStatement.Append("SELECT * ");
            sqlStatement.Append("FROM BookingCategories ");
            sqlStatement.Append($"WHERE ParentID LIKE '{ parentID }';");


            var output = SqliteDataAccess.LoadData<BookingCategorieModel>(sqlStatement.ToString());
            return output;
        }

        // Update

        // Delete
    }
}
