# Rosette.Nested

Creates a multi-layer rosette with nested concentric patterns.

## Overview

This node generates multiple rosette layers at decreasing radii, with alternating rotations to create complex, layered floral patterns. This technique is commonly used in elaborate Islamic geometric designs and mandala-style artwork.

## Parameters

| Input | Type | Description | Default |
|-------|------|-------------|---------|
| `center` | Point | Center point | Required |
| `outerRadius` | double | Radius of the outermost layer | 50 |
| `layers` | int | Number of nested layers (minimum 1) | 3 |
| `petals` | int | Number of petals per layer | 8 |
| `petalDepth` | double | Depth of petal curves (0-1) | 0.3 |

## Output

Returns a list of all `Curve` objects from all layers combined.

## Layer Behavior

- Each inner layer is smaller by `outerRadius / layers`
- Alternate layers are rotated by half a petal width
- This creates an interlocking, woven appearance

## Example Usage

```
// Create a 3-layer nested rosette
center = Point.ByCoordinates(0, 0, 0);
nestedRosette = Rosette.Nested(center, 50, 3, 8, 0.3);
```

## Design Tips

- Use 3-5 layers for balanced complexity
- Match petal count to star polygons for cohesive designs
- Combine with `StarPolygon.Create` for hybrid patterns

## See Also

- `Rosette.Create` - Single rosette layer
- `StarPolygon.Create` - Straight-edged alternatives

