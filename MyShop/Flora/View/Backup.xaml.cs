using Flora.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
            var backupFolder = ConfigurationManager.AppSettings["BackupFolder"];

            // set backupfilename (you will get something like: "C:/temp/MyDatabase-2013-12-07.bak")
            var backupFileName = String.Format("{0}{1}-{2}.bak",
                backupFolder, "MyShop",
                DateTime.Now.ToString("yyyy-MM-dd"));

            const string query = @"
            BACKUP DATABASE @db
            TO DISK = @file;
            ";
            myShopContext.BackupDatabase(backupFileName);
        }
    }
}
