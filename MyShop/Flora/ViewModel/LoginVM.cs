using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Configuration;

namespace Flora.ViewModel
{
    class LoginVM : Utilities.ViewModelBase
    {
        private MyShopContext _shopContext;
        
        public string usernameText { get; set; }
        public string passwordText { get; set; }
        public LoginVM()
        {
            _shopContext = new MyShopContext();
            LoadConfig();
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
            usernameText = usernameInConfig;
            passwordText = password;

        }
    }
}
