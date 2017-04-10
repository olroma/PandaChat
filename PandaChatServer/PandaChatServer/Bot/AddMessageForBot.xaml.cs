using System;
using System.Windows;
using PandaChatServer.Bot;

namespace PandaChatServer
{
    /// <summary>
    /// Interaction logic for AddMessageForBot.xaml
    /// </summary>
    public partial class AddMessageForBot : Window
    {
        public AddMessageForBot()
        {
            InitializeComponent();
        }

        private void SaveBotMessage_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NameOfMessage.Text) || String.IsNullOrEmpty(MessageOfBot.Text))
                return;
            string TypeMessageUser = BotMessageType.SelectedIndex == 0 ? "Обычное сообщение" : "Черный список";
            Addition.window.MessageBot.Items.Add(new BotMessage
            {
                NameMessage = NameOfMessage.Text,
                Message = MessageOfBot.Text,
                Type = TypeMessageUser
            });
            SaveMessageBot.SetMessage(NameOfMessage.Text, MessageOfBot.Text, TypeMessageUser);
            this.Close();
        }
    }
}