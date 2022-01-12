using DataAccess;
using System.Linq;


namespace FinanzenLib.DataConnection
{
    public static class CsvHelper
    {
        /// <summary>
        /// Takes a CSV file and returns it as a DataTable.
        /// It determines the count of columns by the first 50 rows of the file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static System.Data.DataTable readCsv(string path)
        {
            // https://stackoverflow.com/a/23319377/14087514

            // Daten einlesen
            DataTable dt = DataTable.New.ReadLazy(path);


            // Spaltenanzahl ermitteln
            int columnCount = 0;
            int rowCount = dt.Rows.Count() > 50 ? 50 : dt.Rows.Count();

            for (int i = 0; i < rowCount; i++)
            {
                if (columnCount < dt.Rows.ElementAt(i).Values.Count())
                {
                    columnCount = dt.Rows.ElementAt(i).Values.Count();
                }
            }

            // DataTable als output definieren und Spalten erstellen.
            System.Data.DataTable output = new System.Data.DataTable();
            for (int i = 1; i <= columnCount; i++)
            {
                output.Columns.Add(new System.Data.DataColumn("Column " + i));
            }

            // Daten zum DataTable output hinzufügen
            foreach (Row row in dt.Rows)
            {
                output.Rows.Add(row.Values.ToArray());
            }

            // Daten zurückgeben
            return output;
        }
    }
}
