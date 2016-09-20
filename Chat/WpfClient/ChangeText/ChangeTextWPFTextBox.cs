using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityMessage.ChangeText;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfClient.ChangeText
{
    public class ChangeTextWPFTextBox : IChangeText 
    {
        TextBox textBox;
        public ChangeTextWPFTextBox(TextBox textBox)
        {
            this.textBox = textBox;
        }
        public void ChangeText(string text)
        {

            textBox.Dispatcher.Invoke(DispatcherPriority.Background, new
            Action(() =>
            {
                textBox.Text += text + "\n";
            }));
        }
    }
}
