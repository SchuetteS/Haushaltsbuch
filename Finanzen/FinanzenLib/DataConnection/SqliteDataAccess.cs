using Dapper;
//using Microsoft.VisualBasic.ApplicationServices;
//using Microsoft.VisualStudio.Debugger.Interop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FinanzenLib
{
    internal static class SqliteDataAccess
    {
        /// <summary>
        /// Returns the connection string to the Database, which is saved within the App.config.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        /// <summary>
        /// Excecutes an SQL statement to the DB.
        /// </summary>
        /// <param name="sqlStatement"></param>
        public static void DbDataOperation(string sqlStatement)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute(sqlStatement);
            }
        }

        /// <summary>
        /// Needs an SQL statement like Insert or Update and a Model, which is similar to the target table of the DB.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlStatement"></param>
        /// <param name="parameters"></param>
        public static void SaveData<T>(string sqlStatement, T parameters)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute(sqlStatement, parameters);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlStatement"></param>
        /// <returns></returns>
        public static int SaveAndReturnSingleData<T>(string sqlStatement, T parameters)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                return cnn.QuerySingle<int>(sqlStatement, parameters);
            }
        }

        /// <summary>
        /// Returns a List of the given Model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlStatement"></param>
        /// <returns></returns>
        public static List<T> LoadData<T>(string sqlStatement)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                List<T> rows = cnn.Query<T>(sqlStatement, new DynamicParameters()).ToList();
                return rows;
            }
        }

        /// <summary>
        /// Returns an instance of the given Model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlStatement"></param>
        /// <returns></returns>
        public static T LoadSingleDataset<T>(string sqlStatement)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {               
                return cnn.QuerySingleOrDefault<T>(sqlStatement, new DynamicParameters());
            }
        }
    }
}
