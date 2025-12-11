"""
Morpho Patterns Module
======================
Additional pattern generation algorithms for Dynamo.

This module provides:
- Reaction-diffusion patterns (Gray-Scott model)
- Wave interference patterns
- Spiral generators (Archimedean, logarithmic, Fermat)
- Phyllotaxis patterns (sunflower spirals)

Usage in Dynamo Python node:
    import sys
    sys.path.append(r'path\to\Morpho\extra\python')
    from morpho_patterns import reaction_diffusion, wave_interference, spiral
"""

import clr
clr.AddReference('ProtoGeometry')
from Autodesk.DesignScript.Geometry import Point, Line, NurbsCurve, Surface

import math
from typing import List, Tuple, Optional


# =============================================================================
# Spiral Generators
# =============================================================================

def archimedean_spiral(
    center: Point,
    turns: float = 5,
    spacing: float = 5,
    points_per_turn: int = 36
) -> NurbsCurve:
    """
    Create an Archimedean spiral (constant spacing between turns).
    
    Args:
        center: Center point of the spiral
        turns: Number of complete turns
        spacing: Distance between successive turns
        points_per_turn: Resolution per turn
        
    Returns:
        NurbsCurve representing the spiral
    """
    points = []
    total_points = int(turns * points_per_turn)
    
    for i in range(total_points + 1):
        theta = 2 * math.pi * i / points_per_turn
        r = spacing * theta / (2 * math.pi)
        
        x = center.X + r * math.cos(theta)
        y = center.Y + r * math.sin(theta)
        points.append(Point.ByCoordinates(x, y, center.Z))
    
    return NurbsCurve.ByPoints(points)


def logarithmic_spiral(
    center: Point,
    turns: float = 3,
    growth_rate: float = 0.2,
    initial_radius: float = 1,
    points_per_turn: int = 36
) -> NurbsCurve:
    """
    Create a logarithmic spiral (self-similar, found in nature).
    
    Args:
        center: Center point
        turns: Number of turns
        growth_rate: How quickly the spiral expands
        initial_radius: Starting radius
        points_per_turn: Resolution
        
    Returns:
        NurbsCurve representing the spiral
    """
    points = []
    total_points = int(turns * points_per_turn)
    
    for i in range(total_points + 1):
        theta = 2 * math.pi * i / points_per_turn
        r = initial_radius * math.exp(growth_rate * theta)
        
        x = center.X + r * math.cos(theta)
        y = center.Y + r * math.sin(theta)
        points.append(Point.ByCoordinates(x, y, center.Z))
    
    return NurbsCurve.ByPoints(points)


def fermat_spiral(
    center: Point,
    max_radius: float = 100,
    points: int = 500,
    c: float = 5
) -> List[Point]:
    """
    Create a Fermat spiral (parabolic spiral, points version).
    
    Args:
        center: Center point
        max_radius: Maximum radius
        points: Number of points
        c: Scaling constant
        
    Returns:
        List of points along the spiral
    """
    result = []
    
    for i in range(points):
        theta = i * 2 * math.pi / 10  # Adjust for point distribution
        r = c * math.sqrt(theta)
        
        if r > max_radius:
            break
            
        x = center.X + r * math.cos(theta)
        y = center.Y + r * math.sin(theta)
        result.append(Point.ByCoordinates(x, y, center.Z))
    
    return result


def phyllotaxis_points(
    center: Point,
    count: int = 500,
    scale: float = 5,
    golden_angle: bool = True
) -> List[Point]:
    """
    Generate phyllotaxis pattern (sunflower seed arrangement).
    
    Uses the golden angle for optimal packing.
    
    Args:
        center: Center point
        count: Number of points
        scale: Scale factor for radius
        golden_angle: Use golden angle (137.5°) if True
        
    Returns:
        List of points in phyllotaxis arrangement
    """
    points = []
    angle_increment = 137.5077640500378 if golden_angle else 360 / math.phi
    
    for i in range(count):
        theta = math.radians(i * angle_increment)
        r = scale * math.sqrt(i)
        
        x = center.X + r * math.cos(theta)
        y = center.Y + r * math.sin(theta)
        points.append(Point.ByCoordinates(x, y, center.Z))
    
    return points


# =============================================================================
# Wave Interference Patterns
# =============================================================================

def wave_interference_points(
    width: float = 100,
    height: float = 100,
    resolution: int = 50,
    sources: List[Tuple[float, float]] = None,
    wavelength: float = 10,
    amplitude: float = 5
) -> List[Point]:
    """
    Generate points based on wave interference pattern.
    
    Args:
        width: Width of sampling area
        height: Height of sampling area
        resolution: Points per dimension
        sources: List of (x, y) wave source positions
        wavelength: Wave wavelength
        amplitude: Wave amplitude (Z height)
        
    Returns:
        List of points with Z representing wave height
    """
    if sources is None:
        # Default: two sources creating interference
        sources = [(width * 0.25, height * 0.5), (width * 0.75, height * 0.5)]
    
    points = []
    k = 2 * math.pi / wavelength  # Wave number
    
    for i in range(resolution):
        for j in range(resolution):
            x = width * i / (resolution - 1)
            y = height * j / (resolution - 1)
            
            # Sum contributions from all sources
            z = 0
            for sx, sy in sources:
                distance = math.sqrt((x - sx)**2 + (y - sy)**2)
                z += amplitude * math.sin(k * distance)
            
            points.append(Point.ByCoordinates(x, y, z))
    
    return points


def wave_interference_surface(
    width: float = 100,
    height: float = 100,
    resolution: int = 20,
    sources: List[Tuple[float, float]] = None,
    wavelength: float = 15,
    amplitude: float = 5
) -> Surface:
    """
    Generate a surface from wave interference pattern.
    
    Args:
        width: Width of surface
        height: Height of surface
        resolution: Points per dimension
        sources: Wave source positions
        wavelength: Wave wavelength
        amplitude: Wave amplitude
        
    Returns:
        NurbsSurface showing interference pattern
    """
    if sources is None:
        sources = [(width * 0.25, height * 0.5), (width * 0.75, height * 0.5)]
    
    k = 2 * math.pi / wavelength
    point_grid = []
    
    for i in range(resolution):
        row = []
        for j in range(resolution):
            x = width * i / (resolution - 1)
            y = height * j / (resolution - 1)
            
            z = 0
            for sx, sy in sources:
                distance = math.sqrt((x - sx)**2 + (y - sy)**2)
                z += amplitude * math.sin(k * distance)
            
            row.append(Point.ByCoordinates(x, y, z))
        point_grid.append(row)
    
    return NurbsSurface.ByPoints(point_grid)


def moire_pattern(
    width: float = 100,
    height: float = 100,
    resolution: int = 100,
    frequency1: float = 10,
    frequency2: float = 11,
    angle_offset: float = 5
) -> List[Point]:
    """
    Generate a Moiré interference pattern.
    
    Args:
        width: Pattern width
        height: Pattern height
        resolution: Points per dimension
        frequency1: First wave frequency
        frequency2: Second wave frequency
        angle_offset: Angle difference between patterns (degrees)
        
    Returns:
        List of points with Z as intensity
    """
    points = []
    angle_rad = math.radians(angle_offset)
    
    for i in range(resolution):
        for j in range(resolution):
            x = width * i / (resolution - 1)
            y = height * j / (resolution - 1)
            
            # First pattern
            pattern1 = math.sin(2 * math.pi * frequency1 * x / width)
            
            # Second pattern (rotated)
            x2 = x * math.cos(angle_rad) - y * math.sin(angle_rad)
            pattern2 = math.sin(2 * math.pi * frequency2 * x2 / width)
            
            # Interference
            z = (pattern1 + pattern2) / 2
            
            points.append(Point.ByCoordinates(x, y, z * 5))
    
    return points


# =============================================================================
# Reaction-Diffusion (Gray-Scott Model)
# =============================================================================

def reaction_diffusion(
    width: int = 100,
    height: int = 100,
    iterations: int = 3000,
    feed_rate: float = 0.055,
    kill_rate: float = 0.062,
    seed: int = 42
) -> List[List[float]]:
    """
    Run Gray-Scott reaction-diffusion simulation.
    
    Creates organic Turing-like patterns.
    
    Preset parameters for different patterns:
        - Spots: feed=0.037, kill=0.06
        - Stripes: feed=0.055, kill=0.062
        - Labyrinth: feed=0.029, kill=0.057
        - Holes: feed=0.039, kill=0.058
    
    Args:
        width: Grid width
        height: Grid height
        iterations: Simulation steps
        feed_rate: Feed rate (F) - controls pattern type
        kill_rate: Kill rate (k) - controls pattern type
        seed: Random seed for initial conditions
        
    Returns:
        2D array of concentration values (0-1)
    """
    import random
    random.seed(seed)
    
    # Diffusion rates
    dA, dB = 1.0, 0.5
    dt = 1.0
    
    # Initialize concentrations
    A = [[1.0 for _ in range(width)] for _ in range(height)]
    B = [[0.0 for _ in range(width)] for _ in range(height)]
    
    # Seed with random spots
    for _ in range(10):
        cx = random.randint(10, width - 10)
        cy = random.randint(10, height - 10)
        for dy in range(-3, 4):
            for dx in range(-3, 4):
                if 0 <= cy + dy < height and 0 <= cx + dx < width:
                    B[cy + dy][cx + dx] = 1.0
    
    # Laplacian kernel weights
    def laplacian(grid, x, y):
        """Calculate discrete Laplacian at a point."""
        h, w = len(grid), len(grid[0])
        center = grid[y][x]
        
        # Wrap around edges (toroidal)
        top = grid[(y - 1) % h][x]
        bottom = grid[(y + 1) % h][x]
        left = grid[y][(x - 1) % w]
        right = grid[y][(x + 1) % w]
        
        return top + bottom + left + right - 4 * center
    
    # Run simulation
    for _ in range(iterations):
        # Create new grids for next step
        newA = [[0.0 for _ in range(width)] for _ in range(height)]
        newB = [[0.0 for _ in range(width)] for _ in range(height)]
        
        for y in range(height):
            for x in range(width):
                a = A[y][x]
                b = B[y][x]
                
                lapA = laplacian(A, x, y)
                lapB = laplacian(B, x, y)
                
                reaction = a * b * b
                
                newA[y][x] = a + dt * (dA * lapA - reaction + feed_rate * (1 - a))
                newB[y][x] = b + dt * (dB * lapB + reaction - (kill_rate + feed_rate) * b)
                
                # Clamp values
                newA[y][x] = max(0, min(1, newA[y][x]))
                newB[y][x] = max(0, min(1, newB[y][x]))
        
        A = newA
        B = newB
    
    return B


def reaction_diffusion_to_points(
    pattern: List[List[float]],
    scale_xy: float = 1.0,
    scale_z: float = 10.0,
    threshold: float = 0.0
) -> List[Point]:
    """
    Convert reaction-diffusion pattern to 3D points.
    
    Args:
        pattern: 2D array from reaction_diffusion()
        scale_xy: XY scaling factor
        scale_z: Z scaling factor
        threshold: Minimum value to include
        
    Returns:
        List of 3D points
    """
    points = []
    height = len(pattern)
    width = len(pattern[0]) if height > 0 else 0
    
    for y in range(height):
        for x in range(width):
            value = pattern[y][x]
            if value > threshold:
                points.append(Point.ByCoordinates(
                    x * scale_xy,
                    y * scale_xy,
                    value * scale_z
                ))
    
    return points


def reaction_diffusion_to_surface(
    pattern: List[List[float]],
    scale_xy: float = 1.0,
    scale_z: float = 10.0
) -> Surface:
    """
    Convert reaction-diffusion pattern to a surface.
    
    Args:
        pattern: 2D array from reaction_diffusion()
        scale_xy: XY scaling factor
        scale_z: Z scaling factor
        
    Returns:
        NurbsSurface
    """
    point_grid = []
    height = len(pattern)
    width = len(pattern[0]) if height > 0 else 0
    
    # Sample to reduce point count if needed
    step = max(1, width // 50)
    
    for y in range(0, height, step):
        row = []
        for x in range(0, width, step):
            value = pattern[y][x]
            row.append(Point.ByCoordinates(
                x * scale_xy,
                y * scale_xy,
                value * scale_z
            ))
        point_grid.append(row)
    
    return NurbsSurface.ByPoints(point_grid)


# =============================================================================
# Pattern Presets
# =============================================================================

REACTION_DIFFUSION_PRESETS = {
    "spots": {"feed_rate": 0.037, "kill_rate": 0.06},
    "stripes": {"feed_rate": 0.055, "kill_rate": 0.062},
    "labyrinth": {"feed_rate": 0.029, "kill_rate": 0.057},
    "holes": {"feed_rate": 0.039, "kill_rate": 0.058},
    "worms": {"feed_rate": 0.046, "kill_rate": 0.063},
    "mitosis": {"feed_rate": 0.037, "kill_rate": 0.062},
    "coral": {"feed_rate": 0.062, "kill_rate": 0.063},
}


def get_rd_preset(preset_name: str) -> Tuple[float, float]:
    """
    Get reaction-diffusion parameters for a preset pattern.
    
    Args:
        preset_name: One of "spots", "stripes", "labyrinth", "holes", "worms", "mitosis", "coral"
        
    Returns:
        Tuple of (feed_rate, kill_rate)
    """
    if preset_name not in REACTION_DIFFUSION_PRESETS:
        available = ", ".join(REACTION_DIFFUSION_PRESETS.keys())
        raise ValueError(f"Unknown preset '{preset_name}'. Available: {available}")
    
    preset = REACTION_DIFFUSION_PRESETS[preset_name]
    return preset["feed_rate"], preset["kill_rate"]


# =============================================================================
# Dynamo Entry Point
# =============================================================================

# Expected inputs when used as Dynamo Python node:
# IN[0] = pattern_type: "spiral", "phyllotaxis", "wave", "moire", "reaction_diffusion"
# IN[1..] = pattern-specific parameters

if __name__ == "__main__" or "IN" in dir():
    try:
        pattern_type = IN[0] if len(IN) > 0 else "phyllotaxis"
        
        if pattern_type == "spiral":
            spiral_type = IN[1] if len(IN) > 1 else "archimedean"
            center = IN[2] if len(IN) > 2 else Point.ByCoordinates(0, 0, 0)
            turns = IN[3] if len(IN) > 3 else 5
            
            if spiral_type == "logarithmic":
                OUT = logarithmic_spiral(center, turns)
            elif spiral_type == "fermat":
                OUT = fermat_spiral(center, turns * 20)
            else:
                OUT = archimedean_spiral(center, turns)
                
        elif pattern_type == "phyllotaxis":
            center = IN[1] if len(IN) > 1 else Point.ByCoordinates(0, 0, 0)
            count = IN[2] if len(IN) > 2 else 500
            scale = IN[3] if len(IN) > 3 else 5
            OUT = phyllotaxis_points(center, count, scale)
            
        elif pattern_type == "wave":
            width = IN[1] if len(IN) > 1 else 100
            height = IN[2] if len(IN) > 2 else 100
            resolution = IN[3] if len(IN) > 3 else 30
            OUT = wave_interference_surface(width, height, resolution)
            
        elif pattern_type == "moire":
            width = IN[1] if len(IN) > 1 else 100
            height = IN[2] if len(IN) > 2 else 100
            resolution = IN[3] if len(IN) > 3 else 100
            OUT = moire_pattern(width, height, resolution)
            
        elif pattern_type == "reaction_diffusion":
            preset = IN[1] if len(IN) > 1 else "stripes"
            size = IN[2] if len(IN) > 2 else 100
            iterations = IN[3] if len(IN) > 3 else 3000
            
            feed, kill = get_rd_preset(preset)
            pattern = reaction_diffusion(size, size, iterations, feed, kill)
            OUT = reaction_diffusion_to_surface(pattern)
            
        else:
            OUT = f"Unknown pattern type: {pattern_type}"
            
    except NameError:
        # Not running in Dynamo
        pass

