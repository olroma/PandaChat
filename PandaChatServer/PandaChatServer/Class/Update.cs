using System.IO;
using System.Threading;
using PandaChatServer.Properties;
using WhatIsThis;

namespace PandaChatServer.Class
{
    public static class InfoUpdate
    {
        public static DirectoryInfo UpdateDirectory = new DirectoryInfo("Update");
        public static DirectoryInfo UpdateClientDirectory =  new DirectoryInfo("Update/ChatClient");
        public static FileInfo UpdateClientFile = new FileInfo("Update/ChatClient/PandaChatClient.exe");
        public static FileInfo ChangeLogClient = new FileInfo("Update/ChatClient/changelog.txt");

        public static string VersionClient { get; set; }
    }


    public static class UpdateProcess
    {
        public static bool ValidateClient(Client UserCheckUpdate)
        {
            InfoUpdate.VersionClient = UserCheckUpdate.functionTunnel.ReciveLine();
            if (InfoUpdate.VersionClient != Settings.Default.VersionClient)
            {
                UserCheckUpdate.functionTunnel.SendLine("UPDATENEED");
                Thread.Sleep(1000);
                SendUpdate(UserCheckUpdate, InfoUpdate.ChangeLogClient);
                Thread.Sleep(2500);
                SendUpdate(UserCheckUpdate, InfoUpdate.UpdateClientFile);
                Thread.Sleep(1000);
                return true;
            }
            else
            {
                UserCheckUpdate.functionTunnel.SendLine("UPDATENORMAL");
                return false;
            }
        }

        private static void SendUpdate(Client user, FileInfo Update)
        {
            try
            {
                byte[] bufferFile = new byte[1024];
                string UpdateToClient = Update.Length.ToString();
                user.functionTunnel.SendLine(UpdateToClient);
                Thread.Sleep(2);
                int byteRead = 0, countByte = 0;
                var fileStream = Update.OpenRead();
                while (countByte < Update.Length)
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
