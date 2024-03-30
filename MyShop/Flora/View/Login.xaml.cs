using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        private MyShopContext _shopContext;
        public Login()
        {
            InitializeComponent();
            _shopContext = new MyShopContext();
        }
        private void LoadConfig()
        {
            var usernameInConfig = ConfigurationManager.AppSettings["username"];
            var passwordInConfig = ConfigurationManager.AppSettings["password"];
            var entropyInConfig = ConfigurationManager.AppSettings["entropy"];

            if (string.IsNullOrEmpty(usernameInConfig) || string.IsNullOrEmpty(passwordInConfig) || string.IsNullOrEmpty(entropyInConfig))
            {
                return;
            }
            var passwordInByte = Convert.FromBase64String(passwordInConfig);
            var entropyInByte = Convert.FromBase64String(entropyInConfig);

            var decryptedPassword = ProtectedData.Unprotect(passwordInByte, entropyInByte, DataProtectionScope.CurrentUser);
            string password = Encoding.UTF8.GetString(decryptedPassword);
            UsernameTextBox.Text = usernameInConfig;
            PasswordTextBox.Password = password;

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;


            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
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
                    config.Save(ConfigurationSaveMode.Minimal);
                    ConfigurationManager.RefreshSection("appSettings");
                    MessageBox.Show("Username and password saved");
                }
                else
                {
                    config.AppSettings.Settings["username"].Value = "";
                    config.AppSettings.Settings["password"].Value = "";
                    config.AppSettings.Settings["entropy"].Value = "";
                    config.Save(ConfigurationSaveMode.Minimal);
                    ConfigurationManager.RefreshSection("appSettings");
                    MessageBox.Show("Username and password cleared");
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
}
