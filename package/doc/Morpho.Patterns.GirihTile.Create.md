# GirihTile.Create

Creates a Girih tile - the interlocking geometric shapes used in Islamic architecture.

## Overview

Girih tiles are a set of five shapes used in Islamic architecture to create complex, aperiodic tessellations. These tiles were used centuries before Western mathematicians discovered similar patterns (Penrose tiles). The tiles interlock to create intricate geometric patterns found in mosques, palaces, and manuscripts throughout the Islamic world.

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the tile | Required |
| `size` | double | Size of the tile (edge length for most types) | 20 |
| `tileType` | string | Type of tile (see below) | "decagon" |

## Tile Types

| Type | Description | Sides |
|------|-------------|-------|
| `"decagon"` | Regular 10-sided polygon | 10 |
| `"pentagon"` | Regular 5-sided polygon | 5 |
| `"hexagon"` | Regular 6-sided polygon | 6 |
| `"bowtie"` | Concave hexagon (bow-tie shape) | 6 |
| `"rhombus"` | Golden ratio rhombus | 4 |

## Output

Returns a `Surface` representing the tile.

## Example Usage

```
// Create a decagon Girih tile
center = Point.ByCoordinates(0, 0, 0);
tile = GirihTile.Create(center, 20, "decagon");
```

## Historical Context

Girih patterns date back to the 10th century and reached their peak sophistication in the 15th century. The Darb-i Imam shrine in Isfahan, Iran (1453) contains girih patterns that are mathematically equivalent to Penrose tilings, discovered by Roger Penrose in the 1970s.

## See Also

- `GirihTile.Outline` - Just the tile outline (no fill)
- `GirihTile.Strapwork` - Internal decoration lines
- `StarPolygon.Create` - Simpler star patterns

