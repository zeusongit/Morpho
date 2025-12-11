# GirihTile.Outline

Creates the outline curve of a Girih tile without filling.

## Overview

This node creates just the boundary curve of a Girih tile, useful when you need the outline for further manipulation, cutting patterns, or when surfaces are not needed.

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the tile | Required |
| `size` | double | Size of the tile | 20 |
| `tileType` | string | Type: "decagon", "pentagon", "hexagon", "bowtie", "rhombus" | "decagon" |

## Output

Returns a `PolyCurve` representing the tile outline.

## Example Usage

```
// Create a pentagon outline
center = Point.ByCoordinates(0, 0, 0);
outline = GirihTile.Outline(center, 20, "pentagon");
```

## Use Cases

- Laser cutting templates
- CNC milling paths
- Line-based visualizations
- Combining with strapwork patterns

## See Also

- `GirihTile.Create` - Filled surface version
- `GirihTile.Strapwork` - Internal decoration lines

