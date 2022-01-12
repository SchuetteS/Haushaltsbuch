using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Runtime.CompilerServices;
using System.Data;

namespace FinanzenLib.DataConnection
{
    public static class TextConnector
    {
        private static List<string> LoadFile(string file)
        {
            try
            {
                if (!File.Exists(file))
                {
                    return new List<string>();
                }
                return File.ReadAllLines(file).ToList();
            }
            catch (Exception e)
            {
                //Todo - Write Log, Fehlerbehandlung nur auf speziellen Fehler anpassen
            }
            return new List<string>();
        }

        public static List<string> LoadFileToDataTable(string filePath)
        {
            var input = LoadFile(filePath);
            var output = input;

            //foreach (string line in lines)
            //{
            //    string[] cols = line.Split(';');

            //    IsiUserModel u = new IsiUserModel();
            //    u.Benutzer = cols[0];
            //    u.VollstName = cols[1];
            //    u.Gruppe = cols[2];
            //    u.Email = cols[3];
            //    u.Ort = cols[4];
            //    u.OrgEinheit = cols[5];
            //    u.Gesperrt = cols[6];
            //    if (cols[7].Length > 0) { u.GueltigVon = DateTime.Parse(cols[7]); }
            //    if (cols[8].Length > 0) { u.GueltigBis = DateTime.Parse(cols[8]); }
            //    output.Add(u);
            //}

            return output;
        }

        //// Dateiimport
        //public static List<IsiUserModel> ImportIsiUser(string filePath)
        //{
        //    return LoadFile(filePath).ConvertToIsiUserModel();
        //}

        //public static List<IsiRollenModel> ImportIsiRollen(string filePath)
        //{
        //    return ConvertToIsiRollenModel(filePath);
        //}

        //public static List<ImanUserModel> ImportImanUser(string filePath)
        //{
        //    return ConvertToImanUserModel(filePath);
        //}

        //// Datei Konverter
        //private static List<IsiUserModel> ConvertToIsiUserModel(this List<string> lines)
        //{
        //    List<IsiUserModel> output = new List<IsiUserModel>();

        //    lines.RemoveAt(0);

        //    foreach (string line in lines)
        //    {
        //        string[] cols = line.Split(';');

        //        IsiUserModel u = new IsiUserModel();
        //        u.Benutzer = cols[0];
        //        u.VollstName = cols[1];
        //        u.Gruppe = cols[2];
        //        u.Email = cols[3];
        //        u.Ort = cols[4];
        //        u.OrgEinheit = cols[5];
        //        u.Gesperrt = cols[6];
        //        if (cols[7].Length > 0) { u.GueltigVon = DateTime.Parse(cols[7]); }
        //        if (cols[8].Length > 0) { u.GueltigBis = DateTime.Parse(cols[8]); }
        //        output.Add(u);
        //    }

        //    return output;
        //}

        private static void dummy()
        { }

    //    // Save as CSV
    //    public static void SaveCSVTextFile(List<ComparisonUserModel> data, string filepath)
    //    {
    //        string header = "ISI Kontoname;ISI Rolle;iMan Kontoname;iMan Rolle;Rollenbezeichnung;Zuordnungstyp;Fehlerbeschreibung;Benutzergruppe;Email;Ort;OrgEinheit;Unternehmen;iManKontoGueltigVon;iManKontoGueltigBis;iManKontoGesperrt;ISIKontoGueltigVon;ISIKontoGueltigBis;ISIKontoGesperrt;ISIRolleGueltigVon;ISIRolleGueltigBis;Verantworlich";

    //        using (var sw = new StreamWriter(filepath, false, Encoding.UTF8))
    //        {
    //            sw.WriteLine(CreateCSVTextFile(header, data));
    //        }
    //    }

    //    private static string CreateCSVTextFile<T>(string header, List<T> data)
    //    {
    //        // Generic List to CSV String
    //        // https://stackoverflow.com/questions/17698228/generic-list-to-csv-string

    //        var properties = typeof(T).GetProperties();
    //        var result = new StringBuilder();
    //        result.AppendLine(header);

    //        foreach (var row in data)
    //        {
    //            var values = properties.Select(p => p.GetValue(row, null))
    //                                   .Select(v => StringToCSVCell(Convert.ToString(v)));
    //            var line = string.Join(";", values);
    //            result.AppendLine(line);
    //        }

    //        return result.ToString();
    //    }

    //    private static string StringToCSVCell(string str)
    //    {
    //        bool mustQuote = (str.Contains(";") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
    //        if (mustQuote)
    //        {
    //            StringBuilder sb = new StringBuilder();
    //            sb.Append("\"");
    //            foreach (char nextChar in str)
    //            {
    //                sb.Append(nextChar);
    //                if (nextChar == '"')
    //                    sb.Append("\"");
    //            }
    //            sb.Append("\"");
    //            return sb.ToString();
    //        }

    //        return str;
    //    }
    }
}
