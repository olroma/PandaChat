using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using PandaChatClient.Class;

namespace PandaChatClient
{
    /// <summary>
    /// Interaction logic for WaitTransfer.xaml
    /// </summary>
    public partial class WaitTransfer : Window
    {
        public WaitTransfer()
        {
            InitializeComponent();
            Inicialize();
        }
        
        DispatcherTimer timerCountByte = new DispatcherTimer();
        public static string path { get; set; }
        public int ByteCount { get; set; }
        public int ByteCountSpeed { get; set; }
        public float AllByteNeed { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BackgroundWorker Sending = new BackgroundWorker();
            StatusTransfer.Content = "Отправка файла";
            Sending.DoWork += Sending_DoWork;
            Sending.RunWorkerAsync();
            Sending.RunWorkerCompleted += Sending_RunWorkerCompleted;
            timerCountByte.Interval = new TimeSpan(0,0,1);
            timerCountByte.Tick += TimerCountByte_Tick;
        }

        private void TimerCountByte_Tick(object sender, EventArgs e)
        {
            ValProg(ByteCount);
        }

        private void Sending_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void Sending_DoWork(object sender, DoWorkEventArgs e)
        {
            FileInfo file = new FileInfo(TransferFile.pathFile);
            ClassConteiner.Server.Tunnel.Function.SendLine("SENDFILE");
            string toSendServer = file.Name + '/' + file.Length + '/' + TransferFile.userToSend;
            ClassConteiner.Server.Tunnel.Function.SendLine(toSendServer);
            MaxProg(file.Length);
            FileStream streamFIle = file.OpenRead();
            byte[] ReadFile = new byte[1024];
            int byteSend, countByte = 0;
            ByteCount = 0;
            ByteCountSpeed = 0;
            SetStatus("Подготовка файла для отправки...");
            Thread.Sleep(3500);
            SetStatus("Отправка");
            timerCountByte.Start();
            while (countByte < file.Length)
            {
                byteSend = streamFIle.Read(ReadFile, 0, ReadFile.Length);
                ClassConteiner.Server.Tunnel.Info.stream.Write(ReadFile, 0, byteSend);
                countByte += byteSend;
                ByteCount += byteSend;
                ByteCountSpeed += byteSend;
            }
            timerCountByte.Stop();
            ValProg(countByte);
            ClassConteiner.Server.Tunnel.Info.stream.Flush();
            ReadFile = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void MaxProg(long val)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                this.BarTransferS.Maximum = val / 1024.0f;
                this.AllByte.Content = "0/" + (val / 1024.0f).ToString() + " кбайт";
                AllByteNeed = val / 1024.0f;
            }));
        }

        private void Inicialize()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }));
        }

        private void ValProg(long val)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                this.BarTransferS.Value = val / 1024.0f;
                this.AllByte.Content = (val / 1024.0f).ToString() + '/' + AllByteNeed.ToString() + " кбайт";
                this.Speed.Content = (ByteCountSpeed / 1024 / 1024) + " мб/с";
                ByteCountSpeed = 0;
            }));
        }

        private void SetStatus(string status)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                this.Status.Content = status;
            }));
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
