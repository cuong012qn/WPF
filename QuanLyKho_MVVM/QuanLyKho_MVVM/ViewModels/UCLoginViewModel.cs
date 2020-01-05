using QuanLyKho_MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace QuanLyKho_MVVM.ViewModels
{
    class UCLoginViewModel : BaseViewModel
    {
        private string username = string.Empty;
        private string password = string.Empty;
        private string displayName = string.Empty;
        public int idRole;
        public bool IsLogin { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand PasswordBoxCommand { get; set; }

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }
        public string DisplayName
        {
            get => displayName;
            set
            {
                displayName = value;
                OnPropertyChanged();
            }
        }

        public UCLoginViewModel()
        {
            Username = "admin";
            IsLogin = false;
            LoginCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => { Login(p); });
            ExitCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
            PasswordBoxCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
        }

        public async void Login(UserControl window)
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                bool isError = false;
                User user = null;
                await Task.Run(() =>
                {
                    try
                    {
                        user = DataProvider.Instance.DB.Users
                     .Where(x => x.UserName.Equals(username) && x.Password.Equals(password)).SingleOrDefault();
                    }
                    catch (Exception e)
                    {
                        isError = true;
                        //MessageBox.Show(e.Message);
                    }
                });

                if (!isError)
                {
                    if (user != null)
                    {
                        idRole = user.IdRole;
                        IsLogin = true;
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        //window.Close();
                    }
                    else
                    {
                        //MessageBox.Show("Sai tài khoản hoặc mật khẩu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
