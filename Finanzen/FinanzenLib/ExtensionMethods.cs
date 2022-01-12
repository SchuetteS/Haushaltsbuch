using FinanzenLib.Models;
using System;

namespace FinanzenLib
{
    public static class ExtensionMethods
    {
        public static bool isLastDayOfMonth(this DateTime date)
        {
            if (date == date.lastDayOfActualMonth())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DateTime lastDayOfLastMonth(this DateTime date)
        // https://stackoverflow.com/a/1138180/14087514
        {
            var firstDayGivenMonth = new DateTime(date.Year, date.Month, 1);

            var lastDayLastMonth = firstDayGivenMonth.AddDays(-1);

            return lastDayLastMonth;
        }

        public static DateTime lastDayOfActualMonth(this DateTime date)
        {
            var firstDayGivenMonth = new DateTime(date.Year, date.Month, 1);

            var lastDayOfActualMonth = firstDayGivenMonth.AddMonths(1);
            lastDayOfActualMonth = lastDayOfActualMonth.AddDays(-1);

            return lastDayOfActualMonth;
        }

        public static DateTime lastDayOfNextMonth(this DateTime date)
        {
            var firstDayGivenMonth = new DateTime(date.Year, date.Month, 1);

            var lastDayOfNextMonth = firstDayGivenMonth.AddMonths(2);
            lastDayOfNextMonth = lastDayOfNextMonth.AddDays(-1);

            return lastDayOfNextMonth;
        }

        private static void changeIsIncome(this BookingModel b)
        {
            if (b.IsIncome == true)
            {
                b.IsIncome = false;
                b.Kind = "Ausgabe";
            }
            else
            {
                b.IsIncome = true;
                b.Kind = "Einnahmen";
            }
        }
    }
}
