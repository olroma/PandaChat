using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdaterChat
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.GetProcessesByName("PandaChatServer")[0].Kill();
            Console.WriteLine("Обновление программы:");
            File.Replace("PandaChatServerUPD.exe", "PandaChatServer.exe", "PandaChatServer.bak");
            Console.WriteLine("Обновление завершенно!\r Отчистка временных файлов...");
            File.Delete("ControlVersion.txt");
            Process.Start("PandaChatServer.exe");
        }
    }
}