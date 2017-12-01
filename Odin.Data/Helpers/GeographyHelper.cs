using System;
using System.Data.Entity.Spatial;

namespace Odin.Data.Helpers
{
    public static class GeographyHelper
    {
        public static DbGeography CreateCoordinate(string lat, string lon, int srid = 4326)
        {
            string wkt = String.Format("POINT({0} {1})", lon, lat);

            return DbGeography.PointFromText(wkt, srid);
        }
    }
}
