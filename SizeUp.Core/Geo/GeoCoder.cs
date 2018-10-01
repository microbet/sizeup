using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.Web;
using SizeUp.Data;

namespace SizeUp.Core.Geo
{
    public class GeoCoder
    {
        class Geo
        {
            public string CountryCode { get; set; }
            public string Country { get; set; }
            public string RegionCode { get; set; }
            public string Region { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Lat { get; set; }
            public string Lng { get; set; }
            public string Ip { get; set; }
        }

        protected static string GEOCoderAddress { get { return @"http://api.ipinfodb.com/v2/ip_query.php?key=3f1885fa744dbf2517f3236bb50c7f791a68580cee86048388a9393ad36e28bf&ip={0}&timezone=false"; } }
        public static long? GetPlaceIdByIPAddress()
        {
            return GetPlaceIdByIPAddress(HttpContext.Current.Request.UserHostAddress);
        }
        /** Please note this probably fails, since its underlying Web Service endpoint no longer responds. We would need to upgrade to ipinfodb.com v3. */
        public static long? GetPlaceIdByIPAddress(string ip)
        {
            long? id = null;
            var Cache = HttpContext.Current.Cache;
            var cacheKey = string.Format("SizeUp.Core.Geo.GeoCoder.IP{0}", ip);
            
                if (!IsLocalIpAddress(ip))
                {
                    id = Cache[cacheKey] as long?;
                    Geo geo = null;
                    if (id == null)
                    {
                        try
                        {
                            var req = WebRequest.CreateHttp(String.Format(GEOCoderAddress, ip));
                            req.Timeout = 3;
                            XDocument xdoc = XDocument.Load(req.GetResponse().GetResponseStream()/*String.Format(GEOCoderAddress, ip)*/);
                            geo = (from x in xdoc.Descendants("Response")
                                       select new Geo()
                                       {
                                           CountryCode = (string)x.Element("CountryCode"),
                                           Country = (string)x.Element("CountryName"),
                                           RegionCode = (string)x.Element("RegionCode"),
                                           Region = (string)x.Element("RegionName"),
                                           City = (string)x.Element("City"),
                                           PostalCode = (string)x.Element("ZipPostalCode"),
                                           Lat = (string)x.Element("Latitude"),
                                           Lng = (string)x.Element("Longitude"),
                                           Ip = (string)x.Element("Ip")
                                       }).FirstOrDefault();
                        }
                        catch (Exception) {/*gobble gobble;*/}
                        if (geo != null)
                        {
                            using (var context = ContextFactory.SizeUpContext)
                            {
                                var lat = double.Parse(geo.Lat);
                                var lng = double.Parse(geo.Lng);
                                id = Core.DataLayer.Place.ListNear(context, new LatLng{ Lat = lat, Lng = lng})
                                    .Where(i=>i.Distance < 30000)
                                    .OrderBy(i=>i.Distance)
                                    .Select(i => i.Entity.Id).FirstOrDefault();
                                Cache[cacheKey] = id;
                            }
                        }
                    }
                }
   
            return id;
        }

        protected static bool IsLocalIpAddress(string host)
        {
            try
            { // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    // is local address
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch { }
            return false;
        }

    }
}
