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

    // yyyyMMddHHmmss형태의 문자열로 된 시간값 두 개를 입력받아서 시간 차이를 반환 
    // strTime2 - strTime1
    public static long HourDiff(string strTime2, string strTime1) 
    {
        DateTime dt1 = DateTime.ParseExact(strTime1, "yyyyMMddHHmmss", null);
        DateTime dt2 = DateTime.ParseExact(strTime2, "yyyyMMddHHmmss", null);

        TimeSpan ts = dt2 - dt1;

        return (long)ts.TotalHours;
	}
}
