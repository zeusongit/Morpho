# Sierpinski.Tetrahedron

Creates a 3D Sierpinski tetrahedron (pyramid) fractal.

## Overview

The Sierpinski tetrahedron (also called Sierpinski pyramid or tetrix) is the 3D analog of the Sierpinski triangle. It's created by recursively removing octahedral sections from a tetrahedron, leaving four smaller corner tetrahedra.

## How It Works

1. Start with a regular tetrahedron
2. Find midpoints of all six edges
3. Connect midpoints to form four corner tetrahedra and one central octahedron
4. Remove the central octahedron
5. Repeat for each remaining tetrahedron

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the base | Required |
| `size` | double | Edge length of the tetrahedron | 100 |
| `iterations` | int | Number of subdivisions (0-5 recommended) | 4 |

## Output

Returns a list of `Solid` objects representing the tetrahedra.

## Tetrahedron Count by Iteration

| Iterations | Tetrahedra |
|------------|------------|
| 0 | 1 |
| 1 | 4 |
| 2 | 16 |
| 3 | 64 |
| 4 | 256 |
| 5 | 1,024 |

Formula: 4^n tetrahedra at iteration n

## Example Usage

```
// Create a Sierpinski tetrahedron
center = Point.ByCoordinates(0, 0, 0);
pyramid = Sierpinski.Tetrahedron(center, 100, 4);
```

## Performance Note

Due to exponential growth, limit iterations to 5 or fewer for reasonable performance.

## See Also

- `Sierpinski.Triangle` - 2D version
- `Menger.Sponge` - Another 3D fractal

