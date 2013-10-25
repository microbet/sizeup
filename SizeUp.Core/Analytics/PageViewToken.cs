using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SizeUp.Core.Crypto;
using SizeUp.Data;


namespace SizeUp.Core.Analytics
{
    public class PageViewToken
    {
        public long? GeographicLocationId { get; set; }
        public long? IndustryId { get; set; }


        protected static string CryptoPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["Crypto.Password"];
            }
        }

        protected static byte[] CryptoSalt
        {
            get
            {
                return Encoding.Unicode.GetBytes(ConfigurationManager.AppSettings["Crypto.Salt"]);
            }
        }


        public string GetToken()
        {
            MemoryStream s = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            string data = string.Format("{0}|{1}|{2}", GeographicLocationId, IndustryId, DateTime.UtcNow.Ticks);
            bf.Serialize(s, data);
            var cipher = Crypto.Crypto.Encrypt(s.ToArray(), CryptoPassword, CryptoSalt);
            return cipher;
        }

        public static PageViewToken ParseToken(string cipher)
        {
            PageViewToken returnToken = null;
            if (!string.IsNullOrEmpty(cipher))
            {
                var data = Crypto.Crypto.Decrypt(cipher, CryptoPassword, CryptoSalt);
                MemoryStream s = new MemoryStream();
                s.Write(data, 0, data.Length);
                s.Position = 0;
                BinaryFormatter bf = new BinaryFormatter();
                string[] outData = (bf.Deserialize(s) as string).Split("|".ToCharArray());

                returnToken = new PageViewToken();
                returnToken.GeographicLocationId = outData[0].Length > 0 ? long.Parse(outData[0]) : (long?)null;
                returnToken.IndustryId = outData[1].Length > 0 ? long.Parse(outData[1]) : (long?)null;
            }
            return returnToken;
        }

    }
}
