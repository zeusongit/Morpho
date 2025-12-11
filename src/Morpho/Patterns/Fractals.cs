using System;
using System.Collections.Generic;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Morpho.Core;

namespace Morpho.Patterns
{
    /// <summary>
    /// Methods for creating Koch curve fractals.
    /// </summary>
    public static class Koch
    {
        /// <summary>
        /// Creates a Koch snowflake fractal.
        /// </summary>
        /// <param name="center">The center point of the snowflake.</param>
        /// <param name="radius">The circumscribed radius.</param>
        /// <param name="iterations">Number of fractal iterations (0-6 recommended).</param>
        /// <returns name="curves">A list of line curves representing the Koch snowflake.</returns>
        /// <search>koch,snowflake,fractal,curve,self-similar</search>
        public static IList<Line> Snowflake(
            Point center,
            double radius = 100,
            int iterations = 4)
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));
            if (radius <= 0)
                throw new ArgumentException("Radius must be positive.", nameof(radius));
            if (iterations < 0)
                throw new ArgumentException("Iterations must be non-negative.", nameof(iterations));
            if (iterations > 8)
                throw new ArgumentException("Iterations above 8 may cause performance issues.", nameof(iterations));

            // Create initial equilateral triangle
            var triangle = GeometryHelpers.EquilateralTriangle(center, radius);
            
            // Generate Koch curve points for each side
            var allPoints = new List<Point>();
            
            for (int i = 0; i < 3; i++)
            {
                int next = (i + 1) % 3;
                var segmentPoints = GenerateKochPoints(triangle[i], triangle[next], iterations);
                
                // Add all points except the last one (to avoid duplicates)
                for (int j = 0; j < segmentPoints.Count - 1; j++)
                {
                    allPoints.Add(segmentPoints[j]);
                }
            }

            // Create line segments
            var lines = new List<Line>();
            for (int i = 0; i < allPoints.Count; i++)
            {
                int next = (i + 1) % allPoints.Count;
                lines.Add(Line.ByStartPointEndPoint(allPoints[i], allPoints[next]));
            }
            
            return lines;
        }

        /// <summary>
        /// Creates a single Koch curve between two points.
        /// </summary>
        /// <param name="startPoint">The start point of the curve.</param>
        /// <param name="endPoint">The end point of the curve.</param>
        /// <param name="iterations">Number of fractal iterations.</param>
        /// <returns name="curve">A PolyCurve representing the Koch curve.</returns>
        /// <search>koch,curve,fractal,line,self-similar</search>
        public static PolyCurve Curve(
            Point startPoint,
            Point endPoint,
            int iterations = 4)
        {
            if (startPoint == null)
                throw new ArgumentNullException(nameof(startPoint));
            if (endPoint == null)
                throw new ArgumentNullException(nameof(endPoint));
            if (iterations < 0)
                throw new ArgumentException("Iterations must be non-negative.", nameof(iterations));

            var points = GenerateKochPoints(startPoint, endPoint, iterations);
            
            if (points.Count < 2)
                throw new InvalidOperationException("Not enough points to create a curve.");
            
            // Create line segments
            var lines = new List<Curve>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                lines.Add(Line.ByStartPointEndPoint(points[i], points[i + 1]));
            }
            
            return PolyCurve.ByJoinedCurves(lines.ToArray(), 0.001, false, 0.0);
        }

        /// <summary>
        /// Creates a Koch anti-snowflake (points inward).
        /// </summary>
        /// <param name="center">The center point.</param>
        /// <param name="radius">The circumscribed radius.</param>
        /// <param name="iterations">Number of iterations.</param>
        /// <returns name="curve">A PolyCurve representing the anti-snowflake.</returns>
        /// <search>koch,antisnowflake,fractal,inward</search>
        public static PolyCurve AntiSnowflake(
            Point center,
            double radius = 100,
            int iterations = 4)
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));
            if (radius <= 0)
                throw new ArgumentException("Radius must be positive.", nameof(radius));

            // Create initial equilateral triangle (pointing inward)
            var triangle = GeometryHelpers.EquilateralTriangle(center, radius);
            
            var allPoints = new List<Point>();
            
            for (int i = 0; i < 3; i++)
            {
                int next = (i + 1) % 3;
                // Reverse the points to make the bumps go inward
                var segmentPoints = GenerateKochPoints(triangle[next], triangle[i], iterations);
                segmentPoints.Reverse();
                
                for (int j = 0; j < segmentPoints.Count - 1; j++)
                {
                    allPoints.Add(segmentPoints[j]);
                }
            }

            if (allPoints.Count < 2)
                throw new InvalidOperationException("Not enough points to create a curve.");

            // Create line segments and join them
            var lines = new List<Curve>();
            for (int i = 0; i < allPoints.Count; i++)
            {
                int next = (i + 1) % allPoints.Count;
                lines.Add(Line.ByStartPointEndPoint(allPoints[i], allPoints[next]));
            }
            
            return PolyCurve.ByJoinedCurves(lines.ToArray(), 0.001, false, 0.0);
        }

        /// <summary>
        /// Generates Koch curve points between two endpoints.
        /// </summary>
        private static List<Point> GenerateKochPoints(Point p1, Point p2, int iterations)
        {
            var points = new List<Point> { p1, p2 };

            for (int i = 0; i < iterations; i++)
            {
                var newPoints = new List<Point>();

                for (int j = 0; j < points.Count - 1; j++)
                {
                    Point a = points[j];
                    Point e = points[j + 1];

                    // Calculate the four new points
                    Point b = GeometryHelpers.Lerp(a, e, 1.0 / 3.0);
                    Point d = GeometryHelpers.Lerp(a, e, 2.0 / 3.0);

                    // Calculate the peak point (c)
                    double dx = e.X - a.X;
                    double dy = e.Y - a.Y;
                    double length = Math.Sqrt(dx * dx + dy * dy) / 3.0;
                    double angle = Math.Atan2(dy, dx) + Math.PI / 3.0;

                    Point c = Point.ByCoordinates(
                        b.X + length * Math.Cos(angle),
                        b.Y + length * Math.Sin(angle),
                        a.Z
                    );

                    newPoints.Add(a);
                    newPoints.Add(b);
                    newPoints.Add(c);
                    newPoints.Add(d);
                }
                
                newPoints.Add(points[points.Count - 1]);
                points = newPoints;
            }

            return points;
        }
    }

    /// <summary>
    /// Methods for creating Sierpinski fractals.
    /// </summary>
    public static class Sierpinski
    {
        /// <summary>
        /// Creates a 2D Sierpinski triangle (gasket).
        /// </summary>
        /// <param name="center">The center point of the triangle.</param>
        /// <param name="size">The size (height) of the triangle.</param>
        /// <param name="iterations">Number of subdivision iterations (0-8 recommended).</param>
        /// <returns name="surfaces">List of triangle surfaces forming the gasket.</returns>
        /// <search>sierpinski,triangle,gasket,fractal,self-similar</search>
        public static IList<Surface> Triangle(
            Point center,
            double size = 100,
            int iterations = 5)
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));
            if (size <= 0)
                throw new ArgumentException("Size must be positive.", nameof(size));
            if (iterations < 0)
                throw new ArgumentException("Iterations must be non-negative.", nameof(iterations));
            if (iterations > 10)
                throw new ArgumentException("Iterations above 10 may cause performance issues.", nameof(iterations));

            // Create the initial triangle vertices
            var vertices = GeometryHelpers.EquilateralTriangle(center, size);

            var triangles = new List<List<Point>> { vertices };

            // Recursive subdivision
            for (int i = 0; i < iterations; i++)
            {
                var newTriangles = new List<List<Point>>();

                foreach (var tri in triangles)
                {
                    // Get midpoints
                    Point mid01 = GeometryHelpers.Midpoint(tri[0], tri[1]);
                    Point mid12 = GeometryHelpers.Midpoint(tri[1], tri[2]);
                    Point mid20 = GeometryHelpers.Midpoint(tri[2], tri[0]);

                    // Create three new triangles (corners only, not center)
                    newTriangles.Add(new List<Point> { tri[0], mid01, mid20 });
                    newTriangles.Add(new List<Point> { mid01, tri[1], mid12 });
                    newTriangles.Add(new List<Point> { mid20, mid12, tri[2] });
                }

                triangles = newTriangles;
            }

            // Convert to surfaces
            var surfaces = new List<Surface>();
            foreach (var tri in triangles)
            {
                try
                {
                    var pts = new List<Point>(tri) { tri[0] };
                    var polyCurve = PolyCurve.ByPoints(pts, false);
                    surfaces.Add(Surface.ByPatch(polyCurve));
                }
                catch
                {
                    // Skip degenerate triangles
                }
            }

            return surfaces;
        }

        /// <summary>
        /// Creates a 3D Sierpinski tetrahedron (pyramid).
        /// </summary>
        /// <param name="center">The center point of the base.</param>
        /// <param name="size">The edge length of the tetrahedron.</param>
        /// <param name="iterations">Number of subdivision iterations (0-5 recommended).</param>
        /// <returns name="solids">List of tetrahedron solids.</returns>
        /// <search>sierpinski,tetrahedron,pyramid,3d,fractal</search>
        public static IList<Solid> Tetrahedron(
            Point center,
            double size = 100,
            int iterations = 4)
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));
            if (size <= 0)
                throw new ArgumentException("Size must be positive.", nameof(size));
            if (iterations < 0)
                throw new ArgumentException("Iterations must be non-negative.", nameof(iterations));
            if (iterations > 6)
                throw new ArgumentException("Iterations above 6 may cause performance issues.", nameof(iterations));

            // Create initial tetrahedron vertices
            double height = size * Math.Sqrt(2.0 / 3.0);
            double baseRadius = size / Math.Sqrt(3.0);

            var baseTriangle = GeometryHelpers.RegularPolygon(center, baseRadius, 3);
            Point apex = Point.ByCoordinates(center.X, center.Y, center.Z + height);

            var vertices = new List<Point>(baseTriangle) { apex };
            var tetrahedra = new List<List<Point>> { vertices };

            // Recursive subdivision
            for (int i = 0; i < iterations; i++)
            {
                var newTetrahedra = new List<List<Point>>();

                foreach (var tet in tetrahedra)
                {
                    // Get midpoints of all 6 edges
                    Point m01 = GeometryHelpers.Midpoint(tet[0], tet[1]);
                    Point m02 = GeometryHelpers.Midpoint(tet[0], tet[2]);
                    Point m03 = GeometryHelpers.Midpoint(tet[0], tet[3]);
                    Point m12 = GeometryHelpers.Midpoint(tet[1], tet[2]);
                    Point m13 = GeometryHelpers.Midpoint(tet[1], tet[3]);
                    Point m23 = GeometryHelpers.Midpoint(tet[2], tet[3]);

                    // Create four corner tetrahedra
                    newTetrahedra.Add(new List<Point> { tet[0], m01, m02, m03 });
                    newTetrahedra.Add(new List<Point> { m01, tet[1], m12, m13 });
                    newTetrahedra.Add(new List<Point> { m02, m12, tet[2], m23 });
                    newTetrahedra.Add(new List<Point> { m03, m13, m23, tet[3] });
                }

                tetrahedra = newTetrahedra;
            }

            // Convert to solids
            var solids = new List<Solid>();
            foreach (var tet in tetrahedra)
            {
                try
                {
                    var solid = CreateTetrahedronSolid(tet);
                    if (solid != null)
                        solids.Add(solid);
                }
                catch
                {
                    // Skip degenerate tetrahedra
                }
            }

            return solids;
        }

        /// <summary>
        /// Creates the outline curves of a Sierpinski triangle.
        /// </summary>
        /// <param name="center">Center of the triangle.</param>
        /// <param name="size">Size of the triangle.</param>
        /// <param name="iterations">Subdivision iterations.</param>
        /// <returns name="curves">List of triangle outline curves.</returns>
        /// <search>sierpinski,triangle,outline,wireframe</search>
        public static IList<PolyCurve> TriangleOutlines(
            Point center,
            double size = 100,
            int iterations = 5)
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));

            var vertices = GeometryHelpers.EquilateralTriangle(center, size);
            var triangles = new List<List<Point>> { vertices };

            for (int i = 0; i < iterations; i++)
            {
                var newTriangles = new List<List<Point>>();

                foreach (var tri in triangles)
                {
                    Point mid01 = GeometryHelpers.Midpoint(tri[0], tri[1]);
                    Point mid12 = GeometryHelpers.Midpoint(tri[1], tri[2]);
                    Point mid20 = GeometryHelpers.Midpoint(tri[2], tri[0]);

                    newTriangles.Add(new List<Point> { tri[0], mid01, mid20 });
                    newTriangles.Add(new List<Point> { mid01, tri[1], mid12 });
                    newTriangles.Add(new List<Point> { mid20, mid12, tri[2] });
                }

                triangles = newTriangles;
            }

            var curves = new List<PolyCurve>();
            foreach (var tri in triangles)
            {
                var pts = new List<Point>(tri) { tri[0] };
                curves.Add(PolyCurve.ByPoints(pts, false));
            }

            return curves;
        }

        /// <summary>
        /// Creates a tetrahedron solid from four vertices.
        /// </summary>
        private static Solid? CreateTetrahedronSolid(List<Point> vertices)
        {
            if (vertices.Count != 4)
                return null;

            try
            {
                // Create the four triangular faces
                var faces = new List<Surface>();

                // Face 0-1-2
                var pts012 = new List<Point> { vertices[0], vertices[1], vertices[2], vertices[0] };
                faces.Add(Surface.ByPatch(PolyCurve.ByPoints(pts012, false)));

                // Face 0-1-3
                var pts013 = new List<Point> { vertices[0], vertices[1], vertices[3], vertices[0] };
                faces.Add(Surface.ByPatch(PolyCurve.ByPoints(pts013, false)));

                // Face 0-2-3
                var pts023 = new List<Point> { vertices[0], vertices[2], vertices[3], vertices[0] };
                faces.Add(Surface.ByPatch(PolyCurve.ByPoints(pts023, false)));

                // Face 1-2-3
                var pts123 = new List<Point> { vertices[1], vertices[2], vertices[3], vertices[1] };
                faces.Add(Surface.ByPatch(PolyCurve.ByPoints(pts123, false)));

                // Join faces into a solid
                return Solid.ByJoinedSurfaces(faces);
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Methods for creating Menger sponge fractals.
    /// </summary>
    public static class Menger
    {
        /// <summary>
        /// Creates a Menger sponge fractal.
        /// </summary>
        /// <param name="center">The center point of the sponge.</param>
        /// <param name="size">The size (edge length) of the initial cube.</param>
        /// <param name="iterations">Number of subdivision iterations (0-3 recommended due to exponential growth).</param>
        /// <returns name="solids">List of cube solids forming the sponge.</returns>
        /// <search>menger,sponge,cube,3d,fractal,self-similar</search>
        public static IList<Solid> Sponge(
            Point center,
            double size = 100,
            int iterations = 2)
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));
            if (size <= 0)
                throw new ArgumentException("Size must be positive.", nameof(size));
            if (iterations < 0)
                throw new ArgumentException("Iterations must be non-negative.", nameof(iterations));
            if (iterations > 4)
                throw new ArgumentException("Iterations above 4 will create millions of cubes.", nameof(iterations));

            var cubes = new List<(Point center, double size)>
            {
                (center, size)
            };

            for (int i = 0; i < iterations; i++)
            {
                var newCubes = new List<(Point center, double size)>();

                foreach (var cube in cubes)
                {
                    var subCubes = SubdivideMenger(cube.center, cube.size);
                    newCubes.AddRange(subCubes);
                }

                cubes = newCubes;
            }

            // Create actual cube solids
            var solids = new List<Solid>();
            foreach (var cube in cubes)
            {
                try
                {
                    var solid = CreateCube(cube.center, cube.size);
                    if (solid != null)
                        solids.Add(solid);
                }
                catch
                {
                    // Skip failed cubes
                }
            }

            return solids;
        }

        /// <summary>
        /// Creates the outline curves of a Menger sponge (wireframe).
        /// </summary>
        /// <param name="center">Center of the sponge.</param>
        /// <param name="size">Edge length.</param>
        /// <param name="iterations">Number of iterations.</param>
        /// <returns name="curves">List of edge curves.</returns>
        /// <search>menger,sponge,wireframe,outline</search>
        public static IList<Line> SpongeWireframe(
            Point center,
            double size = 100,
            int iterations = 2)
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));

            var cubes = new List<(Point center, double size)>
            {
                (center, size)
            };

            for (int i = 0; i < iterations; i++)
            {
                var newCubes = new List<(Point center, double size)>();
                foreach (var cube in cubes)
                {
                    newCubes.AddRange(SubdivideMenger(cube.center, cube.size));
                }
                cubes = newCubes;
            }

            var lines = new List<Line>();
            foreach (var cube in cubes)
            {
                lines.AddRange(CreateCubeWireframe(cube.center, cube.size));
            }

            return lines;
        }

        /// <summary>
        /// Subdivides a cube into the 20 sub-cubes of a Menger sponge iteration.
        /// </summary>
        private static List<(Point center, double size)> SubdivideMenger(Point center, double size)
        {
            var subCubes = new List<(Point center, double size)>();
            double newSize = size / 3.0;
            double offset = size / 3.0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        // Skip if two or more coordinates are zero (creates the holes)
                        int zeroCount = (x == 0 ? 1 : 0) + (y == 0 ? 1 : 0) + (z == 0 ? 1 : 0);
                        if (zeroCount >= 2)
                            continue;

                        Point newCenter = Point.ByCoordinates(
                            center.X + x * offset,
                            center.Y + y * offset,
                            center.Z + z * offset
                        );

                        subCubes.Add((newCenter, newSize));
                    }
                }
            }

            return subCubes;
        }

        /// <summary>
        /// Creates a cube solid from center and size.
        /// </summary>
        private static Solid? CreateCube(Point center, double size)
        {
            try
            {
                double half = size / 2.0;
                return Cuboid.ByLengths(center, size, size, size);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Creates wireframe lines for a cube.
        /// </summary>
        private static List<Line> CreateCubeWireframe(Point center, double size)
        {
            var lines = new List<Line>();
            double half = size / 2.0;

            // Create 8 vertices
            var vertices = new List<Point>
            {
                Point.ByCoordinates(center.X - half, center.Y - half, center.Z - half),
                Point.ByCoordinates(center.X + half, center.Y - half, center.Z - half),
                Point.ByCoordinates(center.X + half, center.Y + half, center.Z - half),
                Point.ByCoordinates(center.X - half, center.Y + half, center.Z - half),
                Point.ByCoordinates(center.X - half, center.Y - half, center.Z + half),
                Point.ByCoordinates(center.X + half, center.Y - half, center.Z + half),
                Point.ByCoordinates(center.X + half, center.Y + half, center.Z + half),
                Point.ByCoordinates(center.X - half, center.Y + half, center.Z + half)
            };

            // Bottom face
            lines.Add(Line.ByStartPointEndPoint(vertices[0], vertices[1]));
            lines.Add(Line.ByStartPointEndPoint(vertices[1], vertices[2]));
            lines.Add(Line.ByStartPointEndPoint(vertices[2], vertices[3]));
            lines.Add(Line.ByStartPointEndPoint(vertices[3], vertices[0]));

            // Top face
            lines.Add(Line.ByStartPointEndPoint(vertices[4], vertices[5]));
            lines.Add(Line.ByStartPointEndPoint(vertices[5], vertices[6]));
            lines.Add(Line.ByStartPointEndPoint(vertices[6], vertices[7]));
            lines.Add(Line.ByStartPointEndPoint(vertices[7], vertices[4]));

            // Vertical edges
            lines.Add(Line.ByStartPointEndPoint(vertices[0], vertices[4]));
            lines.Add(Line.ByStartPointEndPoint(vertices[1], vertices[5]));
            lines.Add(Line.ByStartPointEndPoint(vertices[2], vertices[6]));
            lines.Add(Line.ByStartPointEndPoint(vertices[3], vertices[7]));

            return lines;
        }
    }
}

