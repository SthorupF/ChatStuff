using System;
using System.Net.Sockets;
using System.Collections;
using System.Text;
using System.Threading;

namespace OpgaveSæt07_Chat_Server
{
    class Program
    {
        public static Hashtable clientList = new Hashtable();
        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(8888);
            TcpClient clientSocket = default(TcpClient);
            int numberOfUser;
            string dataFromClient;
            byte[] byteBuffer = new byte[10025];

            serverSocket.Start();
            Console.WriteLine("Servers is started");
            numberOfUser = 0;

            while (true)
            {
                Console.WriteLine("Waiting for connection");
                clientSocket = serverSocket.AcceptTcpClient();

                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(byteBuffer, 0, clientSocket.ReceiveBufferSize);
                dataFromClient = Encoding.ASCII.GetString(byteBuffer);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                clientList.Add(dataFromClient, clientSocket);
                Broadcast(dataFromClient + " Joined ", dataFromClient,  false);
                Console.WriteLine(dataFromClient + " joined chatroom");

                HandleClient client = new HandleClient(clientSocket, dataFromClient, clientList);
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine("Exit");
            Console.ReadLine();
        }

        public static void Broadcast(string message, string userName, bool flag)
        {
            foreach (DictionaryEntry client in clientList)
            {
                TcpClient broadcastSocket = (TcpClient)client.Value;
                NetworkStream breadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;

                if (flag == true)
                {
                    broadcastBytes = Encoding.ASCII.GetBytes(userName + ": " + message);
                }
                else
                {
                    broadcastBytes = Encoding.ASCII.GetBytes(message);
                }

                breadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                breadcastStream.Flush();
            }
        }
    }
}
