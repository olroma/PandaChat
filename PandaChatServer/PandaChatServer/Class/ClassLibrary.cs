using System;
using System.Text;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Threading;
using PandaChatServer.Class;

namespace PandaChatServer
{
    public static class Server
    {
        public static class InfoServer
        {
            public static string IP = "127.0.0.1";
            public static int PORT = 6666;
            public static TcpListener server { get; set; }
            public static bool? isPassword { get; set; }
            public static string passwordServer { get; set; }
            public static bool? isMaintense { get; set; }
            public static string maintenseReason { get; set; }
            public static DataBase DataUserBase { get; set; }
        }
    }

    

    public static class BooleanQuastion
    {
        public static bool isTrueData { get; set; }
        public static bool isConnect { get; set; }
    }

    public static class MD5Class
    {
        private static string md5String { get; set; }
        private static string password { get; set; }

        public static string ConverPassword(string str)
        {
            password = str;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            string result = BitConverter.ToString(checkSum).Replace("-", string.Empty).ToLower();
            md5String = result;
            return result;
        }
    }

    public static class Function
    {
        public static void setLog (string message, RichTextBox LogText)
        {
            LogText.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { LogText.AppendText(message); LogText.ScrollToEnd();}));
        }

        public static void setAdminLog(string message, TextBox AdminText)
        {
            AdminText.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { AdminText.AppendText(message); }));
        }

        public static void setList(string nameUser, ListBox List)
        {
            List.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { List.Items.Add(nameUser); }));
        }

        public static void deleteList(string nameUser, ListBox List)
        {
            List.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { List.Items.Remove(nameUser); }));
        }
    }
}
