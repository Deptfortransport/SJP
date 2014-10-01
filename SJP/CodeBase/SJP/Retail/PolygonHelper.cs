// *********************************************** 
// NAME             : PolygonHelper      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 12 Jan 2012
// DESCRIPTION  	: PolygonHelper class containing helper methods
// ************************************************
// 

using System.Drawing;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// PolygonHelper class containing helper methods
    /// </summary>
    public class PolygonHelper
    {
        /// <summary>
        /// Returns true if the Point is in the Polygon of Points
        /// </summary>
        /// <param name="p"></param>
        /// <param name="poly"></param>
        /// <returns></returns>
        public static bool PointInPolygon(Point p, Point[] poly)
        {
            // For details of following algorithm see: http://conceptual-misfire.awardspace.com/point_in_polygon.htm

            Point p1, p2;
            bool inside = false;

            if (poly.Length < 3)
            {
                return inside;
            }

            Point oldPoint = new Point(poly[poly.Length - 1].X, poly[poly.Length - 1].Y);

            for (int i = 0; i < poly.Length; i++)
            {
                Point newPoint = new Point(poly[i].X, poly[i].Y);

                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.X < p.X) == (p.X <= oldPoint.X) &&
                   ((long)p.Y - (long)p1.Y) * (long)(p2.X - p1.X) < ((long)p2.Y - (long)p1.Y) * (long)(p.X - p1.X))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }

            return inside;
        }
    }
}
