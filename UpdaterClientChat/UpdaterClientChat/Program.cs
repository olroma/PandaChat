using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdaterClientChat
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.GetProcessesByName("PandaChatClient")[0].Kill();
            Console.WriteLine("Обновление программы:");
            File.Replace("PandaChatClientUPD.exe", "PandaChatClient.exe", "PandaChatClient.bak");
            Console.WriteLine("Обновление завершенно!\r Отчистка временных файлов...");
            File.Delete("ControlVersion.txt");
            Process.Start("PandaChatClient.exe");
        }
    }
}
