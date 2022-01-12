using FinanzenLib.DataConnection;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace FinanzenUI_WPF.Pages
{
    /// <summary>
    /// Interaktionslogik für Import.xaml
    /// </summary>
    public partial class Import : Page
    {
        public Import()
        {
            InitializeComponent();
        }

        private void selectFile_Click(object sender, RoutedEventArgs e)
        {            
            string path = string.Empty;
            path = GetFileName();

            if (path != string.Empty)
            {
                DataTable dt = CsvHelper.readCsv(path);

                dgImportBookings.ItemsSource = dt.AsDataView();
            }
        }

        private void importSelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void updateRules_Click(object sender, RoutedEventArgs e)
        {

        }

        private static string GetFileName()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "csv files (*.csv)|*.csv"; //"txt files(*.txt)| *.txt | All files(*.*) | *.* ";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
            }

            return filePath;
        }

        private static string GetFolderPath()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            string path = string.Empty;

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                path = dialog.SelectedPath;
            }

            return path;
        }
    }
}
