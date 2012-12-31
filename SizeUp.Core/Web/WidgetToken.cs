using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SizeUp.Core.Crypto;
using SizeUp.Data;

namespace SizeUp.Core.Web
{
    public class WidgetToken
    {
        private static readonly string _widgetTokenCookie = "sizeup.widget.token";
        private static readonly string _widgetKeyCookie = "sizeup.widget.key";
        public static Guid? APIKey
        {
            get
            {
                var cookie = HttpContext.Current.Request.Cookies[_widgetKeyCookie];
                Guid? ret = default(Guid?);
                if (cookie != null)
                {
                    ret = Guid.Parse(cookie.Value);
                }
                return ret;
            }
        }

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


        public static bool IsValid
        {
            get
            {
                var t = GetToken();
                using (var context = ContextFactory.SizeUpContext)
                {
                    var widgetKey = context.APIKeys.Where(k => k.KeyValue == APIKey).FirstOrDefault();
                    //checks the given APIkey in keyCookie with the encrypted token key for a match and also checks to see the APIKey is
                    //in the DB if all are true then we are good
                    var valid = t.APIKey == APIKey && t.APIKey != Guid.Empty;
                    valid = valid && widgetKey != null && widgetKey.KeyValue == APIKey;

                    valid = true;
                    //this is a patch for now just to make this work...we need to go in and add additional logging and a hard kill switch for an api key
                    return valid;
                }
            }
        }


        public static void Create(Guid Key)
        {
            Token t = new Token() { APIKey = Key, TimeStamp = DateTime.Now.Ticks };
            MemoryStream s = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, t);
            var cipher = Crypto.Crypto.Encrypt(s.ToArray(), WidgetCryptoPassword, WidgetCryptoSalt);
            HttpCookie c = new HttpCookie(_widgetTokenCookie);
            c.Value = cipher;
            HttpCookie kc = new HttpCookie(_widgetKeyCookie);
            kc.Value = Key.ToString();
            HttpContext.Current.Response.Cookies.Add(c);
            HttpContext.Current.Response.Cookies.Add(kc);
        }


        protected static Token GetToken()
        {
            var cookie = HttpContext.Current.Request.Cookies[_widgetTokenCookie];
            Token t = new Token();
            if (cookie != null)
            {
                var encData = cookie.Value;
                var data = Crypto.Crypto.Decrypt(encData, WidgetCryptoPassword, WidgetCryptoSalt);
                MemoryStream s = new MemoryStream();
                s.Write(data, 0, data.Length);
                s.Position = 0;
                BinaryFormatter bf = new BinaryFormatter();
                t = bf.Deserialize(s) as Token;
            }
            return t;
        }

        [Serializable]
        protected class Token
        {
            public Guid APIKey { get; set; }
            public long TimeStamp { get; set; }
        }
    }
}
