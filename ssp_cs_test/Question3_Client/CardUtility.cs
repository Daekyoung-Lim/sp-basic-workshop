using System;
using System.Security.Cryptography;

class CardUtility
{
    public static string passwordEncryption_SHA256(string strInput)
    {
        byte[] hashValue;
        byte[] byteInput = System.Text.Encoding.UTF8.GetBytes(strInput);
        string strOutput = "";

        SHA256 mySHA256 = SHA256Managed.Create();
        hashValue = mySHA256.ComputeHash(byteInput);

        for (int i = 0; i < hashValue.Length; i++)
        {
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

    public static int WriteMessageInt32(ref byte[] output, int index, int input)
    {
        byte[] ByteValue = BitConverter.GetBytes(input);

        output[index + 0] = ByteValue[0];
        output[index + 1] = ByteValue[1];
        output[index + 2] = ByteValue[2];
        output[index + 3] = ByteValue[3];

        return sizeof(Int32);
    }

    public static byte[] INT2LE(int data)
    {
        byte[] b = new byte[4];
        b[0] = (byte)data;
        b[1] = (byte)(((uint)data >> 8) & 0xFF);
        b[2] = (byte)(((uint)data >> 16) & 0xFF);
        b[3] = (byte)(((uint)data >> 24) & 0xFF);
        return b;
    }

}
