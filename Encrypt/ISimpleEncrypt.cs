using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Encrypt
{
    public interface ISimpleEncrypt
    {

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        string Encrypt(string Text);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        string Decrypt(string Text);

        /// <summary>
        /// 加解密的密钥
        /// </summary>
        string EncryptKey { get; set; }

    }

    public static class DESEncrypt
    {

        public static readonly string DefaultKey = "YzkGroup";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text">要加密的文本</param>
        /// <param name="sKey">加密密码</param>
        /// <returns>返回密文</returns>
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(Text);
            dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(MD5(sKey).Substring(0, 8));
            dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(MD5(sKey).Substring(0, 8));
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array = memoryStream.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                byte b = array[i];
                stringBuilder.AppendFormat("{0:X2}", b);
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text">要加密的文本</param>
        /// <returns>返回密文</returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, DefaultKey);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text">要解密的密文</param>
        /// <param name="sKey">解密密码</param>
        /// <returns>返回明文</returns>
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            int num = Text.Length / 2;
            byte[] array = new byte[num];
            for (int i = 0; i < num; i++)
            {
                int num2 = Convert.ToInt32(Text.Substring(i * 2, 2), 16);
                array[i] = (byte)num2;
            }
            dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(MD5(sKey).Substring(0, 8));
            dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(MD5(sKey).Substring(0, 8));
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(array, 0, array.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.Default.GetString(memoryStream.ToArray());
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text">要解密的密文</param>
        /// <returns>返回明文</returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, DefaultKey);
        }

        private static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("X").PadLeft(2, '0');
            return ret;
        }
    }

    internal class DESSimpleEncrypt : ISimpleEncrypt
    {
        public string Encrypt(string Text)
        {
            return DESEncrypt.Encrypt(Text, string.IsNullOrEmpty(EncryptKey) ? DESEncrypt.DefaultKey : EncryptKey);
        }

        public string Decrypt(string Text)
        {
            return DESEncrypt.Decrypt(Text, string.IsNullOrEmpty(EncryptKey) ? DESEncrypt.DefaultKey : EncryptKey);
        }

        public string EncryptKey { get; set; }
    }
}
