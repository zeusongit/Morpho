# StarPolygon.Grid

Creates a tessellated grid of star polygons.

## Overview

This node generates a rectangular grid of evenly-spaced star polygons, useful for creating repeating Islamic geometric patterns, facade designs, or decorative panels.

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `width` | double | Total width of the pattern grid | 200 |
| `height` | double | Total height of the pattern grid | 200 |
| `cellSize` | double | Size of each grid cell | 50 |
| `points` | int | Number of points on each star | 8 |
| `innerRatio` | double | Inner radius ratio (0-1) | 0.4 |

## Output

Returns a list of `PolyCurve` objects, one for each star in the grid.

## Example Usage

```
// Create a 4x4 grid of 8-pointed stars
stars = StarPolygon.Grid(200, 200, 50, 8, 0.4);
```

## Grid Calculation

The number of stars in each direction is calculated as:
- Columns = ceil(width / cellSize)
- Rows = ceil(height / cellSize)

Each star is sized to fit within its cell with a small margin (90% of cell size).

## Tips

- Combine with `StarPolygon.Connections` to create interlocking patterns
- Use different `innerRatio` values across the grid for variation
- Export to DXF for laser cutting or CNC fabrication

## See Also

- `StarPolygon.Create` - Single star polygon
- `StarPolygon.Connections` - Connecting lines between vertices

