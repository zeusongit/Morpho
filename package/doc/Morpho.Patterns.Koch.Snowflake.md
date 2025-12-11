# Koch Snowflake

Creates a Koch snowflake fractal - one of the most famous mathematical fractals.

## Overview

The Koch snowflake is constructed by starting with an equilateral triangle and recursively adding smaller triangular "bumps" to each edge. Each iteration increases the perimeter while the area converges to a finite value.

![Koch Snowflake Animation](https://upload.wikimedia.org/wikipedia/commons/d/d9/KochFlake.svg)

## How It Works

1. Start with an equilateral triangle
2. Divide each edge into three equal parts
3. Replace the middle third with two sides of a smaller equilateral triangle (pointing outward)
4. Repeat for each new edge

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the snowflake | Required |
| `radius` | double | Distance from center to vertices | 100 |
| `iterations` | int | Number of fractal subdivisions (0-6 recommended) | 4 |

## Output

Returns a list of `Line` segments that form the complete snowflake outline.

## Example Usage

```
// Create a Koch snowflake at the origin
center = Point.ByCoordinates(0, 0, 0);
snowflake = Koch.Snowflake(center, 100, 4);
```

## Tips

- **Iterations 0**: Simple equilateral triangle (3 lines)
- **Iterations 1-2**: Basic fractal shape visible
- **Iterations 3-4**: Good balance of detail and performance
- **Iterations 5+**: High detail but slower to compute

## Mathematical Properties

- **Perimeter**: Increases by 4/3 with each iteration (approaches infinity)
- **Area**: Converges to 8/5 times the original triangle's area
- **Fractal Dimension**: log(4)/log(3) ≈ 1.26

## See Also

- `Koch.Curve` - Single Koch curve between two points
- `Koch.AntiSnowflake` - Snowflake with inward-pointing bumps
- `Sierpinski.Triangle` - Another famous 2D fractal

