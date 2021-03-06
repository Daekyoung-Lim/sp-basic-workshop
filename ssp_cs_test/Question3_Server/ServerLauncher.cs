using System;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace Question3_Server
{
    class ServerLauncher
    {
        static string ReceiveFolder;

        static void Main(string[] args)
        {
            ReceiveFolder = "../SERVER";
            Directory.CreateDirectory(ReceiveFolder);

            CardSocketServer cardSocketServer = new CardSocketServer();
            Thread serverThread = new Thread(cardSocketServer.DoSocketWork);
            serverThread.Start();

            string strLine;
            while (true)
            {
                strLine = Console.ReadLine();

                if (strLine.Equals("QUIT"))
                {
                    cardSocketServer.closeServer();

                    break;
                }
            }

            serverThread.Join();
        }
    }
}
