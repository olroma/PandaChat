using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using PandaChatServer;

namespace WhatIsThis
{
    public class Client
    {
        public UserInfo infoUser = new UserInfo();
        public TunnelInfo infoTunnel = new TunnelInfo();
        public TunnelFunction functionTunnel = new TunnelFunction();

        public void CloseClient()
        {
            infoUser.userTcpClient.Close();
        }

        public void GetInfoFormUser()
        {
            string[] bufferInfo = functionTunnel.ReciveLine().Split('/');
            if (bufferInfo[0] == "REGISTERUSER")
            {
                infoUser.userName = bufferInfo[1];
                infoUser.userPassword = bufferInfo[2];
                infoUser.userIP = IPAddress.Parse(bufferInfo[3]);
                infoUser.isRegin = true;
                return;
            }
            infoUser.userName = bufferInfo[0];
            infoUser.userPassword = bufferInfo[1];
            infoUser.userIP = IPAddress.Parse(bufferInfo[2]);
            infoUser.ComputerName = "";
        }

        public bool IsTruePassword()
        {
            string passwordFormUser = functionTunnel.ReciveLine();
            if (passwordFormUser == Server.InfoServer.passwordServer)
                return true;
            return false;
        }
    }

    public static class Close
    {
        public static void CloseConnection()
        {
            ClientArray.clientUser[ClientArray.clientUser.Length - 1].infoTunnel.CloseThread();
            ClientArray.clientUser[ClientArray.clientUser.Length - 1].CloseClient();
            Array.Resize(ref ClientArray.clientUser, ClientArray.clientUser.Length - 1);
        }
    }

    public class UserInfo 
    {
        public TcpClient userTcpClient { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public string ComputerName { get; set; }
        public IPAddress userIP { get; set; }
        public bool isAdmin { get; set; }
        public bool IsModerator { get; set; }
        public bool isRegin { get; set; }
        public string adminString { get; set; }
        public bool isBan { get; set; }
        public bool isKick { get; set; }
        public int numberUser { get; set; }
    }

    public class TunnelInfo
    {
        public NetworkStream stream { get; set; }
        public StreamWriter writer { get; set; }
        public StreamReader reader { get; set; }
        public int userIndex { get; set; }

        public void SetupThread() 
        {
            stream = ClientArray.clientUser[userIndex].infoUser.userTcpClient.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) {AutoFlush = true};
        }

        public void CloseThread()
        {
            reader.Close();
            writer.Close();
            stream.Close();
        }
    }

    public class TunnelFunction 
    {
        public int userIndex { get; set; }

        public void SendLine(string Message) 
        {

            ClientArray.clientUser[userIndex].infoTunnel.writer.WriteLine(Message);
        }

        public void SendNotLine(string Message)
        {
            ClientArray.clientUser[userIndex].infoTunnel.writer.Write(Message);
        }

        public string ReciveLine()
        {
            return ClientArray.clientUser[userIndex].infoTunnel.reader.ReadLine();
        }

        public char[] ReciveNotLine(int count)
        {
            char[] buffer = new char[count];
            ClientArray.clientUser[userIndex].infoTunnel.reader.Read(buffer, 0, count);
            return buffer;
        }
    }

    public static class ClientArray 
    {
        public static Client[] clientUser = new Client[0];

        /// <summary>
        /// Получение "клиента" по имени.
        /// Возвращает дескриптор пользователя или же null, если такого пользователя нет
        /// </summary>
        /// <param name="name"> - Имя пользователя для поиска</param>
        /// <returns></returns>
        public static Client GetClient(string name)
        {
            foreach (var userSearch in clientUser)
            {
                if (userSearch.infoUser.userName == name)
                    return userSearch;
            }
            return null;
        }
    }
}
