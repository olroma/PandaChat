using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using WhatIsThis;

namespace PandaChatServer.Class
{
    public static class SendFile
    {
        public static RichTextBox LogText { get; set; }
        public static TextBox AdminText { get; set; }
        public static int CountOfMassiveTransfer { get; set; }


        public static void SendFileUser(Client user)
        {
            string[] bufferDownloadFile = user.functionTunnel.ReciveLine().Split('/'); // [0] - Название файла, [1] - размер файла, [2] - пользователь, кому отправлен;
            Client userToSend = null;
            foreach (var name in ClientArray.clientUser)
            {
                if (name.infoUser.userName == bufferDownloadFile[2])
                {
                    userToSend = name;
                    break;
                }
            }
            if (userToSend == null)
                return;
            long sizeFile = long.Parse(bufferDownloadFile[1]);
            DirectoryInfo directorySendUser = new DirectoryInfo("SendFolder/" + user.infoUser.userName);
            if (!directorySendUser.Exists)
                directorySendUser.Create();
            FileInfo fileToSend = new FileInfo(directorySendUser.FullName + '/' + bufferDownloadFile[0]);
            // TODO: Запрос на перезапись файла
            byte[] bufferFile = new byte[1024];
            FileStream fileStream = fileToSend.Create();
            int byteRead, countByte = 0;
            try
            {
                Function.setLog(DateTime.Now.ToString() + "| [" + user.infoUser.userName + "-Сервер] Получены данные для принятия файла!\r", LogText);
                while (countByte < sizeFile)
                {
                    byteRead = user.infoTunnel.stream.Read(bufferFile, 0, bufferFile.Length);
                    fileStream.Write(bufferFile, 0, byteRead);
                    countByte += byteRead;
                }
                bufferFile = null;
                bufferFile = new byte[1024];
                fileStream.Close();
                Function.setLog(DateTime.Now.ToString() + "| [" + user.infoUser.userName + "-Сервер] Получен файл для оправки!\r", LogText);
                string re = bufferDownloadFile[0] + '/' + bufferDownloadFile[1] + '/' + user.infoUser.userName;
                userToSend.functionTunnel.SendLine("SENDFILEFROMSERVER");
                Thread.Sleep(10);
                userToSend.functionTunnel.SendLine(re);
                Thread.Sleep(10);
                byteRead = 0; countByte = 0;
                fileStream = fileToSend.OpenRead();
                while (countByte < sizeFile)
                {
                    byteRead = fileStream.Read(bufferFile, 0, bufferFile.Length);
                    userToSend.infoTunnel.stream.Write(bufferFile, 0, byteRead);
                    countByte += byteRead;
                }
                userToSend.infoTunnel.stream.Flush();
                Function.setLog(DateTime.Now.ToString() + "| [Сервер-" + userToSend.infoUser.userName + "] Файл отправлен!\r", LogText);
            }
            catch (Exception ex)
            {
                Function.setAdminLog(DateTime.Now.ToString() + "| Ошибка отправки файла!\r" + ex.ToString() + '\r', AdminText);
            }
            //WaitDelete(userToSend);
        }

        public static void MassiveSendFile()
        {
            var fileToSend = new OpenFileDialog();
            if (fileToSend.ShowDialog() == false)
                return;
            FileInfo file = new FileInfo(fileToSend.FileName);
            Function.setLog(DateTime.Now.ToString() + "| Массовая отправка файла: " + file.Name + '\r', LogText);
            CountOfMassiveTransfer = ClientArray.clientUser.Length;
            foreach (var u in ClientArray.clientUser)
                new Thread(delegate() { ThreadTransfer(u, file); }).Start();
            
        }

        public static void ThreadTransfer(Client user, FileInfo file)
        {
            try
            {
                byte[] bufferFile = new byte[1024];
                string re = file.Name + '/' + file.Length + "/СЕРВЕР";
                user.functionTunnel.SendLine("SENDFILEFROMSERVER");
                Thread.Sleep(10);
                user.functionTunnel.SendLine(re);
                Thread.Sleep(10);
                int byteRead = 0, countByte = 0;
                var fileStream = file.OpenRead();
                while (countByte < file.Length)
                {
                    byteRead = fileStream.Read(bufferFile, 0, bufferFile.Length);
                    user.infoTunnel.stream.Write(bufferFile, 0, byteRead);
                    countByte += byteRead;
                }
                user.infoTunnel.stream.Flush();
            }
            catch { }
        }
    }
}
