using System;
using System.IO;
using WhatIsThis;

namespace PandaChatServer.Bot
{
    public static class Addition
    {
        public static MainWindow window { get; set; }
        public static string[] Name = new string[0];
        public static string[] Message = new string[0];
        public static string[] Type = new string[0];
    }

    public class BotMessage
    {
        public string NameMessage { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
    }

    /// <summary>
    /// Класс для работы с сообщениями бота
    /// </summary>
    public static class SaveMessageBot
    {
        private static void IncrementBotMessage()
        {
            Array.Resize(ref Addition.Name, Addition.Name.Length + 1);
            Array.Resize(ref Addition.Message, Addition.Message.Length + 1);
            Array.Resize(ref Addition.Type, Addition.Type.Length + 1);
        }

        private static void DecrementBotMessage()
        {
            Array.Resize(ref Addition.Name, Addition.Name.Length - 1);
            Array.Resize(ref Addition.Message, Addition.Message.Length - 1);
            Array.Resize(ref Addition.Type, Addition.Type.Length - 1);
        }

        /// <summary>
        /// Добавление нового сообщения для бота
        /// </summary>
        public static void SetMessage(string NameMessage ,string MessageBot, string type)
        {
            IncrementBotMessage();
            Addition.Name[Addition.Name.Length - 1] = NameMessage;
            Addition.Message[Addition.Message.Length - 1] = MessageBot;
            Addition.Type[Addition.Type.Length - 1] = type;
        }

        /// <summary>
        /// Удаление сообщения для бота
        /// </summary>
        public static void DeleteMessage(string NameMessage)
        {
            int poz = Array.IndexOf(Addition.Name, NameMessage);
            for (int i = poz; i < Addition.Name.Length - 1; i++)
            {
                Addition.Name[i] = Addition.Name[i + 1];
                Addition.Message[i] = Addition.Message[i + 1];
                Addition.Type[i] = Addition.Type[i + 1];
            }
            DecrementBotMessage();
        }

        /// <summary>
        /// Сохранение всех сообщений в файле
        /// </summary>
        public static void SaveNow()
        {
            string[] bufferStringToFile = new string[Addition.Name.Length];
            for (int i = 0; i < Addition.Name.Length; i++)
                bufferStringToFile[i] = Addition.Name[i] + '/' + Addition.Message[i] + '/' + Addition.Type[i];
            File.WriteAllLines("Bot/word.txt", bufferStringToFile);
        }
    }

    /// <summary>
    /// Класс для отправки сообщений
    /// </summary>
    public static class MessageBot
    {
        /// <summary>
        /// Функция для отправки сообщения
        /// </summary>
        public static void SendRandomMessage()
        {
            Random rand = new Random();
            int pozForSend;
            do
            {
                pozForSend = rand.Next(0, Addition.Name.Length);
            } while (Addition.Type[pozForSend] != "Обычное сообщение");
            foreach (var user in ClientArray.clientUser)
            {
                if (user.infoUser.userTcpClient.Connected)
                {
                    user.functionTunnel.SendLine("BOTSENNDING");
                    user.functionTunnel.SendLine(Addition.Message[pozForSend]);
                }
            }
        }
        //TODO: Доделать анализатор сообщений для блокирования сообщений бота
        public static bool AnalyzeMessageForCensor(string FullMessage)
        {
            for (int i = 0; i < Addition.Message.Length; i++)
            {
                
            }
            return true;
        }
    }
}