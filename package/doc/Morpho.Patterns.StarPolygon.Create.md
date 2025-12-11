# StarPolygon.Create

Creates an N-pointed star polygon commonly found in Islamic geometric art.

## Overview

Star polygons are fundamental elements in Islamic geometric patterns. They consist of alternating outer points and inner valleys, creating the characteristic star shape. These patterns have been used in Islamic architecture and art for over a thousand years.

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the star | Required |
| `radius` | double | Outer radius of the star | 50 |
| `points` | int | Number of star points (minimum 3) | 8 |
| `innerRatio` | double | Ratio of inner to outer radius (0-1) | 0.4 |

## Output

Returns a closed `PolyCurve` representing the star polygon.

## Common Star Types

| Points | Name | Common Use |
|--------|------|------------|
| 5 | Pentagram | Western symbolism |
| 6 | Hexagram | Star of David |
| 8 | Octagram | Islamic art (most common) |
| 10 | Decagram | Moorish architecture |
| 12 | Dodecagram | Complex patterns |

## Inner Ratio Effect

- **0.1-0.2**: Very deep, narrow points
- **0.3-0.4**: Classic star appearance
- **0.5-0.6**: Softer, rounder star
- **0.7-0.9**: Barely visible points

## Example Usage

```
// Create an 8-pointed Islamic star
center = Point.ByCoordinates(0, 0, 0);
star = StarPolygon.Create(center, 50, 8, 0.4);
```

## Historical Context

The 8-pointed star (octagram) is particularly significant in Islamic art, symbolizing the eight directions of space and appearing frequently in mosque decorations, tile patterns, and manuscript illuminations.

## See Also

- `StarPolygon.Grid` - Create a tessellated grid of stars
- `StarPolygon.Connections` - Create connecting lines between star vertices
- `Rosette.Create` - Curved petal patterns
- `GirihTile.Create` - Traditional Islamic tile shapes

