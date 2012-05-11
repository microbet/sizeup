using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data.Spatial;
using Microsoft.SqlServer.Types;

namespace SizeUp.Web.Areas.Api.Models.Maps
{
    public class Polygon
    {
        public List<List<LatLng>> Paths { get; set; }
        public static Polygon Create(SqlGeography geo, int reduction = 0)
        {
            geo = Clean(geo);
            Polygon g = new Polygon();           
            g.Paths = new List<List<LatLng>>();
            int numSubGeos = (int)geo.STNumGeometries();
            for (int sg = 1; sg <= numSubGeos; sg++)
            {
                SqlGeography thisGeo = geo.STGeometryN(sg);
                int numRings = (int)thisGeo.NumRings();
                List<LatLng> poly = new List<LatLng>();
                g.Paths.Add(poly);
                for (int r = 1; r <= numRings; r++)
                {
                    SqlGeography thisRing = thisGeo.RingN(r);
                    int numPoints = (int)thisRing.STNumPoints();
                    for (int p = 1; p <= numPoints; p++)
                    {
                        var thisPoint = thisRing.STPointN(p);
                        var geog = new LatLng() { Lng = (double)thisPoint.Long, Lat = (double)thisPoint.Lat };
                        poly.Add(geog);
                    }
                }
            }
            return g;
        }


        private static SqlGeography Clean(SqlGeography geo)
        {
            geo = SqlGeography.STGeomFromWKB(geo.MakeValid().STAsBinary(), (int)geo.STSrid);
            geo = SqlGeography.STGeomFromWKB(geo.STUnion(geo.STStartPoint()).STAsBinary(), (int)geo.STSrid);
            geo = SqlGeography.STGeomFromWKB(geo.STBuffer(0.00001).STBuffer(-0.00001).STAsBinary(), (int)geo.STSrid);
            geo = SqlGeography.STGeomFromWKB(geo.Reduce(0.00001).STAsBinary(), (int)geo.STSrid);

            return geo;

        }
       
    }
}