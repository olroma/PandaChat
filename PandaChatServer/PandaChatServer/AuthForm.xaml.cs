using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using PandaChatServer.Class;

namespace PandaChatServer
{
    /// <summary>
    /// Логика взаимодействия для AuthForm.xaml
    /// </summary>
    public partial class AuthForm : Window
    {
        public AuthForm()
        {
            InitializeComponent();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            bool[] Value = new bool[2];
            // [0] - Логин или пароль, [1] - Проверка на админа
            if (String.IsNullOrEmpty(LoginText.Text) || String.IsNullOrEmpty(PasswordText.Password) || String.IsNullOrEmpty(IPAuthText.Text) || String.IsNullOrEmpty(PortAuthText.Text))
            {
                MessageBox.Show("Не все поля заполены!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            FileInfo userDB = new FileInfo("UserInfo.txt");
            Server.InfoServer.DataUserBase = new DataBase(userDB);
            string ConvertedPassword = MD5Class.ConverPassword(PasswordText.Password);
            Dictionary<string, string> validateAdminServer = Server.InfoServer.DataUserBase.ValidateUser(LoginText.Text, ConvertedPassword);
            if (validateAdminServer["ValidateLogin"] == "false" || validateAdminServer["ValidateData"] == "false")
                Value[0] = false;
            else
                Value[0] = true;
            if (validateAdminServer["isAdmin"] == "false")
                Value[1] = false;
            else
                Value[1] = true;
            if (!Value[0])
            {
                MessageBox.Show("Пользователь не найден!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Value[1])
            {
                MessageBox.Show("Вы не имеете права на запуск сервера!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                Server.InfoServer.IP = IPAuthText.Text;
                Server.InfoServer.PORT = int.Parse(PortAuthText.Text);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); return; }
            BooleanQuastion.isTrueData = true;
            if (isRemember.IsChecked ?? false)
            {
                Properties.Settings.Default.Login = LoginText.Text;
                Properties.Settings.Default.Password = PasswordText.Password;
            }
            else
            {
                Properties.Settings.Default.Login = string.Empty;
                Properties.Settings.Default.Password = string.Empty;
            }
            Properties.Settings.Default.IP = IPAuthText.Text;
            Properties.Settings.Default.PORT = int.Parse(PortAuthText.Text);
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("PandaChatServer.bak"))
                File.Delete("PandaChatServer.bak");
            if (File.Exists("UpdaterCHAT.exe"))
                File.Delete("UpdaterCHAT.exe");
            IPAuthText.Text = Properties.Settings.Default.IP;
            PortAuthText.Text = Properties.Settings.Default.PORT.ToString();
            if (Properties.Settings.Default.Login != string.Empty)
                isRemember.IsChecked = true;
            LoginText.Text = Properties.Settings.Default.Login;
            PasswordText.Password = Properties.Settings.Default.Password;
        }
    }
}
