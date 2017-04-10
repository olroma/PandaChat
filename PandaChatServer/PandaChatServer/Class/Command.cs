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
                "3) !sw <Сообщение> - отправляет сообщение от имени сервера;\r",
                "4) !info <ник> - информация о пользователе;\r",
                "4) !kick <ник> [причина] - кикнуть пользователя;\r",
                "4) !ban <ник> [причина] - забанить (не работает) пользователя;\r",
                "4) !cls - отчистить чат у сервера;\r",
                "4) !modclear - отчистить чат у клиента;\r",
                "5) !version - информация о версиях сервера и клиента;\r",
                "5) !updateversion <новая_версия_клиента> - установить новую версию для обновления клиента;\r"
            };
        }
    }
}
