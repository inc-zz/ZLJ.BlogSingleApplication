using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blogs.Core
{
    /// <summary>
    /// AES加密解密
    /// </summary>
    public static class AESCryptHelper
    {
        private static readonly string key = "D23E4940D413F960C3BFEAD35D632423";
        private static readonly string iv = "B8461DC679E30762";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] ivArray = Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            using Aes aes = Aes.Create();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.FeedbackSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyArray;
            aes.IV = ivArray;
            aes.Mode = CipherMode.CBC;

            using ICryptoTransform cTransform = aes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public static string Decrypt(string toDecrypt)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] ivArray = Encoding.UTF8.GetBytes(iv);
            byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);

            using Aes aes = Aes.Create();
            aes.Key = keyArray;
            aes.IV = ivArray;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.Zeros;

            using ICryptoTransform cTransform = aes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);

            return Encoding.UTF8.GetString(resultArray).TrimEnd('\0');
        }
    }
}