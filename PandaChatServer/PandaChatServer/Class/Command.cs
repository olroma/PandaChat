using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandaChatServer
{
    public static class CommandInfo
    {
        public static class HelpCommand
        {
            public static string[] infoCommand =
            {
                "1) !start - запустить сервер;\r",
                "2) !stop - остановить сервер;\r",
                "3) !SW <Сообщение> - отправляет сообщение от имени сервера;\r",
                "4) !info <ник> - информация о пользователе;\r",
                "5) !version - информация о версиях сервера и клиента;\r"
            };
        }
    }
}
