using System;
using System.Collections.Generic;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace Morpho.Core
{
    /// <summary>
    /// Internal helper methods for geometry operations.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public static class GeometryHelpers
    {
        /// <summary>
        /// Creates a point at a given angle and distance from a center point.
        /// </summary>
        /// <param name="center">The center point.</param>
        /// <param name="angle">The angle in radians.</param>
        /// <param name="distance">The distance from center.</param>
        /// <returns>A new point at the specified position.</returns>
        public static Point PointAtAngle(Point center, double angle, double distance)
        {
            double x = center.X + distance * Math.Cos(angle);
            double y = center.Y + distance * Math.Sin(angle);
            return Point.ByCoordinates(x, y, center.Z);
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">Angle in degrees.</param>
        /// <returns>Angle in radians.</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>Angle in degrees.</returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        /// <summary>
        /// Creates an equilateral triangle centered at a point.
        /// </summary>
        /// <param name="center">Center of the triangle.</param>
        /// <param name="size">Distance from center to vertices.</param>
        /// <returns>List of three vertices.</returns>
        public static List<Point> EquilateralTriangle(Point center, double size)
        {
            var points = new List<Point>();
            for (int i = 0; i < 3; i++)
            {
                double angle = (2 * Math.PI * i / 3) - Math.PI / 2;
                points.Add(PointAtAngle(center, angle, size));
            }
            return points;
        }

        /// <summary>
        /// Creates a regular polygon centered at a point.
        /// </summary>
        /// <param name="center">Center of the polygon.</param>
        /// <param name="radius">Radius (circumscribed).</param>
        /// <param name="sides">Number of sides.</param>
        /// <param name="rotation">Rotation offset in radians.</param>
        /// <returns>List of vertices.</returns>
        public static List<Point> RegularPolygon(Point center, double radius, int sides, double rotation = 0)
        {
            var points = new List<Point>();
            for (int i = 0; i < sides; i++)
            {
                double angle = (2 * Math.PI * i / sides) + rotation - Math.PI / 2;
                points.Add(PointAtAngle(center, angle, radius));
            }
            return points;
        }

        /// <summary>
        /// Linearly interpolates between two points.
        /// </summary>
        /// <param name="p1">Start point.</param>
        /// <param name="p2">End point.</param>
        /// <param name="t">Interpolation factor (0-1).</param>
        /// <returns>Interpolated point.</returns>
        public static Point Lerp(Point p1, Point p2, double t)
        {
            return Point.ByCoordinates(
                p1.X + (p2.X - p1.X) * t,
                p1.Y + (p2.Y - p1.Y) * t,
                p1.Z + (p2.Z - p1.Z) * t
            );
        }

        /// <summary>
        /// Calculates the midpoint between two points.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <returns>Midpoint.</returns>
        public static Point Midpoint(Point p1, Point p2)
        {
            return Lerp(p1, p2, 0.5);
        }

        /// <summary>
        /// Calculates the centroid of a list of points.
        /// </summary>
        /// <param name="points">List of points.</param>
        /// <returns>Centroid point.</returns>
        public static Point Centroid(IList<Point> points)
        {
            if (points == null || points.Count == 0)
                throw new ArgumentException("Points list cannot be null or empty.");

            double sumX = 0, sumY = 0, sumZ = 0;
            foreach (var p in points)
            {
                sumX += p.X;
                sumY += p.Y;
                sumZ += p.Z;
            }

            return Point.ByCoordinates(
                sumX / points.Count,
                sumY / points.Count,
                sumZ / points.Count
            );
        }
    }
}

