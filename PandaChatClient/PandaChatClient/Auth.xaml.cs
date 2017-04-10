using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using PandaChatClient.Class;
using static PandaChatClient.ClassConteiner;

namespace PandaChatClient
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAll())
                return;
            ClassConteiner.Server.Tunnel.Function.SendLine(Properties.Settings.Default.VersionClient);
            string Status = ClassConteiner.Server.Tunnel.Function.ReciveLine();
            if (Status == "UPDATENEED")
            {
                UpdateWin update = new UpdateWin();
                update.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                update.Owner = this;
                update.ShowDialog();
            }
            string StatusPassword = ClassConteiner.Server.Tunnel.Function.ReciveLine();
            if (StatusPassword == "PASSWORDSERVER")
            {
                ClassConteiner.Server.Tunnel.Function.SendLine(PasswordText.Password);
                if (ClassConteiner.Server.Tunnel.Function.ReciveLine() == "NOTTRUEPASSWORD")
                {
                    MessageClass.Password_Server_Error();
                    ClassConteiner.Server.Tunnel.Info.CloseThread();
                    ClassConteiner.Server.server.Close();
                    return;
                }
            }
            string ipCurrent = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
            string MDPasswordConvert = MD5Class.ConverPassword(PasswordTextUser.Password);
            ClassConteiner.Server.Tunnel.Function.SendLine(LoginText.Text + '/' + MDPasswordConvert + '/' + ipCurrent);
            string StatusConnection = ClassConteiner.Server.Tunnel.Function.ReciveLine();
            if (StatusConnection == "OKCONNECTION")
            {
                ClassConteiner.Content.isConnect = true;
                ClassConteiner.UserInfo.userLogin = LoginText.Text;
                this.Close();
            }
            else if (StatusConnection == "SAMEUSER")
            {
                MessageClass.Same_User_Error();
                ClassConteiner.Server.Tunnel.Info.CloseThread();
                ClassConteiner.Server.server.Close();
            }
            else if (StatusConnection == "NOTVALID")
            {
                MessageClass.Validate_User_Error();
                ClassConteiner.Server.Tunnel.Info.CloseThread();
                ClassConteiner.Server.server.Close();
            }
        }

        private void MouseEventLets(object sender, MouseEventArgs e)
        {
            if (((Label) sender).Name == "ExitFormLabel")
                ((Label) sender).Background = new SolidColorBrush(Colors.Red);
            else
                ((Label) sender).Background = new SolidColorBrush(Colors.Turquoise);
        }

        private void MouseEventLetsLeave(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            ((Label) sender).Background = (Brush) bc.ConvertFrom("#00FFFFFF");
        }

        private void ExitLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(1);
        }

        private void MinimasedLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void LoginText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                button_Click(sender, e);
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox) sender).SelectedIndex == 0)
            {
                ClassConteiner.Server.IP = IPAddress.Parse("127.0.0.1");
                IPAuthText.Text = "127.0.0.1";
                IPAuthText.IsEnabled = false;
                IPAuthText.IsReadOnly = true;
                PortAuthText.IsEnabled = false;
                PortAuthText.IsReadOnly = true;
            }
            else if (((ComboBox) sender).SelectedIndex == 1)
            {
                ClassConteiner.Server.IP = IPAddress.Parse("10.0.3.129");
                IPAuthText.Text = "10.0.3.129";
                IPAuthText.IsEnabled = false;
                IPAuthText.IsReadOnly = true;
                PortAuthText.IsEnabled = false;
                PortAuthText.IsReadOnly = true;
            }
            else if (((ComboBox) sender).SelectedIndex == 2)
            {
                IPAuthText.IsEnabled = true;
                IPAuthText.IsReadOnly = false;
                PortAuthText.IsEnabled = true;
                PortAuthText.IsReadOnly = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("updater.exe"))
                File.Delete("updater.exe");
            if (File.Exists("changelog.txt"))
                File.Delete("Cchangelog.txt");
            VersionLabel.Content = Properties.Settings.Default.VersionClient;
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAll())
                return;
            if (ClassConteiner.Server.Tunnel.Function.ReciveLine() == "PASSWORDSERVER")
            {
                MessageClass.Register_Error();
                return;
            }
            string ipCurrent = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
            string MDPasswordConvert = MD5Class.ConverPassword(PasswordTextUser.Password);
            ClassConteiner.Server.Tunnel.Function.SendLine("REGISTERUSER" + '/' + LoginText.Text + '/' + MDPasswordConvert + '/' + ipCurrent);
            string StatusConnection = ClassConteiner.Server.Tunnel.Function.ReciveLine();
            switch (StatusConnection)
            {
                case "REGOK":
                    MessageClass.Register_OK();
                    ClassConteiner.Server.Tunnel.Info.CloseThread();
                    ClassConteiner.Server.server.Close();
                    break;
                case "REGERROR":
                    MessageClass.Same_User_Error();
                    ClassConteiner.Server.Tunnel.Info.CloseThread();
                    ClassConteiner.Server.server.Close();
                    break;
            }
        }

        private bool CheckAll()
        {
            if (ChoseServer.SelectedIndex == -1)
            {
                MessageClass.Choose_Error();
                return false;
            }
            else if (LoginText.Text.Length >= 12)
            {
                MessageClass.Length_Auth_Error();
                return false;
            }
            bool isFindRegWord = false;
            foreach (var userRegisterWord in RegisterWord.Word)
            {
                if (LoginText.Text == userRegisterWord || LoginText.Text.ToUpper() == userRegisterWord.ToUpper() ||
                    LoginText.Text.ToLower() == userRegisterWord.ToLower())
                {
                    if (PasswordText.Password == "morozov")
                        isFindRegWord = false;
                    else
                        isFindRegWord = true;
                    break;
                }
            }
            if (isFindRegWord)
            {
                MessageClass.RegWord_Error();
                isFindRegWord = false;
                return false;
            }
            if (String.IsNullOrEmpty(LoginText.Text) || String.IsNullOrEmpty(PasswordTextUser.Password))
            {
                MessageClass.Empty_Field();
                return false;
            }
            if (ChoseServer.SelectedIndex == 0)
            {
                ClassConteiner.Server.IP = IPAddress.Parse("127.0.0.1");
                ClassConteiner.Server.PORT = 6666;
            }
            else if (ChoseServer.SelectedIndex == 2)
            {
                ClassConteiner.Server.IP = IPAddress.Parse(IPAuthText.Text);
                ClassConteiner.Server.PORT = int.Parse(PortAuthText.Text);
            }
            else if (ChoseServer.SelectedIndex == 1)
            {
                ClassConteiner.Server.IP = IPAddress.Parse("10.0.3.129");
                ClassConteiner.Server.PORT = int.Parse(PortAuthText.Text);
            }
            ClassConteiner.Server.server = new TcpClient();
            try
            {
                ClassConteiner.Server.server.Connect(ClassConteiner.Server.IP, ClassConteiner.Server.PORT);
            }
            catch
            {
                MessageClass.Connect_Error();
                return false;
            }
            ClassConteiner.Server.Tunnel.Info.SetupServerThread();
            return true;
        }

        private void comboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox boxAccount = (ComboBox) sender;
            if (boxAccount.SelectedIndex == 0)
            {
                LoginText.Text = "123";
                PasswordTextUser.Password = "morozov";
            }
            else if (boxAccount.SelectedIndex == 1)
            {
                LoginText.Text = "1234";
                PasswordTextUser.Password = "morozov";
            }
            else if (boxAccount.SelectedIndex == 2)
            {
                LoginText.Text = "";
                PasswordTextUser.Password = "";
            }
        }
    }
}