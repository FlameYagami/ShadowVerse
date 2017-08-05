using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static System.Int32;

namespace Wrapper.Utils
{
    public class StringUtils
    {
        private const string PwdKey = "z619815x";
        private static readonly byte[] Keys = {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF};

        public static string Encrypt(string encryptString)
        {
            try
            {
                var rgbKey = Encoding.UTF8.GetBytes(PwdKey.Substring(0, 8));
                var rgbIv = Keys;
                var inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                var dCsp = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, dCsp.CreateEncryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        public static string Decrypt(string decryptString)
        {
            try
            {
                var rgbKey = Encoding.UTF8.GetBytes(PwdKey);
                var rgbIv = Keys;
                var inputByteArray = Convert.FromBase64String(decryptString);
                var dcsp = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, dcsp.CreateDecryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        /// <summary>
        ///     判断输入参数是否为数字
        /// </summary>
        /// <param name="str">参数</param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            try
            {
                var number = Parse(str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}