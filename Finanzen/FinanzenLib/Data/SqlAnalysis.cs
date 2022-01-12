using FinanzenLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanzenLib.Data
{
    public static class SqlAnalysis
    {
        /// <summary>
        /// Returns a List of two values, the Income and the Spending during the given period and for the given BankAccount.
        /// </summary>
        /// <returns></returns>
        public static List<BookingModel> GetIncomeAndSpending(DateTime dateVon, DateTime dateBis, string indexKonto)
        {
            var sqlStatement = new StringBuilder();

            sqlStatement.Append("SELECT Kind, sum(Amount) as Amount ");
            sqlStatement.Append("FROM Bookings ");
            sqlStatement.Append($"WHERE BookingDate >= datetime('{ dateVon.ToString("yyyy-MM-dd") }') ");
            sqlStatement.Append($"AND BookingDate <= datetime('{ dateBis.ToString("yyyy-MM-dd") }') ");
            sqlStatement.Append($"AND BankAccountID LIKE \"{ indexKonto }\" ");
            sqlStatement.Append("AND IsOffsetBooking = 0 ");
            sqlStatement.Append("GROUP BY Kind;");


            var output = SqliteDataAccess.LoadData<BookingModel>(sqlStatement.ToString());
            return output;
        }

        /// <summary>
        /// Returns a List of the top 5 categories of a given timespan for the selected Konto. Ignores all offset bookings.
        /// </summary>
        /// <param name="dateVon"></param>
        /// <param name="dateBis"></param>
        /// <param name="indexKonto"></param>
        /// <returns></returns>
        public static List<BookingModel> GetTopBookingCategoriesSums(string dateVon, string dateBis, string indexKonto)
        {
            var sqlStatement = new StringBuilder();

            sqlStatement.Append("SELECT CategoryName, Kind, round(sum(Amount), 2) AS Amount ");
            sqlStatement.Append("FROM Bookings ");
            sqlStatement.Append("WHERE Kind LIKE \"Ausgabe\" ");
            if (indexKonto != null)
            {
                sqlStatement.Append($"AND BookingDate >= datetime('{ dateVon }') AND BookingDate <= datetime('{ dateBis }') ");
                sqlStatement.Append($"AND BankAccountID LIKE \"{ indexKonto }\" ");
                sqlStatement.Append($"AND IsOffsetBooking = 0 "); // Keine Umbuchung von Konto A nach Konto B
            }
            sqlStatement.Append("GROUP by CategoryName ");
            sqlStatement.Append("ORDER BY Amount DESC ");
            sqlStatement.Append("LIMIT 5; ");

            var output = SqliteDataAccess.LoadData<BookingModel>(sqlStatement.ToString());
            return output;
        }

        /// <summary>
        /// Returns a list of all active Bank Accounts.
        /// </summary>
        /// <returns></returns>
        public static List<BankAccountModel> GetAllActiveBankAccounts()
        {
            var sqlStatement = new StringBuilder();

            sqlStatement.Append("SELECT * ");
            sqlStatement.Append("FROM BankAccounts ");
            sqlStatement.Append("WHERE IsActive = 1;");

            var output = SqliteDataAccess.LoadData<BankAccountModel>(sqlStatement.ToString());
            return output;
        }

        /// <summary>
        /// Returns a list of bookings which are aggregated per month.
        /// The only values are Amount and BookingDate.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="bankAccountId"></param>
        /// <param name="preBookingDate"></param>
        /// <returns></returns>
        private static List<BookingModel> GetBookingSumsPerPeriod(DateTime date, int bankAccountId, bool preBookingDate)
        {
            // Festlegen ob Daten vor oder nach dem Startdatum des BankAccounts abgerufen werden
            string comparisonOperator = ">";
            string sorting = "ASC";

            if (preBookingDate)
            {
                comparisonOperator = "<";
                sorting = "DESC";
            }

            // Daten abfragen
            var sqlStatement = new StringBuilder();

            // Liste mit je zwei Einträgen pro Monat, Einnahme und Ausgabe
            //sqlStatement.Append("SELECT sum(amount) as Amount, Kind, strftime('%m-%Y', BookingDate) as BookingDate ");
            //sqlStatement.Append("FROM Bookings ");
            //sqlStatement.Append($"WHERE BookingDate { comparisonOperator } '{ date }' and BankAccountID = { bankAccountId } ");
            //sqlStatement.Append("GROUP BY Kind, strftime('%m-%Y', BookingDate) ");
            //sqlStatement.Append($"ORDER BY BookingDate { sorting }; ");

            sqlStatement.Append("SELECT sum(Amount) as Amount, BookingDate ");
            sqlStatement.Append("FROM ( ");
            sqlStatement.Append("SELECT CASE Kind WHEN 'Einnahme' THEN Amount ELSE Amount * -1 END Amount, ");
            sqlStatement.Append("strftime('%Y-%m', BookingDate) as BookingDate ");
            sqlStatement.Append("FROM Bookings ");
            sqlStatement.Append($"WHERE BookingDate { comparisonOperator } '{ date.ToString("yyyy-MM-dd") }' ");
            sqlStatement.Append($"AND BankAccountID = { bankAccountId }) ");
            sqlStatement.Append("GROUP BY BookingDate ");
            sqlStatement.Append($"ORDER BY BookingDate { sorting }; ");

            var output = SqliteDataAccess.LoadData<BookingModel>(sqlStatement.ToString());

            if (preBookingDate)
            {
                foreach (var item in output)
                {
                    item.Amount *= -1;
                }
            }

            return output;
        }

        /// <summary>
        /// This returns a series of monthly account balance for multiple bank accounts.
        /// </summary>
        /// <returns></returns>
        public static List<List<BankAccountModel>> GetBankAccountsDevelopment()
        {
            List<List<BankAccountModel>> bankAccountsDevelopment = new List<List<BankAccountModel>>();

            // Alle BankAccounts abfragen
            List<BankAccountModel> bankAccounts = GetAllActiveBankAccounts();


            // Alle BankAccounts einzeln durchgehen
            foreach (var bankAccount in bankAccounts)
            {
                BankAccountModel startBankBalance = new BankAccountModel(bankAccount);

                List<BankAccountModel> bankAccountDevelopment = startBankBalance.getBankBalances();

                bankAccountsDevelopment.Add(bankAccountDevelopment);
            }

            return bankAccountsDevelopment.normalizeLists();
        }

        /// <summary>
        /// It fills the list of BankAccountModel with the historical balances, if an initial entry is given.
        /// </summary>
        /// <param name="bankAccountDevelopment"></param>
        /// <param name="startBankBalance"></param>
        /// <param name="preStartDate"></param>
        /// <returns></returns>
        private static List<BankAccountModel> getBankBalances(this BankAccountModel startBankBalance)
        {
            List<BankAccountModel> bankAccountDevelopment = new List<BankAccountModel>();
            bool preStartDate = true;

            for (int goThrough = 1; goThrough <= 2; goThrough++)
            {
                if (goThrough == 2) { preStartDate = false; }

                //bankAccountDevelopment.Insert(0, new BankAccountModel(startBankBalance));
                bankAccountDevelopment.Add(new BankAccountModel(startBankBalance));

                // Buchungssummen abrufen, die vor oder nach dem StartDate des BankAccounts liegen
                List<BookingModel> bookingSumsPerPeriodBackward = GetBookingSumsPerPeriod(startBankBalance.StartDate,
                                                                                          startBankBalance.ID,
                                                                                          preStartDate);

                if (bookingSumsPerPeriodBackward.Count > 0)
                {
                    // Falls die jeweils ersten Einträge der Listen aus dem selben Monat sind
                    if (bookingSumsPerPeriodBackward[0].Date.Year == bankAccountDevelopment[bankAccountDevelopment.Count - 1].StartDate.Year &&
                        bookingSumsPerPeriodBackward[0].Date.Month == bankAccountDevelopment[bankAccountDevelopment.Count - 1].StartDate.Month)
                    {
                        bankAccountDevelopment[bankAccountDevelopment.Count - 1].StartDate = bookingSumsPerPeriodBackward[0].Date;
                        bankAccountDevelopment[bankAccountDevelopment.Count - 1].StartBalance += bookingSumsPerPeriodBackward[0].Amount;
                        bookingSumsPerPeriodBackward.RemoveAt(0);
                    }

                    if (bookingSumsPerPeriodBackward.Count > 0)
                    {
                        // Zeitspanne in Monaten berechnen
                        int length = bookingSumsPerPeriodBackward.Count();
                        int timeSpan = timeSpanInMonths(bankAccountDevelopment[bankAccountDevelopment.Count - 1].StartDate,
                                                        bookingSumsPerPeriodBackward[length - 1].Date);
                        int j = 0;

                        if (goThrough == 1)
                        {
                            length = 0;
                        }

                        // Zeitspanne in Monaten durchlaufen und Einträge erzeugen
                        for (int i = 0; i < timeSpan; i++)
                        {
                            BankAccountModel b = new BankAccountModel(bankAccountDevelopment[bankAccountDevelopment.Count - 1]);

                            // Grenzwerte festlegen
                            var yourDate = bookingSumsPerPeriodBackward[j].Date;
                            var lowerBoundYear = bankAccountDevelopment[bankAccountDevelopment.Count - 1].StartDate.AddMonths(-1).Year;
                            var lowerBoundMonth = bankAccountDevelopment[bankAccountDevelopment.Count - 1].StartDate.AddMonths(-1).Month;
                            var upperBoundYear = bankAccountDevelopment[bankAccountDevelopment.Count - 1].StartDate.AddMonths(+1).Year;
                            var upperBoundMonth = bankAccountDevelopment[bankAccountDevelopment.Count - 1].StartDate.AddMonths(+1).Month;

                            // Es müssen immer die aktuellsten Einträge verglichen werden
                            if ((yourDate.Year == lowerBoundYear && yourDate.Month == lowerBoundMonth) ||
                                (yourDate.Year == upperBoundYear && yourDate.Month == upperBoundMonth))
                            {
                                b.StartDate = bookingSumsPerPeriodBackward[j].Date;
                                b.StartBalance += bookingSumsPerPeriodBackward[j].Amount;
                                j++;
                            }
                            else
                            {
                                int addMonth = preStartDate ? -1 : 1;
                                b.StartDate = b.StartDate.AddMonths(addMonth);
                            }

                            bankAccountDevelopment.Add(b);
                        }
                    }
                }

                bankAccountDevelopment = bankAccountDevelopment.correctDates(preStartDate);
            }

            // https://stackoverflow.com/a/3309230/14087514
            // List<Order> SortedList = objListOrder.OrderBy(o=>o.OrderDate).ToList();
            // List<Order> SortedList = objListOrder.OrderByDescending(o=>o.OrderDate).ToList();
            List<BankAccountModel> SortedList = bankAccountDevelopment.OrderBy(o => o.StartDate).ToList();

            // Ergebnis zurückgeben
            return SortedList;
        }

        /// <summary>
        /// Changes the StartDate of each item 
        /// </summary>
        /// <param name="bankAccountDevelopment"></param>
        /// <param name="preStartDate"></param>
        /// <returns></returns>
        private static List<BankAccountModel> correctDates (this List<BankAccountModel> bankAccountDevelopment, bool preStartDate)
        {
            // Prüfen ob Liste Inhalt hat
            if (bankAccountDevelopment.Count == 0)
            {
                return bankAccountDevelopment;
            }

            // Datum korrigieren
            foreach (var item in bankAccountDevelopment)
            {
                if (!item.StartDate.isLastDayOfMonth())
                {
                    if (preStartDate)
                    {
                        item.StartDate = item.StartDate.AddDays(-1);
                    }
                    else
                    {
                        item.StartDate = item.StartDate.lastDayOfActualMonth();
                    }
                }
            }

            return bankAccountDevelopment;
        }

        /// <summary>
        /// Returns the difference between two dates in months.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="newDate"></param>
        /// <returns></returns>
        private static int timeSpanInMonths(DateTime date1, DateTime date2)
        {
            if (date1 < date2)
            {
                DateTime temp = date1;
                date1 = date2;
                date2 = temp;
            }
            return ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month;
        }

        /// <summary>
        /// Takes the list of lists of bankAccountsDevelopment and sorts each inner list by date.
        /// It also adds elements to each list, till they all have the same amount of elements.
        /// </summary>
        /// <param name="bankAccountsDevelopment"></param>
        /// <returns></returns>
        private static List<List<BankAccountModel>> normalizeLists(this List<List<BankAccountModel>> bankAccountsDevelopment)
        {
            List<List<BankAccountModel>> sortedBankAccountsDevelopment = new List<List<BankAccountModel>>();

            // ältesten und jüngsten Eintrag ermitteln
            //DateTime newest = bankAccountsDevelopment[0][bankAccountsDevelopment[0].Count - 1].StartDate;
            DateTime newest = DateTime.Today;

            foreach (var list in bankAccountsDevelopment)
            {
                if (newest < list[list.Count -1].StartDate)
                {
                    newest = list[list.Count - 1].StartDate;
                }
            }

            // Listen normalisieren
            foreach (var list in bankAccountsDevelopment)
            {            
                int length = list.Count;

                // Listen bis zum akutellsten Datum verlängern
                if (list[length - 1].StartDate < newest)
                {
                    int timeSpan = timeSpanInMonths(newest, list[length - 1].StartDate);
                    for (int i = 0; i < timeSpan; i++)
                    {
                        BankAccountModel b = new BankAccountModel(list[list.Count - 1]);

                        b.StartDate = b.StartDate.AddMonths(1);

                        list.Add(b);
                    }
                }

                length = list.Count;

                // Falls weniger als 12 Einträge vorhanden sind, leere Einträge hinzufügen
                if (length < 12)
                {
                    // füge Datensätze am Ende hinzu
                    for (int i = length; i < 12; i++)
                    {
                        BankAccountModel b = new BankAccountModel(list[0]);

                        b.StartDate = b.StartDate.AddMonths(-1);

                        //SortedList.Add(b);
                        list.Insert(0, b);
                    }
                }

                // Falls Liste < 12 Einträge, älteste Einträge entfernen
                if (length > 12)
                {
                    for (int i = 12; i < length; i++)
                    {
                        // Immer den ersten und damit ältesten Eintrag entfernen
                        list.RemoveAt(0);
                    }
                }

                sortedBankAccountsDevelopment.Add(list);
            }

            return sortedBankAccountsDevelopment;
        } 
    }
}
