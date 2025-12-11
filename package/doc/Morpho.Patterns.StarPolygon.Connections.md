# StarPolygon.Connections

Creates connecting lines between star polygon vertices for interlocking patterns.

## Overview

This node generates lines that connect vertices across the star polygon, creating the characteristic interlocking geometric patterns found in Islamic art. These connection lines form the basis for more complex tessellations.

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the star | Required |
| `radius` | double | Outer radius | 50 |
| `points` | int | Number of star points | 8 |

## Output

Returns a list of `Line` objects connecting vertices across the star.

## Example Usage

```
// Create connection lines for an 8-pointed star
center = Point.ByCoordinates(0, 0, 0);
connections = StarPolygon.Connections(center, 50, 8);
```

## Pattern Building

Combine with `StarPolygon.Create` to build complete Islamic geometric patterns:

1. Create the star outline with `StarPolygon.Create`
2. Add internal structure with `StarPolygon.Connections`
3. Tile using `StarPolygon.Grid` or manual placement

## See Also

- `StarPolygon.Create` - Star outline
- `StarPolygon.Grid` - Tessellated stars

