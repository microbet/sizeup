using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace SizeUp.Core.Tiles
{
    public class GeoSink : IGeographySink110
    {

        private readonly GoogleMapsAPIProjection projection;
        private bool invalidShape;
        public GeoSink(GoogleMapsAPIProjection projection)
        {
            this.projection = projection;
            Geographies = new List<List<List<PointF>>>();
        }

        #region Implementation of IGeographySink

        protected List<PointF> CurrentFigure { get; set; }
        protected List<List<PointF>> CurrentGeography { get; set; }
        public List<List<List<PointF>>> Geographies { get; set; }

        public void SetSrid(int srid)
        {
            return;
        }

        public void BeginGeography(OpenGisGeographyType type)
        {
            switch (type)
            {
                case OpenGisGeographyType.Polygon:
                case OpenGisGeographyType.MultiPolygon:
                case OpenGisGeographyType.Point:
                    break;
                default:
                    invalidShape = true;
                    break;
            }
            CurrentGeography = new List<List<PointF>>();
        }

        private void addPoint(double latitude, double longitude, double? z, double? m)
        {
            if (!invalidShape)
                CurrentFigure.Add(projection.FromCoordinatesToPixel(new PointF((float)longitude, (float)latitude)));
        }
        public void BeginFigure(double latitude, double longitude, double? z, double? m)
        {
            CurrentFigure = new List<PointF>();
            addPoint(latitude, longitude, z, m);
        }

        public void AddLine(double latitude, double longitude, double? z, double? m)
        {
            addPoint(latitude, longitude, z, m);
        }

        public void EndFigure()
        {
            if (!invalidShape)
                CurrentGeography.Add(CurrentFigure);
            invalidShape = false;
        }

        public void EndGeography()
        {
            Geographies.Add(CurrentGeography);
        }

        #endregion




        public void AddCircularArc(double x1, double y1, double? z1, double? m1, double x2, double y2, double? z2, double? m2)
        {
            throw new NotImplementedException();
        }


    }
}
