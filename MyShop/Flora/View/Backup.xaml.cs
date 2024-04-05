using DocumentFormat.OpenXml.InkML;
using Flora.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for Backup.xaml
    /// </summary>
    public partial class Backup : UserControl
    {
        private MyShopContext myShopContext;
        private BackupVM backupVM { get; set; }
        public Backup()
        {
            InitializeComponent();
            myShopContext = new MyShopContext();
            backupVM = DataContext as BackupVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            // read backup folder from config file ("C:/temp/")
            var currentPath = System.IO.Directory.GetCurrentDirectory();

            if(!System.IO.Directory.Exists(currentPath + "\\Backup"))
                System.IO.Directory.CreateDirectory(currentPath + "\\Backup");

            // set backupfilename (you will get something like: "C:/temp/MyDatabase-2013-12-07.bak")
            var backupFileName = String.Format("{0}\\{1}\\{2}-{3}.bak",
                currentPath,"Backup", "MyShop",
                DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"));

            const string query = @"
            BACKUP DATABASE @db
            TO DISK = @file;
            ";
            myShopContext.BackupDatabase(backupFileName);
        }

        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            var currentPath = System.IO.Directory.GetCurrentDirectory();

            // set backupfilename (you will get something like: "C:/temp/MyDatabase-2013-12-07.bak")
            var backupFileName = String.Format("{0}\\{1}\\{2}-{3}.bak",
                currentPath, "Backup", "MyShop",
                DateTime.Now.ToString("yyyy-MM-dd-"));
            var serverName = "localhost";
            var newDatabaseName = "MyShop";
            RestoreDatabase(serverName, "MyShop", backupFileName, newDatabaseName);
        }

        private void RestoreDatabase(string serverName, string databaseName, string backupFilePath, string newDatabaseName)
        {
            ServerConnection serverConnection = new ServerConnection(serverName);
            Server sqlServer = new Server(serverConnection);

            Restore restore = new Restore();
            restore.Database = newDatabaseName;
            restore.Action = RestoreActionType.Database;
            restore.Devices.AddDevice(backupFilePath, DeviceType.File);
            restore.ReplaceDatabase = true;


            sqlServer.KillAllProcesses(databaseName); // Kill existing connections
            restore.SqlRestore(sqlServer);

            MessageBox.Show("Database restored successfully.");
        }
    }
}
