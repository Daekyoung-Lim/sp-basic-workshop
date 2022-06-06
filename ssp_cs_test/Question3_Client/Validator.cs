using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Question3_Client
{
    class Validator
    {
        public string id {get; set;}

        public void checkCard(string id, string startTime, string busID, string cardInfo)
        {
            string strValidateCode;

            // cardInfo parsing 
            //string cardID = cardInfo.Substring(0, 8); 
            string cardBusID = cardInfo.Substring(8, 7);
            string code = cardInfo.Substring(15, 1);
            string rideTime = cardInfo.Substring(16);

            // Get Inspect Time
            string strInspectTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            // Validation
            // 1. Bus ID Match
            if (busID.Equals(cardBusID))
            {
                // 2. Check Aboard Code
                if (code.Equals("N"))
                {
                    // 3. Time Difference
                    if (CardUtility.HourDiff(strInspectTime, rideTime) < 3)
                    {
                        strValidateCode = "R1";
                    }
                    else
                    {
                        strValidateCode = "R4";
                    }
                }
                else
                {
                    strValidateCode = "R3";
                }
            }
            else
            {
                strValidateCode = "R2";
            }

            // Create Folder
            string destFolder = "..\\" + id;
            System.IO.Directory.CreateDirectory(destFolder);

            // File Writing
            string strFilename = destFolder + "\\" + id + "_" + startTime + ".TXT";
            string strWrite = id + "#" + busID + "#" + cardInfo + "#" + strValidateCode + "#" + strInspectTime + "\n";
            FileStream fs = new System.IO.FileStream(strFilename, FileMode.Append);
            fs.Write(Encoding.UTF8.GetBytes(strWrite), 0, strWrite.Length);
            fs.Close();
        }

        public void inspectCard(string id, string rideTime, string busInfo)
        {
            while (true)
            {
                string cardInfo = Console.ReadLine();

                if (cardInfo.Equals("DONE"))
                {
                    break;
                }
                else if (cardInfo.Length < 30)
                {
                    Console.WriteLine("Wrong Card Info");
                    continue;
                }

                checkCard(id, rideTime, busInfo, cardInfo);
            }
        }
    }
}
