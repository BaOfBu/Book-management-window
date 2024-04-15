using Flora.ViewModel;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        private MyShopContext _shopContext;
        private LoginVM _loginVM { get; set; }
        public Login()
        {
            InitializeComponent();
            _loginVM = DataContext as LoginVM;
            //_shopContext = new MyShopContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // modified connection string and connect to database
            string server = ServerNameBox.Text;
            string database = DatabaseNameBox.Text;

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings["MyConnectionString"].ConnectionString = $"Server={server};Database={database};Trusted_Connection=True;TrustServerCertificate=True";
            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.RefreshSection("connectionStrings");

            // check if the connection is valid
            try
            {
                _shopContext = new MyShopContext();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed");
                return;
            }

            if (!_shopContext.Database.CanConnect()) { 
                MessageBox.Show("Connection failed");
                return;
            }

            // authenticate user
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;

            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropy = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }
            var encryptedPassword = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);

            var query = from user in _shopContext.UserAccounts
                        where user.Username == username
                        select user;
            var userAccount = query.FirstOrDefault();
            if (userAccount == null)
            {
                MessageBox.Show("Login failed");
                return;
            }

            if(userAccount.Entropy == null)
            {
                var passwordDTB = userAccount.Password;
                var entropyDTB = Convert.ToBase64String(entropy);
                if(password == passwordDTB)
                {
                    MessageBox.Show("Login successful");
                    if (remember.IsChecked == true)
                    {
                        config.AppSettings.Settings["username"].Value = username;
                        config.AppSettings.Settings["password"].Value = Convert.ToBase64String(encryptedPassword);
                        config.AppSettings.Settings["entropy"].Value = entropyDTB;
                        config.AppSettings.Settings["server"].Value = server;
                        config.AppSettings.Settings["database"].Value = database;
                        config.Save(ConfigurationSaveMode.Minimal);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    else
                    {
                        config.AppSettings.Settings["username"].Value = "";
                        config.AppSettings.Settings["password"].Value = "";
                        config.AppSettings.Settings["entropy"].Value = "";
                        config.AppSettings.Settings["server"].Value = "";
                        config.AppSettings.Settings["database"].Value = "";
                        config.Save(ConfigurationSaveMode.Minimal);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    userAccount.Entropy = entropyDTB;
                    userAccount.Password = Convert.ToBase64String(encryptedPassword);
                    _shopContext.SaveChanges();
                    Window mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Login failed");
                }   
            }
            else
            {
                var passwordDTBInByte = Convert.FromBase64String(userAccount.Password);
                var entropyDTBInByte = Convert.FromBase64String(userAccount.Entropy);
                var decryptedPasswordFromDTB = Encoding.UTF8.GetString(ProtectedData.Unprotect(passwordDTBInByte, entropyDTBInByte, DataProtectionScope.CurrentUser));
                if (password == decryptedPasswordFromDTB)
                {
                    MessageBox.Show("Login successful");
                    if (remember.IsChecked == true)
                    {
                        config.AppSettings.Settings["username"].Value = username;
                        config.AppSettings.Settings["password"].Value = Convert.ToBase64String(encryptedPassword);
                        config.AppSettings.Settings["entropy"].Value = Convert.ToBase64String(entropy);
                        config.AppSettings.Settings["server"].Value = server;
                        config.AppSettings.Settings["database"].Value = database;
                        config.Save(ConfigurationSaveMode.Minimal);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    else
                    {
                        config.AppSettings.Settings["username"].Value = "";
                        config.AppSettings.Settings["password"].Value = "";
                        config.AppSettings.Settings["entropy"].Value = "";
                        config.AppSettings.Settings["server"].Value = "";
                        config.AppSettings.Settings["database"].Value = "";
                        config.Save(ConfigurationSaveMode.Minimal);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    Window mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Login failed");
                }
            }




        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
