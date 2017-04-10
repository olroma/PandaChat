using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using PandaChatClient.Class;

namespace PandaChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool isSend = false;

        public MainWindow()
        {
            InitializeComponent();
            Auth authForm = new Auth();
            authForm.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!ClassConteiner.Content.isConnect)
                Environment.Exit(1);
            string[] bufferUserOnline = ClassConteiner.Server.Tunnel.Function.ReciveLine().Split('/');
            foreach (string t in bufferUserOnline)
            {
                if (t == ClassConteiner.UserInfo.userLogin)
                    NameUserLabel.Content = t;
                else
                    UserListOnline.Items.Add(t);
            }
            LogText.AppendText("Соединение с сервером установлено!\r");
            Paragraph ph = new Paragraph();
            ColorText.IniColorText(LogText, ph);
            Thread listenServer = new Thread(ListenThread);
            ClassConteiner.Server.serverSocket = ClassConteiner.Server.server.Client;
            listenServer.Start();
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSend)
            {
                ColorText.ColorLog(LogText, "[Ошибка] ", "У вас передается файл!", Brushes.OrangeRed);
                SendMessageText.Clear();
                return;
            }
            if (String.IsNullOrEmpty(SendMessageText.Text.Trim()))
                return;
            if (SendMessageText.Text.Trim() == "!help")
            {
                for (int i = 0; i < Command.HelpComman.Length; i++)
                    LogText.AppendText(Command.HelpComman[i] + '\r');
                SendMessageText.Clear();
                return;
            }
            string[] bufferedText = SendMessageText.Text.Trim().Split(' ');
            if (bufferedText[0] == "!sendfile")
            {
                SendFile(bufferedText[1], bufferedText[2]);
                SendMessageText.Clear();
                return;
            }
            else if (bufferedText[0] == "!modclear")
            {
                ClassConteiner.Server.Tunnel.Function.SendLine("CLEARMODER");
                SendMessageText.Clear();
                return;
            }
            else if (bufferedText[0] == "!modkick")
            {
                if (bufferedText.Length < 2)
                {
                    ColorText.ColorLog(LogText, "[Ошибка] ", "Использование: !modkick <ник>!", Brushes.OrangeRed);
                    return;
                }
                else if (bufferedText[1] == ClassConteiner.UserInfo.userLogin)
                {
                    ColorText.ColorLog(LogText, "[Ошибка] ", "Нельзя кикнуть самого себя", Brushes.OrangeRed);
                    return;
                }
                ClassConteiner.Server.Tunnel.Function.SendLine("KICKMODER");
                ClassConteiner.Server.Tunnel.Function.SendLine(bufferedText[1]);
                SendMessageText.Clear();
                return;
            }
            else if (bufferedText[0] == "!mod")
            {
                LogText.AppendText("Команды модератора: \r");
                foreach (var command in ModeratorCommand.Command)
                    LogText.AppendText(command + '\r');
                SendMessageText.Clear();
                return;
            }
            else if (bufferedText[0] == "!ls")
            {
                if (bufferedText.Length < 2)
                {
                    ColorText.ColorLog(LogText, "[Ошибка] ", "Использование: !ls <ник> <сообщение>", Brushes.OrangeRed);
                    SendMessageText.Clear();
                    return;
                }
                else if (bufferedText[1] == ClassConteiner.UserInfo.userLogin)
                {
                    ColorText.ColorLog(LogText, "[Ошибка] ", "Невозможно отправить личное сообщение самому себе", Brushes.OrangeRed);
                    SendMessageText.Clear();
                    return;
                }
                for (int i = 0; i < UserListOnline.Items.Count; i++)
                {
                    if (UserListOnline.Items[i].ToString() == bufferedText[1])
                    {
                        string nameToLS = bufferedText[1];
                        string messageToLS = SendMessageText.Text.Trim().Substring(SendMessageText.Text.Trim().IndexOf(UserListOnline.Items[i].ToString()) + nameToLS.Length + 1);
                        ClassConteiner.Server.Tunnel.Function.SendLine("PERSMESSAGE");
                        string toSendLS = bufferedText[1] + '/' + messageToLS;
                        ClassConteiner.Server.Tunnel.Function.SendLine(toSendLS);
                        ColorText.ColorLog(LogText, "ЛС[Вы -> " + nameToLS + "] ", messageToLS, Brushes.Cyan);
                        SendMessageText.Clear();
                        return;
                    }
                }
                ColorText.ColorLog(LogText, "[Ошибка] ", "Не найден пользователь: " + bufferedText[1], Brushes.Cyan);
                SendMessageText.Clear();
                return;
            }
            Thread.Sleep(3);
            ClassConteiner.Server.Tunnel.Function.SendLine("SANDMESSAGE");
            Thread.Sleep(5);
            ClassConteiner.Server.Tunnel.Function.SendLine(SendMessageText.Text);
            LogText.AppendText("<Вы> " + SendMessageText.Text + '\r');
            SendMessageText.Clear();
        }

        public void ListenThread()
        {
            string buffer;
            while (ClassConteiner.Content.isConnect)
            {
                try
                {
                    buffer = ClassConteiner.Server.Tunnel.Function.ReciveLine();
                }
                catch { break; }
                switch (buffer)
                {
                    case "SERVERMESSAGESENDING":
                        {
                            string serverMessage = ClassConteiner.Server.Tunnel.Function.ReciveLine();
                            string statusMessage = ClassConteiner.Server.Tunnel.Function.ReciveLine();
                            ColorText.ColorLog(LogText, "<Ole}i{a> " + serverMessage, Brushes.Purple);
                            if (statusMessage != "NORMAL")
                            {
                                string nameBuffer = serverMessage.Split(' ')[2];
                                ClassConteiner.Content.delListUser(nameBuffer, UserListOnline);
                            }
                            break;
                        }
                    case "SENDFILEFROMSERVER":
                        {
                            ReciveFile();
                            break;
                        }
                    case "SERVERCLOSECONNECTION":
                        {
                            ClassConteiner.Server.Tunnel.Info.CloseThread();
                            ClassConteiner.Server.server.Close();
                            ClassConteiner.Content.isConnect = false;
                            break;
                        }
                    case "KICKUSER":
                    case "BANUSER":
                        {
                            string Reason = ClassConteiner.Server.Tunnel.Function.ReciveLine();
                            if (buffer == "KICKUSER")
                                MessageClass.Kick_User(Reason);
                            else
                                MessageClass.Ban_User(Reason);
                            Environment.Exit(1);
                            break;
                        }
                    case "ONLINENEW":
                        {
                            string userName = ClassConteiner.Server.Tunnel.Function.ReciveLine();
                            ClassConteiner.Content.setListUser(userName, UserListOnline);
                            ColorText.ColorLog(LogText, "[...] ", "Пользователь " + userName + " присоединился к чату", Brushes.Green);
                            break;
                        }
                    case "DELETEUSERONLINE":
                        {
                            string userName = ClassConteiner.Server.Tunnel.Function.ReciveLine();
                            ClassConteiner.Content.delListUser(userName, UserListOnline);
                            ColorText.ColorLog(LogText, "[...] ", "Пользователь " + userName + " остоединился", Brushes.OrangeRed);
                            break;
                        }
                    case "PERSONALMESSAGE":
                        {
                            string messagePersonalbuffer = ClassConteiner.Server.Tunnel.Function.ReciveLine();
                            string nameFrom = messagePersonalbuffer.Split('/')[0];
                            string messageLS = messagePersonalbuffer.Split('/')[1];
                            ColorText.ColorLog(LogText, "ЛС[" + nameFrom + " -> Вы] ", messageLS, Brushes.Cyan);
                            break;
                        }
                    case "BOTSENNDING":
                        {
                            string messageFromBot = ClassConteiner.Server.Tunnel.Function.ReciveLine();
                            ColorText.ColorLog(LogText, "[ChatBOT] ", messageFromBot, Brushes.Magenta);
                            break;
                        }
                    case "PERMISSIONDENNIED":
                        {
                            ColorText.ColorLog(LogText, "[Модератор] ", "У вас нет разрешений для выполнения этой команды", Brushes.Purple);
                            break;
                        }
                    case "REQUESTSEND":
                        {
                            if (MessageBox.Show("Хотите принять файл?", "Запрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                ClassConteiner.Server.Tunnel.Function.SendLine("OKTRANSFER");
                            else
                                ClassConteiner.Server.Tunnel.Function.SendLine("NONETRANSFER");
                            break;
                        }
                    case "OKROGER":
                        {
                            TransferFile.isOk = true;
                            break;
                        }
                    case "CLEARSCREEN":
                        {
                            ClassConteiner.Content.Cls(LogText);
                            ColorText.ColorLog(LogText, "[Модератор] ", "Чат был очищен", Brushes.Purple);
                            break;
                        }
                    default:
                        {
                            ClassConteiner.Content.setLog(buffer, LogText);
                            break;
                        }
                }
            }
            MessageBox.Show("Сервер закрыл соединение!", "Сервер", MessageBoxButton.OK, MessageBoxImage.Information);
            Environment.Exit(1);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (ClassConteiner.Content.isConnect)
            {
                ClassConteiner.Server.Tunnel.Function.SendLine("CLOSECONNECTION");
                ClassConteiner.Server.Tunnel.Info.reader.Close();
                ClassConteiner.Server.Tunnel.Info.writer.Close();
                ClassConteiner.Server.Tunnel.Info.stream.Close();
                ClassConteiner.Server.server.Close();
            }
        }

        private void MouseEventLets(object sender, MouseEventArgs e)
        {
            SolidColorBrush colroBrush = new SolidColorBrush { Opacity = 0.5 };
            if (((Label)sender).Name == "ExitFormLabel")
            {
                colroBrush.Color = Colors.Red;
                ((Label)sender).Background = colroBrush;
            }
            else
            {
                colroBrush.Color = Colors.Turquoise;
                ((Label)sender).Background = colroBrush;
            }
        }

        private void MouseEventLetsLeave(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            ((Label)sender).Background = (Brush)bc.ConvertFrom("#00FFFFFF");
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

        private void SendMessageText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessageButton_Click(sender, e);
        }

        public void ReciveFile()
        {
            string[] bufferDownloadFile = ClassConteiner.Server.Tunnel.Function.ReciveLine().Split('/');
            // [0] - Название файла, [1] - размер файла, [2] - пользователь, кто отправил;
            int sizeFile = int.Parse(bufferDownloadFile[1]);
            DirectoryInfo directorySendUser = new DirectoryInfo("DownloadFile/" + bufferDownloadFile[2]);
            if (!directorySendUser.Exists)
                directorySendUser.Create();
            FileInfo fileRecive = new FileInfo(directorySendUser.FullName + '/' + bufferDownloadFile[0]);
            fileRecive.Delete();
            byte[] bufferFile = new byte[1024];
            FileStream fileStream = fileRecive.Create();
            int byteRead;
            ClassConteiner.Content.setLog("[Файл] Получены данные\r", LogText);
            while ((byteRead = ClassConteiner.Server.Tunnel.Info.stream.Read(bufferFile, 0, bufferFile.Length)) > 0)
            {
                fileStream.Write(bufferFile, 0, byteRead);
                if (fileStream.Length >= sizeFile)
                {
                    fileStream.Close();
                    ClassConteiner.Content.setLog("[Файл] Файл " + fileRecive.Name + " принят от пользователя "+ bufferDownloadFile[2] + "\r", LogText);
                    break;
                }
            }
        }

        public void SendFile(string pathFile, string userToSend)
        {
            TransferFile.pathFile = pathFile;
            TransferFile.userToSend = userToSend;
            WaitTransfer tempWindow = new WaitTransfer();
            tempWindow.ShowDialog();
        }

        private void SendFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserListOnline.SelectedIndex == -1)
            {
                MessageClass.Index_Error();
                return;
            }
            OpenFileDialog sendingFile = new OpenFileDialog();
            if (sendingFile.ShowDialog() != false)
                SendFile(sendingFile.FileName, UserListOnline.Items[UserListOnline.SelectedIndex].ToString());
        }
    }
}
