using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

class CardUtility
{
    public static string passwordEncryption_SHA256(string strInput)
    {
        byte[] hashValue;
        byte[] byteInput = System.Text.Encoding.UTF8.GetBytes(strInput);
        string strOutput = "";

        SHA256 mySHA256 = SHA256Managed.Create();
        hashValue = mySHA256.ComputeHash(byteInput);

        for (int i = 0; i < hashValue.Length; i++) {
            //Console.Write(String.Format("{0:X2}", hashValue[i]));
            strOutput += String.Format("{0:X2}", hashValue[i]);
        }

        return strOutput;
    }
}
