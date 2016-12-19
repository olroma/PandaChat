using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PandaChatClient.Class
{
    public static class MessageClass
    {
        public static void Empty_Field()
        {
            MessageBox.Show("Не все поля были введены!", "Пустые поля", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Connect_Error()
        {
            MessageBox.Show("Сервер не доступен! Попробуйте позже", "Ошибка подкючения", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Password_Server_Error()
        {
            MessageBox.Show("Введен неверный пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Same_User_Error()
        {
            MessageBox.Show("Данный пользователь уже есть на сервере!\nВход невозможен!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Ban_User(string ReasonBan)
        {
            MessageBox.Show("Вы были забанены на сервере!\rПричина: " + ReasonBan, "Бан", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Kick_User(string ReasonKick)
        {
            MessageBox.Show("Вы были кикнуты с сервера!\rПричина: " + ReasonKick, "Кик", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Index_Error()
        {
            MessageBox.Show("Не выбран пользователь для отправки файла!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Beta_Error()
        {
            MessageBox.Show("Данная функция не дуступна!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void RegWord_Error()
        {
            MessageBox.Show("Данное имя недоступно для входа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Choose_Error()
        {
            MessageBox.Show("Не выбран сервер!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Length_Auth_Error()
        {
            MessageBox.Show("Большой ник! Максимально допустимая длина: 11 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Validate_User_Error()
        {
            MessageBox.Show("Вы ввели не верные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Register_Error()
        {
            MessageBox.Show("Сейчас невозможно зарегистрироваться!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Register_OK()
        {
            MessageBox.Show("Вы успешно зарегистрировались!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
