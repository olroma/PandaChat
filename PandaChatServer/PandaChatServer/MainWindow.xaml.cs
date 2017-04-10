using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using PandaChatServer.Bot;
using PandaChatServer.Class;
using WhatIsThis;

namespace PandaChatServer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] usersName = new string[0];
        DispatcherTimer BotTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            AuthForm auth = new AuthForm();
            auth.ShowDialog();
        }

        private bool isNeedPh = true;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!BooleanQuastion.isTrueData)
                Environment.Exit(1);
            IPAddress IP = IPAddress.Parse(Server.InfoServer.IP);
            Server.InfoServer.server = new TcpListener(IP, Server.InfoServer.PORT);
            Server.InfoServer.server.Start();
            BooleanQuastion.isConnect = true;
            Thread listenThread = new Thread(Listen);
            listenThread.Start();
            if (isNeedPh)
            {
                Paragraph ph = new Paragraph();
                ColorText.IniColorText(LogText, ph);
                isNeedPh = false;
            }
            LogText.AppendText("Сервер запустился и готов принимать пользователей!\r");
            LogText.AppendText("Данные сервера: " + Server.InfoServer.IP + ':' + Server.InfoServer.PORT + '\r');
            Server.InfoServer.isMaintense = false;
            Server.InfoServer.isPassword = false;
            IPServerText.Text = Server.InfoServer.IP;
            PortServerText.Text = Server.InfoServer.PORT.ToString();
            BotTimer.Interval = new TimeSpan(0, 60, 60);
            BotTimer.Tick += Bot_Tick;
            Addition.window = this;
            try
            {
                string[] bufferWordBot = File.ReadAllLines("Bot/word.txt");
                foreach (string t in bufferWordBot)
                {
                    string[] buffer = t.Split('/');
                    MessageBot.Items.Add(new BotMessage { NameMessage = buffer[0], Message = buffer[1], Type = buffer[2] });
                    SaveMessageBot.SetMessage(buffer[0], buffer[1], buffer[2]);
                }
            }
            catch { return; }
            SendFile.LogText = LogText;
            SendFile.AdminText = AdminTextLog;
        }

        private void Bot_Tick(object sender, EventArgs e) => Bot.MessageBot.SendRandomMessage();

        private void Listen()
        {
            TcpClient client;
            while (BooleanQuastion.isConnect)
            {
                CONNECT: try
                {
                    client = Server.InfoServer.server.AcceptTcpClient();
                }
                catch { break; }
                Array.Resize(ref ClientArray.clientUser, ClientArray.clientUser.Length + 1);
                ClientArray.clientUser[ClientArray.clientUser.Length - 1] = new Client();
                ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userTcpClient = client;
                ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoTunnel.userIndex = ClientArray.clientUser.Length - 1;
                ClientArray.clientUser[ClientArray.clientUser.Length - 1].functionTunnel.userIndex = ClientArray.clientUser.Length - 1;
                ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoTunnel.SetupThread();
                if (UpdateProcess.ValidateClient(ClientArray.clientUser[ClientArray.clientUser.Length - 1]))
                {
                    WhatIsThis.Close.CloseConnection();
                    goto CONNECT;
                }
                if (Server.InfoServer.isPassword ?? false)
                {
                    ClientArray.clientUser[ClientArray.clientUser.Length - 1].functionTunnel.SendLine("PASSWORDSERVER");
                    if (!ClientArray.clientUser[ClientArray.clientUser.Length - 1].IsTruePassword())
                    {
                        ClientArray.clientUser[ClientArray.clientUser.Length - 1].functionTunnel.SendLine("NOTTRUEPASSWORD");
                        ColorText.ColorLog(LogText, " [Ошибка] ", "Пользователь ввел неверный пароль!", Brushes.OrangeRed);
                        ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoTunnel.CloseThread();
                        client.Close();
                        ClientArray.clientUser[ClientArray.clientUser.Length - 1] = null;
                        Array.Resize(ref ClientArray.clientUser, ClientArray.clientUser.Length - 1);
                        goto CONNECT;
                    }
                    ClientArray.clientUser[ClientArray.clientUser.Length - 1].functionTunnel.SendLine("TRUEPASSWORD");
                }
                else
                    ClientArray.clientUser[ClientArray.clientUser.Length - 1].functionTunnel.SendLine("NOTPASSWORD");
                ClientArray.clientUser[ClientArray.clientUser.Length - 1].GetInfoFormUser();
                Dictionary<string, string> validateUserData = Server.InfoServer.DataUserBase.ValidateUser(ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userName, ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userPassword);
                // TODO: Сделать проверку на бан
                if (validateUserData["ValidateLogin"] == "false" || validateUserData["ValidateData"] == "false")
                {
                    if (ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.isRegin)
                    {
                        string nickname = ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userName;
                        string password = ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userPassword;
                        string toDB = nickname + '/' + password + "/banFalse/adminFalse";
                        Server.InfoServer.DataUserBase.Add_User(toDB);
                        ClientArray.clientUser[ClientArray.clientUser.Length - 1].functionTunnel.SendLine("REGOK");
                        ColorText.ColorLog(LogText, " [Клиент] ", "Пользователь зарегестрирован: " + ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userIP, Brushes.Green);
                        WhatIsThis.Close.CloseConnection();
                        goto CONNECT;
                    }
                    ColorText.ColorLog(LogText, " [Ошибка] ", "Пользователь ввел неправильные данные: " + ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userIP, Brushes.OrangeRed);
                    ClientArray.clientUser[ClientArray.clientUser.Length - 1].functionTunnel.SendLine("NOTVALID");
                    WhatIsThis.Close.CloseConnection();
                    goto CONNECT;
                }
                else if (validateUserData["ValidateLogin"] == "true" && ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.isRegin)
                {
                    ClientArray.clientUser[ClientArray.clientUser.Length - 1].functionTunnel.SendLine("REGERROR");
                    ColorText.ColorLog(LogText, " [Ошибка] ", "Неудачная попытка зарегестрироваться: " + ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userIP, Brushes.OrangeRed);
                    WhatIsThis.Close.CloseConnection();
                    goto CONNECT;
                }
                else if (validateUserData["isAdmin"] == "true")
                    ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.isAdmin = true;
                Array.Resize(ref usersName, usersName.Length + 1);
                usersName[usersName.Length - 1] = ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userName;
                if (ClientArray.clientUser.Length > 1)
                {
                    for (int i = 0; i < ClientArray.clientUser.Length - 1; i++)
                    {
                        if (ClientArray.clientUser[i].infoUser.userName == ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userName && !ClientArray.clientUser[i].infoUser.userTcpClient.Connected)
                        {
                            int saveBufferClientIndex = ClientArray.clientUser[i].infoTunnel.userIndex;
                            ClientArray.clientUser[i] = ClientArray.clientUser[ClientArray.clientUser.Length - 1];
                            ClientArray.clientUser[i].infoUser.numberUser = saveBufferClientIndex;
                            ClientArray.clientUser[i].infoTunnel.userIndex = saveBufferClientIndex;
                            ClientArray.clientUser[i].functionTunnel.userIndex = saveBufferClientIndex;
                            ClientArray.clientUser[i].infoTunnel.SetupThread();
                            ClientArray.clientUser[i].functionTunnel.SendLine("OKCONNECTION");
                            usersName[usersName.Length - 1] = null;
                            Array.Resize(ref usersName, usersName.Length - 1);
                            usersName[i] = ClientArray.clientUser[i].infoUser.userName;
                            ClientArray.clientUser[ClientArray.clientUser.Length - 1] = null;
                            Array.Resize(ref ClientArray.clientUser, ClientArray.clientUser.Length - 1);
                            Thread sameUserThread = new Thread(new ParameterizedThreadStart(UserThread));
                            sameUserThread.Start(ClientArray.clientUser[i]);
                            string[] buferStringsSame = new string[usersName.Length];
                            int counterSame = 0;
                            for (int iSame = 0; iSame < usersName.Length; iSame++)
                            {
                                if (usersName[iSame] != null)
                                {
                                    buferStringsSame[counterSame] = usersName[iSame];
                                    counterSame++;
                                }
                            }
                            Array.Resize(ref buferStringsSame, buferStringsSame.Length - (usersName.Length - counterSame));
                            string bufferNamesSame = String.Join("/", buferStringsSame);
                            ClientArray.clientUser[i].functionTunnel.SendLine(bufferNamesSame);
                            SendOnlyCommand("ONLINENEW", ClientArray.clientUser[i]);
                            SendOnlyCommand(ClientArray.clientUser[i].infoUser.userName, ClientArray.clientUser[i]);
                            Function.setList(ClientArray.clientUser[i].infoUser.userName, ListUserBox);
                            goto CONNECT;
                        }
                        else if (ClientArray.clientUser[i].infoUser.userName == ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userName && ClientArray.clientUser[i].infoUser.userTcpClient.Connected)
                        {
                            usersName[usersName.Length - 1] = null;
                            Array.Resize(ref usersName, usersName.Length - 1);
                            Function.setAdminLog(DateTime.Now.ToString() + "| Пользователь " + ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userName + " был найден в онлайне!\r", AdminTextLog);
                            ClientArray.clientUser[ClientArray.clientUser.Length - 1].functionTunnel.SendLine("SAMEUSER");
                            WhatIsThis.Close.CloseConnection();
                            goto CONNECT;
                        }
                    }
                }
                ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.numberUser = ClientArray.clientUser.Length - 1;
                ClientArray.clientUser[ClientArray.clientUser.Length - 1].functionTunnel.SendLine("OKCONNECTION");
                ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userIP = ((IPEndPoint)ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userTcpClient.Client.RemoteEndPoint).Address;
                Function.setList(ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userName, ListUserBox);
                string[] buferStrings = new string[usersName.Length];
                int counter = 0;
                for (int i = 0; i < usersName.Length; i++)
                {
                    if (usersName[i] != null)
                    {
                        buferStrings[counter] = usersName[i];
                        counter++;
                    }
                }
                Array.Resize(ref buferStrings, buferStrings.Length - (usersName.Length - counter));
                string bufferNames = String.Join("/", buferStrings);
                ClientArray.clientUser[ClientArray.clientUser.Length - 1].functionTunnel.SendLine(bufferNames);
                SendOnlyCommand("ONLINENEW", ClientArray.clientUser[ClientArray.clientUser.Length - 1]);
                SendOnlyCommand(ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoUser.userName, ClientArray.clientUser[ClientArray.clientUser.Length - 1]);
                Thread newThreadUser = new Thread(new ParameterizedThreadStart(UserThread));
                newThreadUser.Start(ClientArray.clientUser[ClientArray.clientUser.Length - 1]);
            }
        }

        public void UserThread(object userClient)
        {
            Client user = (Client)userClient;
            ColorText.ColorLog(LogText, " [Клиент] " , " Пользователь подключен: " + user.infoUser.userName + ", IP: " + user.infoUser.userIP.ToString(), Brushes.Chartreuse);
            string buffer = "";
            while (user.infoUser.userTcpClient.Connected && BooleanQuastion.isConnect)
            {
                try
                {
                    buffer = user.functionTunnel.ReciveLine();
                }
                catch
                {
                    break;
                }
                switch (buffer)
                {
                    case "SANDMESSAGE":
                        {
                            string message = user.functionTunnel.ReciveLine();
                            Function.setLog(
                                DateTime.Now.ToString() + "| [" + user.infoUser.userName + "] Получено сообщение: " +
                                message + '\r', LogText);
                            SendMessageThread(message, user);
                            break;
                        }
                    case "PERSMESSAGE":
                        {
                            string message = user.functionTunnel.ReciveLine();
                            string nameToSend = message.Split('/')[0];
                            string messageToSend = message.Split('/')[1];
                            foreach (var userToSend in ClientArray.clientUser)
                            {
                                if (userToSend.infoUser.userName == nameToSend)
                                {
                                    userToSend.functionTunnel.SendLine("PERSONALMESSAGE");
                                    userToSend.functionTunnel.SendLine(user.infoUser.userName + '/' + messageToSend);
                                    break;
                                }
                            }
                            Function.setAdminLog(DateTime.Now.ToString() + "| [" + user.infoUser.userName + "] Получено личное сообщение для '" + nameToSend + "': " + message.Split('/')[1] + '\r', AdminTextLog);
                            break;
                        }
                    case "CLOSECONNECTION":
                        {
                            user.infoTunnel.CloseThread();
                            user.infoUser.userTcpClient.Close();
                            break;
                        }
                    case "CLEARMODER":
                        {
                            if (!user.infoUser.isAdmin)
                                user.functionTunnel.SendLine("PERMISSIONDENNIED");
                            else
                            {
                                SendFromServerCommand("CLEARSCREEN");
                                ColorText.ColorLog(LogText, " [Модератор] ", "Чат был отчищен модератором '" + user.infoUser.userName + "'", Brushes.Purple);
                            }
                            break;
                        }
                    case "KICKMODER":
                        {
                            if (!user.infoUser.isAdmin)
                                user.functionTunnel.SendLine("PERMISSIONDENNIED");
                            else
                            {
                                string nameKick = user.functionTunnel.ReciveLine();
                                foreach (var userKick in ClientArray.clientUser)
                                {
                                    if (userKick.infoUser.userName == nameKick)
                                    {
                                        userKick.functionTunnel.SendLine("KICKUSER");
                                        userKick.functionTunnel.SendLine("TEST");
                                        userKick.infoUser.isKick = true;
                                    }
                                }
                                ColorText.ColorLog(LogText, " [Модератор] ", "'" + nameKick + "' был кикнут с сервера", Brushes.Purple);
                            }
                            break;
                        }
                    case "SENDFILE":
                        {
                            SendFile.SendFileUser(user);
                            break;
                        }
                }
            }
            if (user.infoUser.userTcpClient.Connected)
            {
                user.infoTunnel.CloseThread();
                user.CloseClient();
            }
            Function.deleteList(user.infoUser.userName, ListUserBox);
            if (user.infoUser.isBan)
            {
                SendFromServer(" Пользователь " + user.infoUser.userName + " был забанен на сервере!");
                SendOnlyCommand("BAN", user);
            }
            else if (user.infoUser.isKick)
            {
                SendFromServer(" Пользователь " + user.infoUser.userName + " был кикнут с сервера!");
                SendOnlyCommand("KICK", user);
            }
            else
            {
                SendOnlyCommand("DELETEUSERONLINE", user);
                SendOnlyCommand(user.infoUser.userName, user);
            }
            usersName[user.infoTunnel.userIndex] = null;
            ColorText.ColorLog(LogText, " '" + user.infoUser.userName + "' ", "Закрыто соединение!", Brushes.Red);
        }

        public void SendOnlyCommand(string message, Client User)
        {
            for (int i = 0; i < ClientArray.clientUser.Length; i++)
            {
                if (User.infoUser.numberUser != i && ClientArray.clientUser[i].infoUser.userTcpClient.Connected)
                    ClientArray.clientUser[i].functionTunnel.SendLine(message);
            }
        }

        public void SendFromServerCommand(string message)
        {
            foreach (var user in ClientArray.clientUser)
            {
                if (user.infoUser.userTcpClient.Connected)
                    user.functionTunnel.SendLine(message);
            }
        }

        public void SendFromServer(string message)
        {
            foreach (var user in ClientArray.clientUser)
            {
                if (user.infoUser.userTcpClient.Connected)
                {
                    user.functionTunnel.SendLine("SERVERMESSAGESENDING");
                    user.functionTunnel.SendLine(message);
                }
            }
        }

        public void SendMessageThread(string message, Client User)
        {
            for (int i = 0; i < ClientArray.clientUser.Length; i++)
            {
                if (User.infoUser.numberUser != i && ClientArray.clientUser[i].infoUser.userTcpClient.Connected)
                    ClientArray.clientUser[i].functionTunnel.SendLine('<' + User.infoUser.userName + "> " + message);
            }
        }

        private void StartServerButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (BooleanQuastion.isConnect)
            {
                ColorText.ColorLog(LogText, " [Ошибка] ", "Сервер уже включен!", Brushes.OrangeRed);
                return;
            }
            BooleanQuastion.isConnect = false;
            Server.InfoServer.server.Stop();
            Window_Loaded(this, e);
        }

        private void StopServerButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (!BooleanQuastion.isConnect)
            {
                ColorText.ColorLog(LogText, " [Ошибка] ", "Сервер уже выключен!", Brushes.OrangeRed);
                return;
            }
            CloseServer(this, e);
            LogText.AppendText("Сервер остановлен!\r");
            ClientArray.clientUser = null;
            ClientArray.clientUser = new Client[0];
            GC.Collect();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(SendText.Text.Trim()))
                return;
            string[] bufferText = SendText.Text.Trim().Split(' ');
            int poz;
            if ((poz = SendText.Text.Trim().IndexOf("!sw")) != -1 || ChooseChatCommand.SelectedIndex == 1)
            {
                string message;
                if (ChooseChatCommand.SelectedIndex != 1)
                    message = SendText.Text.Trim().Substring(poz + 3);
                else
                    message = SendText.Text.Trim();
                SendFromServer(message);
                SendFromServerCommand("NORMAL");
                SendText.Clear();
                ColorText.ColorLog(LogText, " [Сервер] ", "'" + message + "'", Brushes.Purple);
                return;
            }
            else if ((poz = SendText.Text.Trim().IndexOf("!ban")) != -1 || (poz = SendText.Text.Trim().IndexOf("!kick")) != -1)
            {
                bool isFindUser = false;
                foreach (var userForBan in ClientArray.clientUser)
                {
                    if (bufferText.Length == 1)
                        break;
                    if (userForBan.infoUser.userName == bufferText[1])
                    {
                        isFindUser = true;
                        if (SendText.Text.Trim().IndexOf("!kick") != -1)
                        {
                            userForBan.functionTunnel.SendLine("KICKUSER");
                            userForBan.infoUser.isKick = true;
                        }
                        else
                        {
                            userForBan.functionTunnel.SendLine("BANUSER");
                            userForBan.infoUser.isBan = true;
                        }
                        userForBan.functionTunnel.SendLine(SendText.Text.Trim().Substring(SendText.Text.Trim().LastIndexOf(bufferText[1]) + userForBan.infoUser.userName.Length)); // причина
                        userForBan.infoTunnel.CloseThread();
                        userForBan.CloseClient();
                        if (SendText.Text.Trim().IndexOf("!kick") != -1)
                            ColorText.ColorLog(LogText, " [Администрирование] ", " Пользователь " + userForBan.infoUser.userName + " был кикнут. Причина: " + SendText.Text.Trim().Substring(SendText.Text.Trim().LastIndexOf(bufferText[1]) + userForBan.infoUser.userName.Length), Brushes.Purple);
                        else
                            ColorText.ColorLog(LogText, " [Администрирование] ", " Пользователь " + userForBan.infoUser.userName + " был заблокирован. Причина: " + SendText.Text.Trim().Substring(SendText.Text.Trim().LastIndexOf(bufferText[1]) + userForBan.infoUser.userName.Length), Brushes.Purple);
                        //TODO: Обновление базы данных (файла)
                        break;
                    }
                }
                SendText.Clear();
                if (!isFindUser)
                    ColorText.ColorLog(LogText, " [Ошибка] ", " Пользователь не найден или введена неправильная команда.\r Использование: !ban <имя_пользователя> [причина]\r !kick  <имя_пользователя> [причина]", Brushes.OrangeRed);
                return;
            }
            else if (bufferText[0] == "!info" || ChooseChatCommand.SelectedIndex == 2)
            {
                string nameUserFind;
                if (ChooseChatCommand.SelectedIndex == 2)
                    nameUserFind = SendText.Text.Trim();
                else if (bufferText.Length < 2)
                {
                    Function.setLog("[Информация] Неправильное использование команды! !info <ник_пользователя>\r", LogText);
                    return;
                }
                else
                    nameUserFind = bufferText[1];
                bool isFindUser = false;
                foreach (var userForGetInfo in ClientArray.clientUser)
                {
                    if (userForGetInfo.infoUser.userName == nameUserFind)
                    {
                        isFindUser = true;
                        Function.setLog("[Информация] Пользователь " + userForGetInfo.infoUser.userName + '\r' +
                            "IP: " + userForGetInfo.infoUser.userIP.ToString() + '\r' +
                            "HostName: (нет информации)" + '\r' + 
                            "Администратор: " + userForGetInfo.infoUser.isAdmin + '\r', LogText);
                    }
                }
                if (!isFindUser)
                    Function.setLog("[Информация] Пользователь не найден!\r", LogText);
                else
                    SendText.Clear();
                return;
            }
            else if (bufferText[0] == "!updateversion")
            {
                Properties.Settings.Default.VersionClient = bufferText[1];
                Properties.Settings.Default.Save();
                ColorText.ColorLog(LogText, " [Администрирование] ", " Новая версия" + Properties.Settings.Default.VersionClient + " клиента установлена", Brushes.Purple);
                SendText.Clear();
                return;
            }
            else if (bufferText[0] == "!ping")
            {
                if (bufferText.Length < 2)
                    return;
                Ping ping = new Ping();
                IPAddress ip = null;
                Client cl;
                
                if ((cl = ClientArray.GetClient(bufferText[1])) == null)
                {
                    ColorText.ColorLog(LogText, " [Ошибка] ", " Пользователь '" + bufferText[1] + "' не найден", Brushes.OrangeRed);
                    return;
                }
                ip = cl.infoUser.userIP;
                PingReply reply = ping.Send(ip);
                if (reply.Status == IPStatus.Success)
                    ColorText.ColorLog(LogText, " [Информация] ", " Пинг до пользователя '" + bufferText[1] + " - " + reply.RoundtripTime.ToString(), Brushes.Purple);
                SendText.Clear();
                return;
            }
            switch (SendText.Text)
            {
                case "!start":
                    {
                        MouseButtonEventArgs e1 = null;
                        StartServerButton_Click(this, e1);
                        break;
                    }
                case "!stop":
                    {
                        MouseButtonEventArgs e1 = null;
                        StopServerButton_Click(this, e1);
                        break;
                    }
                case "!cls":
                    {
                        LogText.Document.Blocks.Clear();
                        Paragraph newPh = new Paragraph();
                        ColorText.IniColorText(LogText, newPh);
                        break;
                    }
                case "!modclear":
                    {
                        SendFromServerCommand("CLEARSCREEN");
                        break;
                    }
                case "!help":
                    {
                        Function.setLog("[Команды] Команды сервера: \r", LogText);
                        foreach (string command in CommandInfo.HelpCommand.infoCommand)
                            Function.setLog(command, LogText);
                        break;
                    }
                case "!version":
                    {
                        LogText.AppendText("Версия сервера: " + Properties.Settings.Default.VersionServer + '\r');
                        LogText.AppendText("Версия клиента: " + Properties.Settings.Default.VersionClient + '\r');
                        break;
                    }
                default:
                    {
                        ColorText.ColorLog(LogText, " [Ошибка] ", " Команда '" + SendText.Text + "' не найдена", Brushes.OrangeRed);
                        break;
                    }
            }
            SendText.Clear();
        }

        private void SendText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendButton_Click(this, e);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Server.InfoServer.IP = IPServerText.Text;
                IPAddress.Parse(Server.InfoServer.IP);
                Server.InfoServer.PORT = int.Parse(PortServerText.Text);
                Server.InfoServer.isMaintense = isMaintense.IsChecked;
                Server.InfoServer.isPassword = isPasswordServer.IsChecked;
                Server.InfoServer.maintenseReason = MaintenseServerReason.Text;
                Server.InfoServer.passwordServer = PasswordServerText.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            Properties.Settings.Default.IP = Server.InfoServer.IP;
            Properties.Settings.Default.PORT = Server.InfoServer.PORT;
        }

        private void isChecked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).Name == "isPasswordServer")
            {
                if (((CheckBox)sender).IsChecked == true)
                    PasswordServerText.IsEnabled = true;
                else if (((CheckBox)sender).IsChecked == false)
                    PasswordServerText.IsEnabled = false;
            }
            else if (((CheckBox)sender).Name == "isMaintense")
            {
                if (((CheckBox)sender).IsChecked == true)
                    MaintenseServerReason.IsEnabled = true;
                else
                    MaintenseServerReason.IsEnabled = false;
            }
        }

        private void MouseEventLets(object sender, MouseEventArgs e)
        {
            SolidColorBrush colroBrush = new SolidColorBrush { Opacity = 0.5 };
            if (((Label)sender).Name == "ExitLabel")
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

        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception ex) { }
        }

        private void ExitLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(1);
        }

        private void MinimasedLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseServer(object sender, EventArgs e)
        {
            BooleanQuastion.isConnect = false;
            foreach (var userForCloseConnection in ClientArray.clientUser)
            {
                if (userForCloseConnection.infoUser.userTcpClient.Connected)
                {
                    userForCloseConnection.functionTunnel.SendLine("SERVERCLOSECONNECTION");
                    userForCloseConnection.infoTunnel.CloseThread();
                    userForCloseConnection.CloseClient();
                }
            }
            ClientArray.clientUser = null;
            usersName = null;
            usersName = new string[0];
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Server.InfoServer.server.Stop();
        }

        private void PlusMessage_Click(object sender, RoutedEventArgs e)
        {
            AddMessageForBot addBotMessage = new AddMessageForBot();
            addBotMessage.Owner = this;
            addBotMessage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addBotMessage.ShowDialog();
        }

        private void MinusMessage_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBot.SelectedIndex == -1)
                return;
            var sl = (dynamic)MessageBot.SelectedItems[0];
            string NameToDelete = sl.NameMessage;
            SaveMessageBot.DeleteMessage(NameToDelete);
            MessageBot.Items.Remove(MessageBot.SelectedItem);
        }

        private void SaveWordBot_Click(object sender, RoutedEventArgs e)
        {
            SaveMessageBot.SaveNow();
        }

        private void IsBot_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            if (box.IsChecked ?? false)
            {
                MinutesInterval.IsEnabled = true;
                SecondsInterval.IsEnabled = true;
                BotTimer.Start();
                LogText.AppendText("[БОТ] Бот запущен!\r");
            }
            else
            {
                MinutesInterval.IsEnabled = false;
                SecondsInterval.IsEnabled = false;
                BotTimer.Stop();
                LogText.AppendText("[БОТ] Бот остановлен!\r");
            }

        }

        private void SaveBotSetting_Click(object sender, RoutedEventArgs e)
        {
            BotTimer.Interval = new TimeSpan(0, int.Parse(MinutesInterval.Text), int.Parse(SecondsInterval.Text));
        }

        private void SendMassive_Click(object sender, MouseButtonEventArgs e)
        {
            SendFile.MassiveSendFile();
        }

        private void label6_MouseEnter(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            ((Label)sender).Foreground = (Brush)bc.ConvertFrom("#FF10FF00");
        }

        private void label6_MouseLeave(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            ((Label)sender).Foreground = (Brush)bc.ConvertFrom("#FF2F8929");
        }

        private void ClearChat(object sender, MouseButtonEventArgs e)
        {
            SendFromServerCommand("CLEARSCREEN");
            ColorText.ColorLog(LogText, " [Админстратор] ", "Чат был отчищен администратором ", Brushes.Purple);
        }

        private void label6_Copy4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ListUserBox.SelectedIndex == -1)
            {
                ColorText.ColorLog(LogText, " [Ошибка] ", " Чтобы сделать это действие, выберите пользователя", Brushes.OrangeRed);
                return;
            }

        }

        private bool isMenuPush = false;

        private void AnimationMainChat(object sender, MouseButtonEventArgs e)
        {
            //===================== Анимация главного окна чата ==================
            DoubleAnimation animHeightPicture = null, animWidthPicture = null;
            DoubleAnimation textBoxWidth = null, listUserOnlineWidth = null;
            ThicknessAnimation animPicture = null, animGridMainChat = null;
            ThicknessAnimation animListUser = null;
            ThicknessAnimation animButtonSend = null;
            DoubleAnimation animButtonWidth = null, animTextSend = null;
            //====================================================================

            //========== Анимация дополнительного меню =============
            ThicknessAnimation animAdvMenu = null;
            //=====================================================
            if (!isMenuPush)
            {
                AdMenu.Visibility = Visibility.Visible;
                animHeightPicture = new DoubleAnimation(28, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animWidthPicture = new DoubleAnimation(28, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animPicture = new ThicknessAnimation(new Thickness(222, 10, 0, 0), new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                textBoxWidth = new DoubleAnimation(638, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                listUserOnlineWidth = new DoubleAnimation(133, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animGridMainChat = new ThicknessAnimation(new Thickness(269, 5, 10, 27), new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animListUser = new ThicknessAnimation(new Thickness(646, 0, 0, 0), new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animButtonSend = new ThicknessAnimation(new Thickness(647, 493, 0, 0), new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animButtonWidth = new DoubleAnimation(131, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animTextSend = new DoubleAnimation(579, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animAdvMenu = new ThicknessAnimation(new Thickness(0, 0, -246, 0), new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animAdvMenu.Completed -= AnimAdvMenu_Completed;
            }
            else
            {
                animHeightPicture = new DoubleAnimation(21, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animWidthPicture = new DoubleAnimation(21, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animPicture = new ThicknessAnimation(new Thickness(8, 10, 0, 0), new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                textBoxWidth = new DoubleAnimation(817, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                listUserOnlineWidth = new DoubleAnimation(177, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animGridMainChat = new ThicknessAnimation(new Thickness(38, 5, 10, 27), new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animListUser = new ThicknessAnimation(new Thickness(829, 0, 0, 0), new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animButtonSend = new ThicknessAnimation(new Thickness(832, 493, 0, 0), new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animButtonWidth = new DoubleAnimation(177, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animTextSend = new DoubleAnimation(758, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animAdvMenu = new ThicknessAnimation(new Thickness(0, 0, 0, 0), new Duration(new TimeSpan(0, 0, 0, 0, 150)));
                animAdvMenu.Completed += AnimAdvMenu_Completed;
            }
            ChatMainGrid.BeginAnimation(FrameworkElement.MarginProperty, animGridMainChat);
            MenuPircture.BeginAnimation(FrameworkElement.HeightProperty, animHeightPicture);
            MenuPircture.BeginAnimation(FrameworkElement.WidthProperty, animWidthPicture);
            MenuPircture.BeginAnimation(FrameworkElement.MarginProperty, animPicture);
            LogText.BeginAnimation(FrameworkElement.WidthProperty, textBoxWidth);
            ListUserBox.BeginAnimation(FrameworkElement.WidthProperty, listUserOnlineWidth);
            ListUserBox.BeginAnimation(FrameworkElement.MarginProperty, animListUser);
            SendText.BeginAnimation(FrameworkElement.WidthProperty, animTextSend);
            SendButton.BeginAnimation(FrameworkElement.MarginProperty, animButtonSend);
            SendButton.BeginAnimation(FrameworkElement.WidthProperty, animButtonWidth);
            AdvancedMenuControlServer.BeginAnimation(FrameworkElement.MarginProperty, animAdvMenu);
            isMenuPush = !isMenuPush;
        }

        private void AnimAdvMenu_Completed(object sender, EventArgs e)
        {
            AdMenu.Visibility = Visibility.Hidden;
        }
    }
}