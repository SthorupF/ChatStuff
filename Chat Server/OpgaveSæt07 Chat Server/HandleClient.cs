using System;
using System.Collections;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace OpgaveSæt07_Chat_Server
{
    class HandleClient
    {
        TcpClient clientSocket;
        string clNo;
        Hashtable clientList;

        public HandleClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            clientList = cList;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private void doChat()
        {
            int requestCount = 0;
            byte[] byteBuffer = new byte[10025];
            string dataFromCLient = null;
            byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;

            while (true)
            {
                try
                {
                    requestCount += 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(byteBuffer, 0, clientSocket.ReceiveBufferSize);
                    dataFromCLient = Encoding.ASCII.GetString(byteBuffer);
                    dataFromCLient = dataFromCLient.Substring(0, dataFromCLient.IndexOf("$"));
                    Console.WriteLine("From client - " + clNo + " : " + dataFromCLient);
                    rCount = Convert.ToString(requestCount);

                    Program.Broadcast(dataFromCLient, clNo, true);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
