# Menger.Sponge

Creates a Menger sponge fractal - a 3D cube-based fractal.

## Overview

The Menger sponge is a 3D fractal constructed by recursively removing cubic sections from a cube. It was described by Karl Menger in 1926. The sponge has infinite surface area but zero volume, and is one of the most famous 3D fractals.

## How It Works

1. Start with a cube
2. Divide it into 27 smaller cubes (3×3×3 grid)
3. Remove the center cube and the 6 face-center cubes (7 total)
4. This leaves 20 smaller cubes
5. Repeat for each remaining cube

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the sponge | Required |
| `size` | double | Edge length of the initial cube | 100 |
| `iterations` | int | Number of subdivisions (0-3 recommended) | 2 |

## Output

Returns a list of `Solid` (Cuboid) objects forming the sponge.

## Cube Count by Iteration

| Iterations | Cubes |
|------------|-------|
| 0 | 1 |
| 1 | 20 |
| 2 | 400 |
| 3 | 8,000 |
| 4 | 160,000 |

Formula: 20^n cubes at iteration n

## Example Usage

```
// Create a Menger sponge
center = Point.ByCoordinates(0, 0, 0);
sponge = Menger.Sponge(center, 100, 2);
```

## Performance Warning

⚠️ **Keep iterations at 3 or below!** The geometry count grows extremely fast:
- Iteration 3: 8,000 cubes
- Iteration 4: 160,000 cubes (will be very slow)

## Mathematical Properties

- **Hausdorff dimension**: log(20)/log(3) ≈ 2.727
- **Surface area**: Approaches infinity
- **Volume**: Approaches zero
- **Cross-sections**: Sierpinski carpet pattern

## See Also

- `Menger.SpongeWireframe` - Lighter wireframe version
- `Sierpinski.Tetrahedron` - Another 3D fractal

