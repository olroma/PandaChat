using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PandaChatClient
{
    /// <summary>
    /// Interaction logic for UpdateWin.xaml
    /// </summary>
    public partial class UpdateWin : Window
    {
        public UpdateWin()
        {
            InitializeComponent();
        }

        DispatcherTimer timer = new DispatcherTimer();
        private int countByte = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentVersionLabel.Content = Properties.Settings.Default.VersionClient;
            string DataFile = ClassConteiner.Server.Tunnel.Function.ReciveLine();
            long sizeFile = long.Parse(DataFile);
            byte[] bufferFile = new byte[1024];
            FileInfo fileRecive = new FileInfo("changelog.txt");
            FileStream fileStream = fileRecive.Create();
            int byteRead;
            while ((byteRead = ClassConteiner.Server.Tunnel.Info.stream.Read(bufferFile, 0, bufferFile.Length)) > 0)
            {
                fileStream.Write(bufferFile, 0, byteRead);
                if (fileStream.Length >= sizeFile)
                {
                    fileStream.Close();
                    break;
                }
            }
            ChangeUpdate.Text = File.ReadAllText(fileRecive.FullName);
            BackgroundWorker upd = new BackgroundWorker();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            upd.DoWork += Upd_DoWork;
            upd.RunWorkerCompleted += Upd_RunWorkerCompleted;
            upd.RunWorkerAsync();
            File.WriteAllBytes("updater.exe", Properties.Resources.UpdaterClientChat);
            ProgressDownload.Maximum = sizeFile / 1024.0f / 1024.0f;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                ProgressDownload.Value = countByte / 1024.0f / 1024.0f;
            }));
        }

        private void Upd_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label5.Content = "Успешно завершено!";
            ContinueUpdate.IsEnabled = true;
        }

        private void Upd_DoWork(object sender, DoWorkEventArgs e)
        {
            string DataFile = ClassConteiner.Server.Tunnel.Function.ReciveLine();
            long sizeFile = long.Parse(DataFile);
            byte[] bufferFile = new byte[1024];
            FileInfo fileRecive = new FileInfo("PandaChatClientUPD.exe");
            FileStream fileStream = fileRecive.Create();
            int byteRead;
            timer.Start();
            while ((byteRead = ClassConteiner.Server.Tunnel.Info.stream.Read(bufferFile, 0, bufferFile.Length)) > 0)
            {
                fileStream.Write(bufferFile, 0, byteRead);
                countByte += byteRead;
                if (fileStream.Length >= sizeFile)
                {
                    fileStream.Close();
                    break;
                }
            }
        }

        private void ContinueUpdate_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("updater.exe");
        }
    }
}
