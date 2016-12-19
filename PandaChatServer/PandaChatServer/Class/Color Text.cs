using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace PandaChatServer.Class
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
                ParagraphColor.Inlines.Add(DateTime.Now.ToString() + '|');
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
                ParagraphColor.Inlines.Add(DateTime.Now.ToString() + '|');
                ParagraphColor.Inlines.Add(new Bold(new Run(ColorText))
                {
                    Foreground = color
                });
                ParagraphColor.Inlines.Add(new LineBreak());
                LogText.ScrollToEnd();
            }));
        }
    }
}
