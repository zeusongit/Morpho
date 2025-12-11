# Rosette.Create

Creates a rosette pattern with curved petals.

## Overview

Rosettes are flower-like patterns featuring curved petals arranged radially around a center point. They are commonly found in Islamic geometric art, Gothic architecture, and decorative design. Unlike star polygons which have straight edges, rosettes use arcs to create softer, more organic shapes.

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point of the rosette | Required |
| `radius` | double | Outer radius of the rosette | 50 |
| `petals` | int | Number of petals (minimum 3) | 8 |
| `petalDepth` | double | Depth of petal curves (0-1) | 0.3 |

## Output

Returns a list of `Curve` objects (arcs) forming the rosette pattern.

## Petal Depth Effect

- **0.1-0.2**: Shallow curves, subtle petal definition
- **0.3-0.4**: Classic rosette appearance
- **0.5-0.7**: Deep curves, dramatic petal shapes
- **0.8-0.9**: Very deep, almost touching center

## Example Usage

```
// Create an 8-petal rosette
center = Point.ByCoordinates(0, 0, 0);
rosette = Rosette.Create(center, 50, 8, 0.3);
```

## Historical Use

Rosettes appear extensively in:
- Islamic mosque decorations
- Gothic cathedral rose windows
- Classical Greek and Roman ornament
- Art Nouveau design

## See Also

- `Rosette.Nested` - Multiple concentric rosette layers
- `StarPolygon.Create` - Straight-edged star patterns

