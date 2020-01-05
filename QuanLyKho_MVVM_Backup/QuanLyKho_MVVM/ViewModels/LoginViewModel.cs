﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuanLyKho_MVVM.Views;
using QuanLyKho_MVVM.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QuanLyKho_MVVM.ViewModels
{
    class LoginViewModel : BaseViewModel
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

        public LoginViewModel()
        {
            Username = "admin";
            IsLogin = false;
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            ExitCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            PasswordBoxCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
        }

        public async void Login(Window window)
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                User user = null;
                //string name = from x in DataProvider.Instance.DB.Users
                //              where x.UserName.Equals(Username) && x.Password.Equals(Password)
                //              select x.DisplayName.First;
                await Task.Run(() =>
                {
                    user = DataProvider.Instance.DB.Users
                 .Where(x => x.UserName.Equals(username) && x.Password.Equals(password)).SingleOrDefault();
                });

                if (user != null)
                {
                    DisplayName = user.DisplayName;
                    idRole = user.IdRole;
                    IsLogin = true;
                    window.Close();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
