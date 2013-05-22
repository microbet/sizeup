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
using System.Web;

namespace SizeUp.Core.API
{
    public class APIToken
    {
        private long _keyId;
        private long _timestamp;
        public long APIKeyId { get { return _keyId;} }
        public long TimeStamp { get { return _timestamp; } }

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

        public bool IsValid
        {
            get
            {
                var now = DateTime.UtcNow;
                var old = new DateTime(TimeStamp);

                var diff = now - old;
                var minutes = (int)diff.TotalMinutes;
                bool isValid = false;

                if (minutes < int.Parse(ConfigurationManager.AppSettings["Api.TokenExpiration"]))
                {
                    if (!HttpContext.Current.Request.IsLocal)
                    {
                        using (var context = ContextFactory.APIContext)
                        {
                            isValid = context.APIKeys.Any(i => i.Id == APIContext.Current.ApiToken.APIKeyId);
                        }
                    }
                    else
                    {
                        isValid = true;
                    }
                }
                return isValid;
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
            var cipher = Crypto.Crypto.Encrypt(s.ToArray(), CryptoPassword, CryptoSalt);
            return cipher;
        }

        public static APIToken ParseToken(string cipher)
        {
            APIToken returnToken = null;
            if (!string.IsNullOrEmpty(cipher))
            {
                var data = Crypto.Crypto.Decrypt(cipher, CryptoPassword, CryptoSalt);
                MemoryStream s = new MemoryStream();
                s.Write(data, 0, data.Length);
                s.Position = 0;
                BinaryFormatter bf = new BinaryFormatter();
                string[] outData = (bf.Deserialize(s) as string).Split("|".ToCharArray());
                returnToken = new APIToken(long.Parse(outData[0]));
                returnToken._timestamp = long.Parse(outData[1]);
            }
            return returnToken;
        }
    }
}
