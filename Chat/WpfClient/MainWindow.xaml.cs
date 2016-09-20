using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfClient.ChangeText;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ChangeTextWPFTextBox wpfTextBox;
        ClientObj client;
        public MainWindow()
        {
            InitializeComponent();

            Start.IsEnabled = true;
            Disconect.IsEnabled = false;
            SendBtn.IsEnabled = false;

            wpfTextBox = new ChangeTextWPFTextBox(ChatBox);
        }
        public void StartHandler(object sender, RoutedEventArgs e)
        {
            
            Start.IsEnabled = false;
            Disconect.IsEnabled = true;
            SendBtn.IsEnabled = true;
            MassegeBox.Text = " ";

            //
            NameUser.IsEnabled = false;
            ChatClient.Text = "Client: " + NameUser.Text;

            client = new ClientObj(wpfTextBox, NameUser.Text);
            client.Start();
        }
        public void DisconectHandler(object sender, RoutedEventArgs e)
        {
            //
            NameUser.IsEnabled = true;

            SendBtn.IsEnabled = false;
            Start.IsEnabled = true;
            Disconect.IsEnabled = false;

            client.Disconnect();
        }
        public void SendHandler(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(MassegeBox.Text))
            {
                client.SendMessage(MassegeBox.Text);
                MassegeBox.Text = null;
            }

        }
    }
}
