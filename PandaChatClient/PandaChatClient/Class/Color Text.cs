using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace PandaChatClient.Class
{
    public static class ColorText
    {
        private static Paragraph ParagraphColor { get; set; }

        public static void IniColorText(RichTextBox LogText, Paragraph paragraph)
        {
            LogText.Document = new FlowDocument(paragraph);
            ParagraphColor = paragraph;
        }

        public static void ColorLog(RichTextBox LogText, string ColorText, string Message, SolidColorBrush color)
        {
            LogText.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                ParagraphColor.Inlines.Add(new Bold(new Run(ColorText))
                {
                    Foreground = color
                });
                ParagraphColor.Inlines.Add(Message);
                ParagraphColor.Inlines.Add(new LineBreak());
                LogText.ScrollToEnd();
            }));
        }

        public static void ColorLog(RichTextBox LogText, string ColorText, SolidColorBrush color)
        {
            LogText.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                ParagraphColor.Inlines.Add(new Bold(new Run(ColorText))
                {
                    Foreground = color
                });
                ParagraphColor.Inlines.Add(new LineBreak());
                LogText.ScrollToEnd();
            }));
        }

        public static void LineLog(RichTextBox LogText, string textUnderlined, string normalText)
        {
            LogText.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                ParagraphColor.Inlines.Add(new Bold(new Underline(new Run(textUnderlined))));
                ParagraphColor.Inlines.Add(normalText);
                ParagraphColor.Inlines.Add(new LineBreak());
                LogText.ScrollToEnd();
            }));
        }

        public static void ColorModerator(ListBox listUser, string nameModerator)
        {
            for(int i = 0; i < listUser.Items.Count; i++)
            {
                if (listUser.Items[i].ToString() == nameModerator)
                {
                    ListBoxItem lbi = (ListBoxItem) listUser.ItemContainerGenerator.ContainerFromIndex(i);
                    listUser.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { lbi.Foreground = Brushes.Purple; }));
                }
            }
        }
    }
}
