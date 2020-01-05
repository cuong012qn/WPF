using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MessageBox = System.Windows;

namespace QuanLyKho.ViewModels
{
    class LoginViewModel : Screen
    {
        private string username;
        private string password;

        public string Username
        {
            get => username;
            set
            {
                username = value;
                NotifyOfPropertyChange(() => username);
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                NotifyOfPropertyChange(() => password);
            }
        }

        public bool CanLogin(string username, string password)
        {
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }
        
        public void Login()
        {
            MessageBox.MessageBox.Show(username + " " + password);
        }
    }
}
