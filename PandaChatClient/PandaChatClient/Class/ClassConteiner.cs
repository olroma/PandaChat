using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using PandaChatClient.Class;

namespace PandaChatClient
{
    class ClassConteiner
    {
        public static class Server
        {
            public static IPAddress IP { get; set; }
            public static string CurrentUser { get; set; }
            public static int PORT { get; set; }
            public static TcpClient server { get; set; }
            public static Socket serverSocket { get; set; }

            public static class Tunnel
            {
                public static class Info
                {
                    public static NetworkStream stream { get; set; }
                    public static StreamWriter writer { get; set; }
                    public static StreamReader reader { get; set; }

                    public static void SetupServerThread()
                    {
                        stream = server.GetStream();
                        reader = new StreamReader(stream);
                        writer = new StreamWriter(stream) {AutoFlush = true};
                    }

                    public static void CloseThread()
                    {
                        writer.Close();
                        reader.Close();
                        stream.Close();
                    }
                }

                public static class Function
                {
                    public static void SendLine(string message)
                    {
                        Info.writer.WriteLine(message);
                    }

                    public static void SendNotLine(string message)
                    {
                        Info.writer.Write(message);
                    }

                    public static string ReciveLine()
                    {
                        return Info.reader.ReadLine();
                    }

                    public static char[] ReciveNotLine(int count)
                    {
                        char[] buffer = new char[count];
                        Info.reader.Read(buffer, 0, count);
                        return buffer;
                    }
                }
            }
        }

        public static class Content
        {
            public static bool isConnect { get; set;}

            public static void setLog(string message, RichTextBox LogText)
            {
                LogText.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { LogText.AppendText(message + '\r'); }));
            }

            public static void Cls(RichTextBox LogText)
            {
                LogText.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    LogText.Document.Blocks.Clear();
                    Paragraph newPh = new Paragraph();
                    ColorText.IniColorText(LogText, newPh);
                }));
            }

            public static void setListUser(string name, ListBox LogText)
            {
                LogText.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { LogText.Items.Add(name); }));
            }

            public static void delListUser(string name, ListBox LogText)
            {
                LogText.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { LogText.Items.Remove(name); }));
            }

            public static void scrollToEnd(TextBox LogText)
            {
                LogText.Dispatcher.Invoke(DispatcherPriority.Background, new Action(LogText.ScrollToEnd));
            }
        }

        public static class DownloadFileGoogle
        {
            public static bool StartDownload(string idFile, string path)
            {
                try
                {
                    WebClient fieClient = new WebClient();
                    fieClient.DownloadFile("https://drive.google.com/uc?export=download&id=" + idFile, path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return false;
                }
                return true;
            }
        }

        public static class UserInfo
        {
            public static string userLogin { get; set; }
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
    }
}
