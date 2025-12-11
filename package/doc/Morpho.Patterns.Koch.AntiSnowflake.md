# Koch Anti-Snowflake

Creates a Koch snowflake with inward-pointing bumps instead of outward.

## Overview

The anti-snowflake (or Koch antisnowflake) is a variation where the triangular bumps point inward toward the center rather than outward. This creates a star-like shape that gets increasingly intricate with more iterations.

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the anti-snowflake | Required |
| `radius` | double | Distance from center to vertices | 100 |
| `iterations` | int | Number of fractal subdivisions | 4 |

## Output

Returns a `PolyCurve` representing the anti-snowflake.

## Example Usage

```
// Create an anti-snowflake
center = Point.ByCoordinates(0, 0, 0);
antiSnowflake = Koch.AntiSnowflake(center, 100, 3);
```

## Visual Comparison

- **Snowflake**: Bumps point outward, creating a "fluffy" appearance
- **Anti-Snowflake**: Bumps point inward, creating a "spiky" appearance

## See Also

- `Koch.Snowflake` - Standard outward-pointing version
- `Koch.Curve` - Single Koch curve segment

