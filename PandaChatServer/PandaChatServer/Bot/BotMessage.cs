using System;
using System.IO;

namespace PandaChatServer.Bot
{
    public static class Addition
    {
        public static MainWindow window { get; set; }
        public static string[] Name = new string[0];
        public static string[] Message = new string[0];

        //public static string[] BlockWord;
    }

    public class BotMessage
    {
        public string NameMessage { get; set; }
        public string Message { get; set; }
    }

    public static class SaveMessageBot
    {
        private static void IncrementBotMessage()
        {
            Array.Resize(ref Addition.Name, Addition.Name.Length + 1);
            Array.Resize(ref Addition.Message, Addition.Message.Length + 1);
        }

        private static void DecrementBotMessage()
        {
            Array.Resize(ref Addition.Name, Addition.Name.Length - 1);
            Array.Resize(ref Addition.Message, Addition.Message.Length - 1);
        }

        public static void SetMessage(string NameMessage ,string MessageBot)
        {
            IncrementBotMessage();
            Addition.Name[Addition.Name.Length - 1] = NameMessage;
            Addition.Message[Addition.Message.Length - 1] = MessageBot;
        }

        public static void DeleteMessage(string NameMessage)
        {
            int poz = Array.IndexOf(Addition.Name, NameMessage);
            for (int i = poz; i < Addition.Name.Length - 1; i++)
            {
                Addition.Name[i] = Addition.Name[i + 1];
                Addition.Message[i] = Addition.Message[i + 1];
            }
            DecrementBotMessage();
        }

        public static void SaveNow()
        {
            string[] bufferStringToFile = new string[Addition.Name.Length];
            for (int i = 0; i < Addition.Name.Length; i++)
                bufferStringToFile[i] = Addition.Name[i] + '/' + Addition.Message[i];
            File.WriteAllLines("Bot/word.txt", bufferStringToFile);
        }
    }
}