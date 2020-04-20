using System;
using System.Text;
using System.Threading;
using System.Windows;
using System.Net.Sockets;



namespace OpgaveSæt07_Chat_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            UsernameTxt.IsEnabled = false;
            readData = "Conected to Chat server ...";
            Message();
            clientSocket.Connect("127.0.0.1", 8888);
            serverStream = clientSocket.GetStream();

            byte[] outstream = Encoding.ASCII.GetBytes(UsernameTxt.Text + "$");
            serverStream.Write(outstream, 0, outstream.Length);
            serverStream.Flush();

            Thread ctThread = new Thread(GetMessage);
            ctThread.Start();
        }

        private void GetMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                byte[] inStream = new byte[10025];
                int bufferSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, bufferSize);
                string returnData = Encoding.ASCII.GetString(inStream);
                readData = "" + readData;
                Message();
            }
        }

        private void Message()
        {
            ChatBox.Text = ChatBox.Text + Environment.NewLine + ">> " + readData;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            byte[] outStream = Encoding.ASCII.GetBytes(MessageTxt.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            MessageTxt.Text = "";
        }
    }
}
