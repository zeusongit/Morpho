# Sierpinski.Triangle

Creates a 2D Sierpinski triangle (gasket) fractal.

## Overview

The Sierpinski triangle (also called Sierpinski gasket or Sierpinski sieve) is a fractal created by recursively removing triangular sections from an equilateral triangle. It was described by Polish mathematician Wacław Sierpiński in 1915.

## How It Works

1. Start with an equilateral triangle
2. Find the midpoints of each side
3. Connect the midpoints to form an inner triangle
4. Remove the inner triangle (leaving 3 corner triangles)
5. Repeat for each remaining triangle

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the triangle | Required |
| `size` | double | Height of the triangle | 100 |
| `iterations` | int | Number of subdivisions (0-8 recommended) | 5 |

## Output

Returns a list of `Surface` objects representing the filled triangles.

## Triangle Count by Iteration

| Iterations | Triangles |
|------------|-----------|
| 0 | 1 |
| 1 | 3 |
| 2 | 9 |
| 3 | 27 |
| 4 | 81 |
| 5 | 243 |

Formula: 3^n triangles at iteration n

## Example Usage

```
// Create a Sierpinski triangle
center = Point.ByCoordinates(0, 0, 0);
sierpinski = Sierpinski.Triangle(center, 100, 5);
```

## Mathematical Properties

- **Hausdorff dimension**: log(3)/log(2) ≈ 1.585
- **Area**: Approaches zero as iterations increase
- **Self-similarity**: Each corner triangle is a smaller copy of the whole

## See Also

- `Sierpinski.TriangleOutlines` - Wireframe version
- `Sierpinski.Tetrahedron` - 3D version
- `Koch.Snowflake` - Another famous 2D fractal

