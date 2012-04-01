using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
namespace SizeUp.Core.Crypto
{
    public static class Crypto
    {
        public static string Encrypt(byte[] data, string Password, byte[] Salt)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, Salt);
            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }

        public static byte[] Decrypt(string cipherData, string Password, byte[] Salt)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, Salt);
            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            byte[] cipherBytes = Convert.FromBase64String(cipherData);
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.Close();
            return ms.ToArray();
        }
    }
}
