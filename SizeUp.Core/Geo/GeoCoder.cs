﻿using System;
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
        public static City GetPlaceByIPAddress()
        {
            return GetPlaceByIPAddress(HttpContext.Current.Request.UserHostAddress);
        }
        public static City GetPlaceByIPAddress(string ip)
        {
            City p = null;
            var Cache = HttpContext.Current.Cache;
            var cacheKey = string.Format("SizeUp.Core.Geo.GeoCoder.IP{0}", ip);
            try
            {
                if (!IsLocalIpAddress(ip))
                {
                    p = Cache[cacheKey] as City;
                    if (p == null)
                    {
                        XDocument xdoc = XDocument.Load(String.Format(GEOCoderAddress, ip));
                        var geo = (from x in xdoc.Descendants("Response")
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

                        if (geo != null)
                        {
                            var point = System.Data.Spatial.DbGeography.FromText(string.Format("POINT ({0} {1})", geo.Lng, geo.Lat));
                            p = DataContexts.SizeUpContext.Cities.Where(i => i.Geography.Distance(point) < 30000 && i.Geography.Area > 0).FirstOrDefault();
                            Cache[cacheKey] = p;
                        }
                    }
                }
            }
            catch (Exception e){ }
            return p;
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
