using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PandaChatServer.Bot;
using PandaChatServer.Class;

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
            Addition.window.MessageBot.Items.Add(new BotMessage
            {
                NameMessage = NameOfMessage.Text,
                Message = MessageOfBot.Text
            });
            SaveMessageBot.SetMessage(NameOfMessage.Text, MessageOfBot.Text);
            this.Close();
        }
    }
}