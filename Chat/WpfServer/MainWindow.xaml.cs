using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using WpfServer.ChangeText;

namespace WpfServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ChangeTextWPFTextBox wpfTextBox;
        ServerObj server;
        

        public MainWindow()
        {
            InitializeComponent();

            Start.IsEnabled = true;
            Disconect.IsEnabled = false;
            SentBtn.IsEnabled = false;

            wpfTextBox = new ChangeTextWPFTextBox(ChatBox);
            server = new ServerObj(wpfTextBox);
        }
        public void StartHandler(object sender, RoutedEventArgs e)
        {
            SentBtn.IsEnabled = true;

            MassegeBox.Text = " ";

            Start.IsEnabled = false;
            Disconect.IsEnabled = true;

            Thread listenThread = new Thread(new ThreadStart(server.Listen));
            listenThread.Start();

        }
        public void DisconectHandler(object sender, RoutedEventArgs e)
        {
            SentBtn.IsEnabled = false;

            Start.IsEnabled = true;
            Disconect.IsEnabled = false;

            server.Disconnect();
        }
        public void SendHandler(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MassegeBox.Text))
            {
                server.SendMessage(MassegeBox.Text);
                MassegeBox.Text = null;
            }
        }
    }
}
