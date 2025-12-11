# GirihTile.Strapwork

Creates the internal strapwork decoration lines of a Girih tile.

## Overview

Strapwork refers to the decorative lines drawn inside Girih tiles that connect midpoints of edges and radiate from the center. When tiles are placed together, these lines align to create continuous, interlacing patterns across the entire surface.

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the tile | Required |
| `size` | double | Size of the tile | 20 |
| `tileType` | string | Type: "decagon", "pentagon", "hexagon", "bowtie", "rhombus" | "decagon" |

## Output

Returns a list of `Line` objects representing the strapwork pattern.

## Line Types Generated

1. **Radial lines**: From center to each vertex
2. **Cross lines**: Connecting midpoints of opposite edges

## Example Usage

```
// Create strapwork for a decagon
center = Point.ByCoordinates(0, 0, 0);
strapwork = GirihTile.Strapwork(center, 20, "decagon");
```

## Building Complete Patterns

Combine all three GirihTile nodes for complete tiles:

```
outline = GirihTile.Outline(center, size, type);
surface = GirihTile.Create(center, size, type);
decoration = GirihTile.Strapwork(center, size, type);
```

## See Also

- `GirihTile.Create` - Filled tile surface
- `GirihTile.Outline` - Tile boundary curve

