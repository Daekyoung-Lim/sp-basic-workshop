using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Question3_Client
{
    class ValidatorLauncher
    {
        Dictionary<string, string> validatorUsers = new Dictionary<string, string>();
        int validatorState = 0;
        Validator validator;
        string id = "";

        public ValidatorLauncher(string userInfoFile)
        {
            loadUserInfo(userInfoFile);
            validator = new Validator();
        }

        public void loadUserInfo(string userInfoFile)
        {
            List<string> listLine = File.ReadAllLines(userInfoFile).ToList<string>();

            foreach (string line in listLine)
            {
                string[] strs = line.Split(' ');

                if (!validatorUsers.ContainsKey(strs[0]))
                    validatorUsers.Add(strs[0], strs[1]);
            }
        }

        public bool checkUser(string id, string pw)
        {
            string encryptedPw = CardUtility.passwordEncryption_SHA256(pw);

            if (validatorUsers.ContainsKey(id) && (validatorUsers[id] == encryptedPw))
            {
                return true;
            }
            return false;
        }

        public string loginWaiting()
        {
            while (true)
            {
                string loginInfo = Console.ReadLine();
                string[] idPw = loginInfo.Split(' ');
                if (checkUser(idPw[0], idPw[1]))
                {
                    validatorState = 1;
                    Console.WriteLine("LOGIN SUCCESS");
                    return idPw[0];
                }
                else
                {
                    Console.WriteLine("Loging Fail");
                }
            }
        }

        public void rideBus(string id)
        {
            while (true)
            {
                string busInfo = Console.ReadLine();

                if (busInfo.Equals("LOGOUT"))
                {
                    validatorState = 2;
                    break;
                }
                else if (!busInfo.StartsWith("BUS_"))
                {
                    Console.WriteLine("Wrong Bus ID");
                    continue;
                }
                else
                {
                    string rideTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    validator.id = id;
                    validator.inspectCard(id, rideTime, busInfo);
                }
            }
        }

        public void logout(string loginId)
        {
            fileTranfer(loginId);
            fileMove(loginId);

            //wait login state
            validatorState = 0;
        }

        public void fileMove(string loginId)
        {
            try
            {
                string dirPath = "../" + loginId + "/";
                string destPath = "../BACKUP/";
                DirectoryInfo di = new DirectoryInfo(dirPath);
                FileInfo[] fiArr = di.GetFiles();

                foreach (FileInfo infoFile in fiArr)
                {
                    File.Move(dirPath + infoFile.Name, destPath + infoFile.Name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }

        public void fileTranfer(string loginId)
        {
            const string strIP = "127.0.0.1";
            const int BUF_SIZE = 4096;
            const int PORT = 27015;

            // Data buffer for incoming data.
            byte[] bytes = new byte[BUF_SIZE];

            // Establish the remote endpoint for the socket.
            IPAddress ipAddress = IPAddress.Parse(strIP);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, PORT);

            // Create a TCP/IP  socket.
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.
            try
            {
                sender.Connect(remoteEP);

                NetworkStream ns = new NetworkStream(sender);
                BinaryWriter bw = new BinaryWriter(ns);

                string dirPath = "../" + loginId;
                DirectoryInfo di = new DirectoryInfo(dirPath);
                FileInfo[] fiArr = di.GetFiles();
                foreach (FileInfo infoFile in fiArr)
                {
                    // 파일이름 전송
                    bw.Write(infoFile.Name);
                    long lSize = infoFile.Length;

                    // 파일크기 전송
                    bw.Write(lSize);

                    // 파일내용 전송
                    FileStream fs = new FileStream(infoFile.FullName, FileMode.Open);
                    while (lSize > 0)
                    {
                        int nReadLen = fs.Read(bytes, 0, Math.Min(BUF_SIZE, (int)lSize));
                        bw.Write(bytes, 0, nReadLen);
                        lSize -= nReadLen;
                    }
                    fs.Close();
                }

                bw.Close();
                ns.Close();

                // Release the socket.
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

                Console.WriteLine("Finished.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }

        public void run()
        {
            while (true)
            {

                switch (validatorState)
                {
                    case 0:     //login waiting
                        id = loginWaiting();
                        break;
                    case 1:     //loged in
                        rideBus(id);
                        break;
                    case 2:
                        logout(id);
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            ValidatorLauncher validatorLauncher = new ValidatorLauncher(@"..\CLIENT\INSPECTOR.txt");

            validatorLauncher.run();
        }
    }
}
