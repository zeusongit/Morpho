# Koch Curve

Creates a single Koch curve between two points.

## Overview

The Koch curve is the building block of the Koch snowflake. It transforms a straight line into a fractal curve by recursively replacing the middle third of each segment with a triangular bump.

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `startPoint` | Point | Starting point of the curve | Required |
| `endPoint` | Point | Ending point of the curve | Required |
| `iterations` | int | Number of fractal subdivisions | 4 |

## Output

Returns a `PolyCurve` representing the Koch curve.

## Example Usage

```
// Create a Koch curve from (0,0) to (100,0)
start = Point.ByCoordinates(0, 0, 0);
end = Point.ByCoordinates(100, 0, 0);
curve = Koch.Curve(start, end, 4);
```

## Segment Count by Iteration

| Iterations | Segments |
|------------|----------|
| 0 | 1 |
| 1 | 4 |
| 2 | 16 |
| 3 | 64 |
| 4 | 256 |
| 5 | 1,024 |

## See Also

- `Koch.Snowflake` - Complete snowflake made of three Koch curves
- `Koch.AntiSnowflake` - Inverted version

