using System;
using System.Collections.Generic;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Morpho.Core;

namespace Morpho.Patterns
{
    /// <summary>
    /// Methods for creating star polygon patterns commonly found in Islamic geometric art.
    /// </summary>
    public static class StarPolygon
    {
        /// <summary>
        /// Creates an N-pointed star polygon.
        /// </summary>
        /// <param name="center">The center point of the star.</param>
        /// <param name="radius">The outer radius of the star.</param>
        /// <param name="points">The number of star points (typically 5, 6, 8, 10, or 12).</param>
        /// <param name="innerRatio">The ratio of inner radius to outer radius (0-1). Default is 0.4.</param>
        /// <returns name="curve">A closed PolyCurve representing the star polygon.</returns>
        /// <search>islamic,star,polygon,geometric,pattern,ornament</search>
        public static PolyCurve Create(
            Point center,
            double radius = 50,
            int points = 8,
            double innerRatio = 0.4)
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));
            if (radius <= 0)
                throw new ArgumentException("Radius must be positive.", nameof(radius));
            if (points < 3)
                throw new ArgumentException("Star must have at least 3 points.", nameof(points));
            if (innerRatio <= 0 || innerRatio >= 1)
                throw new ArgumentException("Inner ratio must be between 0 and 1.", nameof(innerRatio));

            double innerRadius = radius * innerRatio;
            var starPoints = new List<Point>();

            for (int i = 0; i < points; i++)
            {
                // Outer point
                double outerAngle = (2 * Math.PI * i / points) - Math.PI / 2;
                starPoints.Add(GeometryHelpers.PointAtAngle(center, outerAngle, radius));

                // Inner point (between outer points)
                double innerAngle = outerAngle + Math.PI / points;
                starPoints.Add(GeometryHelpers.PointAtAngle(center, innerAngle, innerRadius));
            }

            // Close the polygon
            starPoints.Add(starPoints[0]);

            return PolyCurve.ByPoints(starPoints, false);
        }

        /// <summary>
        /// Creates a grid of tessellated star polygons.
        /// </summary>
        /// <param name="width">Total width of the pattern grid.</param>
        /// <param name="height">Total height of the pattern grid.</param>
        /// <param name="cellSize">Size of each grid cell.</param>
        /// <param name="points">Number of points on each star.</param>
        /// <param name="innerRatio">Inner radius ratio (0-1).</param>
        /// <returns name="curves">List of star polygon curves.</returns>
        /// <search>islamic,star,grid,tessellation,pattern,tile</search>
        public static IList<PolyCurve> Grid(
            double width = 200,
            double height = 200,
            double cellSize = 50,
            int points = 8,
            double innerRatio = 0.4)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Width and height must be positive.");
            if (cellSize <= 0)
                throw new ArgumentException("Cell size must be positive.", nameof(cellSize));

            var stars = new List<PolyCurve>();
            int cols = (int)Math.Ceiling(width / cellSize);
            int rows = (int)Math.Ceiling(height / cellSize);
            double starRadius = cellSize * 0.45;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    double cx = col * cellSize + cellSize / 2;
                    double cy = row * cellSize + cellSize / 2;
                    var center = Point.ByCoordinates(cx, cy, 0);
                    
                    stars.Add(Create(center, starRadius, points, innerRatio));
                }
            }

            return stars;
        }

        /// <summary>
        /// Creates connecting lines between star polygon vertices for interlocking patterns.
        /// </summary>
        /// <param name="center">Center of the star.</param>
        /// <param name="radius">Outer radius.</param>
        /// <param name="points">Number of star points.</param>
        /// <returns name="lines">List of connecting lines.</returns>
        /// <search>islamic,star,interlocking,connection,lines</search>
        public static IList<Line> Connections(
            Point center,
            double radius = 50,
            int points = 8)
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));

            var lines = new List<Line>();
            var outerPoints = new List<Point>();

            for (int i = 0; i < points; i++)
            {
                double angle = (2 * Math.PI * i / points) - Math.PI / 2;
                outerPoints.Add(GeometryHelpers.PointAtAngle(center, angle, radius));
            }

            // Create lines connecting every other point (skip connection)
            int skip = points / 2;
            for (int i = 0; i < points; i++)
            {
                int nextIndex = (i + skip) % points;
                lines.Add(Line.ByStartPointEndPoint(outerPoints[i], outerPoints[nextIndex]));
            }

            return lines;
        }
    }

    /// <summary>
    /// Methods for creating rosette patterns found in Islamic art.
    /// </summary>
    public static class Rosette
    {
        /// <summary>
        /// Creates a rosette pattern with curved petals.
        /// </summary>
        /// <param name="center">The center point of the rosette.</param>
        /// <param name="radius">The outer radius.</param>
        /// <param name="petals">Number of petals.</param>
        /// <param name="petalDepth">Depth of the petal curve (0-1). Higher values create deeper curves.</param>
        /// <returns name="curves">List of arc curves forming the rosette.</returns>
        /// <search>islamic,rosette,flower,petal,geometric,ornament</search>
        public static IList<Curve> Create(
            Point center,
            double radius = 50,
            int petals = 8,
            double petalDepth = 0.3)
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));
            if (radius <= 0)
                throw new ArgumentException("Radius must be positive.", nameof(radius));
            if (petals < 3)
                throw new ArgumentException("Rosette must have at least 3 petals.", nameof(petals));
            if (petalDepth <= 0 || petalDepth >= 1)
                throw new ArgumentException("Petal depth must be between 0 and 1.", nameof(petalDepth));

            var curves = new List<Curve>();
            double innerRadius = radius * (1 - petalDepth);

            for (int i = 0; i < petals; i++)
            {
                double angle1 = 2 * Math.PI * i / petals;
                double angle2 = 2 * Math.PI * (i + 1) / petals;
                double midAngle = (angle1 + angle2) / 2;

                Point p1 = GeometryHelpers.PointAtAngle(center, angle1, radius);
                Point p2 = GeometryHelpers.PointAtAngle(center, angle2, radius);
                Point midPoint = GeometryHelpers.PointAtAngle(center, midAngle, innerRadius);

                try
                {
                    Arc arc = Arc.ByThreePoints(p1, midPoint, p2);
                    curves.Add(arc);
                }
                catch
                {
                    // Fallback to line if arc creation fails
                    curves.Add(Line.ByStartPointEndPoint(p1, p2));
                }
            }

            return curves;
        }

        /// <summary>
        /// Creates a multi-layer rosette with nested patterns.
        /// </summary>
        /// <param name="center">Center point.</param>
        /// <param name="outerRadius">Outer radius.</param>
        /// <param name="layers">Number of nested layers.</param>
        /// <param name="petals">Number of petals per layer.</param>
        /// <param name="petalDepth">Depth of petal curves.</param>
        /// <returns name="curves">All curves from all layers.</returns>
        /// <search>islamic,rosette,nested,layers,complex</search>
        public static IList<Curve> Nested(
            Point center,
            double outerRadius = 50,
            int layers = 3,
            int petals = 8,
            double petalDepth = 0.3)
        {
            if (layers < 1)
                throw new ArgumentException("Must have at least 1 layer.", nameof(layers));

            var allCurves = new List<Curve>();
            double radiusStep = outerRadius / layers;

            for (int layer = 0; layer < layers; layer++)
            {
                double layerRadius = outerRadius - (layer * radiusStep);
                // Alternate rotation for each layer
                double rotation = layer % 2 == 0 ? 0 : Math.PI / petals;

                var layerCurves = CreateRotated(center, layerRadius, petals, petalDepth, rotation);
                allCurves.AddRange(layerCurves);
            }

            return allCurves;
        }

        /// <summary>
        /// Creates a rosette with a specified rotation.
        /// </summary>
        private static IList<Curve> CreateRotated(
            Point center,
            double radius,
            int petals,
            double petalDepth,
            double rotation)
        {
            var curves = new List<Curve>();
            double innerRadius = radius * (1 - petalDepth);

            for (int i = 0; i < petals; i++)
            {
                double angle1 = 2 * Math.PI * i / petals + rotation;
                double angle2 = 2 * Math.PI * (i + 1) / petals + rotation;
                double midAngle = (angle1 + angle2) / 2;

                Point p1 = GeometryHelpers.PointAtAngle(center, angle1, radius);
                Point p2 = GeometryHelpers.PointAtAngle(center, angle2, radius);
                Point midPoint = GeometryHelpers.PointAtAngle(center, midAngle, innerRadius);

                try
                {
                    Arc arc = Arc.ByThreePoints(p1, midPoint, p2);
                    curves.Add(arc);
                }
                catch
                {
                    curves.Add(Line.ByStartPointEndPoint(p1, p2));
                }
            }

            return curves;
        }
    }

    /// <summary>
    /// Methods for creating Girih tiles - the interlocking geometric shapes used in Islamic architecture.
    /// </summary>
    public static class GirihTile
    {
        /// <summary>
        /// Creates a Girih tile of the specified type.
        /// </summary>
        /// <param name="center">Center point of the tile.</param>
        /// <param name="size">Size of the tile (edge length for most types).</param>
        /// <param name="tileType">Type of tile: "decagon", "pentagon", "hexagon", "bowtie", "rhombus".</param>
        /// <returns name="surface">The tile as a surface.</returns>
        /// <search>islamic,girih,tile,decagon,pentagon,hexagon,bowtie,rhombus</search>
        public static Surface Create(
            Point center,
            double size = 20,
            string tileType = "decagon")
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));
            if (size <= 0)
                throw new ArgumentException("Size must be positive.", nameof(size));

            var points = GetTilePoints(center, size, tileType.ToLowerInvariant());
            
            if (points.Count < 3)
                throw new ArgumentException($"Unknown tile type: {tileType}", nameof(tileType));

            // Close the polygon and create surface
            points.Add(points[0]);
            var polygon = PolyCurve.ByPoints(points, false);
            return Surface.ByPatch(polygon);
        }

        /// <summary>
        /// Creates the outline curves of a Girih tile.
        /// </summary>
        /// <param name="center">Center point of the tile.</param>
        /// <param name="size">Size of the tile.</param>
        /// <param name="tileType">Type of tile.</param>
        /// <returns name="curve">The tile outline as a PolyCurve.</returns>
        /// <search>islamic,girih,tile,outline,curve</search>
        public static PolyCurve Outline(
            Point center,
            double size = 20,
            string tileType = "decagon")
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));
            if (size <= 0)
                throw new ArgumentException("Size must be positive.", nameof(size));

            var points = GetTilePoints(center, size, tileType.ToLowerInvariant());
            
            if (points.Count < 3)
                throw new ArgumentException($"Unknown tile type: {tileType}", nameof(tileType));

            points.Add(points[0]);
            return PolyCurve.ByPoints(points, false);
        }

        /// <summary>
        /// Creates the internal strapwork lines of a Girih tile.
        /// </summary>
        /// <param name="center">Center point of the tile.</param>
        /// <param name="size">Size of the tile.</param>
        /// <param name="tileType">Type of tile.</param>
        /// <returns name="lines">List of internal decoration lines.</returns>
        /// <search>islamic,girih,strapwork,decoration,lines</search>
        public static IList<Line> Strapwork(
            Point center,
            double size = 20,
            string tileType = "decagon")
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));

            var lines = new List<Line>();
            var points = GetTilePoints(center, size, tileType.ToLowerInvariant());

            if (points.Count < 3)
                return lines;

            // Create lines from center to vertices
            foreach (var point in points)
            {
                lines.Add(Line.ByStartPointEndPoint(center, point));
            }

            // Create lines connecting midpoints of edges
            for (int i = 0; i < points.Count; i++)
            {
                int next = (i + 1) % points.Count;
                Point mid1 = GeometryHelpers.Midpoint(points[i], points[next]);
                
                int opposite = (i + points.Count / 2) % points.Count;
                int oppositeNext = (opposite + 1) % points.Count;
                Point mid2 = GeometryHelpers.Midpoint(points[opposite], points[oppositeNext]);
                
                lines.Add(Line.ByStartPointEndPoint(mid1, mid2));
            }

            return lines;
        }

        /// <summary>
        /// Gets the vertex points for a given tile type.
        /// </summary>
        private static List<Point> GetTilePoints(Point center, double size, string tileType)
        {
            var points = new List<Point>();

            switch (tileType)
            {
                case "decagon":
                    // Regular 10-sided polygon
                    for (int i = 0; i < 10; i++)
                    {
                        double angle = (2 * Math.PI * i / 10) - Math.PI / 2;
                        points.Add(GeometryHelpers.PointAtAngle(center, angle, size));
                    }
                    break;

                case "pentagon":
                    // Regular 5-sided polygon
                    for (int i = 0; i < 5; i++)
                    {
                        double angle = (2 * Math.PI * i / 5) - Math.PI / 2;
                        points.Add(GeometryHelpers.PointAtAngle(center, angle, size));
                    }
                    break;

                case "hexagon":
                    // Regular 6-sided polygon
                    for (int i = 0; i < 6; i++)
                    {
                        double angle = (2 * Math.PI * i / 6) - Math.PI / 2;
                        points.Add(GeometryHelpers.PointAtAngle(center, angle, size));
                    }
                    break;

                case "bowtie":
                    // Bow-tie (concave hexagon)
                    double bowAngle = Math.PI / 5;  // 36 degrees
                    points.Add(GeometryHelpers.PointAtAngle(center, -Math.PI / 2, size));
                    points.Add(GeometryHelpers.PointAtAngle(center, -Math.PI / 2 + bowAngle, size * 0.5));
                    points.Add(GeometryHelpers.PointAtAngle(center, -Math.PI / 2 + 2 * bowAngle, size));
                    points.Add(GeometryHelpers.PointAtAngle(center, Math.PI / 2, size));
                    points.Add(GeometryHelpers.PointAtAngle(center, Math.PI / 2 + bowAngle, size * 0.5));
                    points.Add(GeometryHelpers.PointAtAngle(center, Math.PI / 2 + 2 * bowAngle, size));
                    break;

                case "rhombus":
                    // Rhombus with 72° and 108° angles (golden rhombus)
                    double phi = (1 + Math.Sqrt(5)) / 2;  // Golden ratio
                    double rhombAngle = Math.Atan(1 / phi);
                    points.Add(Point.ByCoordinates(center.X, center.Y + size, center.Z));
                    points.Add(Point.ByCoordinates(center.X + size * Math.Sin(rhombAngle * 2), center.Y, center.Z));
                    points.Add(Point.ByCoordinates(center.X, center.Y - size, center.Z));
                    points.Add(Point.ByCoordinates(center.X - size * Math.Sin(rhombAngle * 2), center.Y, center.Z));
                    break;

                default:
                    // Return empty for unknown types
                    break;
            }

            return points;
        }
    }
}

