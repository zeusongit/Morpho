# Sierpinski.TriangleOutlines

Creates the outline curves of a Sierpinski triangle (wireframe version).

## Overview

This node generates just the boundary curves of each triangle in the Sierpinski fractal, without creating filled surfaces. This is useful for line-based visualizations, laser cutting, or when you need lighter geometry.

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the triangle | Required |
| `size` | double | Height of the triangle | 100 |
| `iterations` | int | Number of subdivisions | 5 |

## Output

Returns a list of `PolyCurve` objects, one closed curve per triangle.

## Example Usage

```
// Create Sierpinski triangle outlines
center = Point.ByCoordinates(0, 0, 0);
outlines = Sierpinski.TriangleOutlines(center, 100, 5);
```

## Use Cases

- Laser cutting patterns
- Line drawings and visualizations
- Reduced geometry for large iterations
- Combining with other line-based patterns

## See Also

- `Sierpinski.Triangle` - Filled surface version
- `Menger.SpongeWireframe` - 3D wireframe fractal

