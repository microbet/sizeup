using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SizeUp.Core.Crypto;


namespace SizeUp.Core.API
{
    public class APIToken
    {
        private long _keyId;
        private long _timestamp;
        public long APIKeyId { get { return _keyId;} }
        public long TimeStamp { get { return _timestamp; } }

        protected static string WidgetCryptoPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["Crypto.Password"];
            }
        }

        protected static byte[] WidgetCryptoSalt
        {
            get
            {
                return Encoding.Unicode.GetBytes(ConfigurationManager.AppSettings["Crypto.Salt"]);
            }
        }


        public APIToken(long KeyId)
        {
            _keyId = KeyId;
            _timestamp = DateTime.UtcNow.Ticks;
        }

        public string GetToken()
        {
            MemoryStream s = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            string data = string.Format("{0}|{1}", APIKeyId, TimeStamp);
            bf.Serialize(s, data);
            var cipher = Crypto.Crypto.Encrypt(s.ToArray(), WidgetCryptoPassword, WidgetCryptoSalt);
            return cipher;
        }

        public static APIToken GetToken(string cipher)
        {
            APIToken returnToken = null;
            if (!string.IsNullOrEmpty(cipher))
            {
                var data = Crypto.Crypto.Decrypt(cipher, WidgetCryptoPassword, WidgetCryptoSalt);
                MemoryStream s = new MemoryStream();
                s.Write(data, 0, data.Length);
                s.Position = 0;
                BinaryFormatter bf = new BinaryFormatter();
                string[] outData = (bf.Deserialize(s) as string).Split("|".ToCharArray());
                returnToken = Create(long.Parse(outData[0]));
                returnToken._timestamp = long.Parse(outData[1]);
            }
            return returnToken;
        }

        public static APIToken Create(long KeyId)
        {
            return new APIToken(KeyId);
        }
    }
}
