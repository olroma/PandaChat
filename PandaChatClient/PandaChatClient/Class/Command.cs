using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandaChatClient.Class
{
    public static class Command
    {
        public static string[] HelpComman =
        {
            "1) !sendfile <название_файла> <пользователь> - отправка файла;",
            "2) !ls <пользователь> <сообщение> - отправка личного сообщения"
        };
    }
}
