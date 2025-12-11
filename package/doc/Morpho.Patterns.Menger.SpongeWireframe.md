# Menger.SpongeWireframe

Creates the wireframe (edges only) of a Menger sponge.

## Overview

This node generates only the edge lines of each cube in the Menger sponge fractal, without creating solid geometry. This results in much lighter geometry that renders faster and is suitable for visualization or fabrication.

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the sponge | Required |
| `size` | double | Edge length of the initial cube | 100 |
| `iterations` | int | Number of subdivisions | 2 |

## Output

Returns a list of `Line` objects representing all cube edges.

## Line Count by Iteration

Each cube has 12 edges, so:

| Iterations | Lines |
|------------|-------|
| 0 | 12 |
| 1 | 240 |
| 2 | 4,800 |
| 3 | 96,000 |

## Example Usage

```
// Create a Menger sponge wireframe
center = Point.ByCoordinates(0, 0, 0);
wireframe = Menger.SpongeWireframe(center, 100, 2);
```

## Advantages Over Solid Version

- **Faster rendering**: Lines are simpler than solids
- **Lower memory**: No surface/solid data
- **Better visibility**: Can see the internal structure
- **Fabrication ready**: Direct output for wire-frame models

## See Also

- `Menger.Sponge` - Solid cube version
- `Sierpinski.TriangleOutlines` - 2D wireframe fractal

