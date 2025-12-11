"""
Morpho Utilities Module
=======================
Shared utility functions for Morpho Python scripts.

This module provides common helper functions used across other Morpho modules.
"""

import clr
clr.AddReference('ProtoGeometry')
from Autodesk.DesignScript.Geometry import Point, Vector, Line

import math
from typing import List, Tuple, Optional


def degrees_to_radians(degrees: float) -> float:
    """Convert degrees to radians."""
    return degrees * math.pi / 180.0


def radians_to_degrees(radians: float) -> float:
    """Convert radians to degrees."""
    return radians * 180.0 / math.pi


def point_at_angle(center: Point, angle: float, distance: float) -> Point:
    """
    Create a point at given angle and distance from center.
    
    Args:
        center: Center point
        angle: Angle in radians
        distance: Distance from center
        
    Returns:
        New point at the specified location
    """
    x = center.X + distance * math.cos(angle)
    y = center.Y + distance * math.sin(angle)
    return Point.ByCoordinates(x, y, center.Z)


def lerp(p1: Point, p2: Point, t: float) -> Point:
    """
    Linear interpolation between two points.
    
    Args:
        p1: Start point
        p2: End point
        t: Interpolation factor (0-1)
        
    Returns:
        Interpolated point
    """
    return Point.ByCoordinates(
        p1.X + (p2.X - p1.X) * t,
        p1.Y + (p2.Y - p1.Y) * t,
        p1.Z + (p2.Z - p1.Z) * t
    )


def midpoint(p1: Point, p2: Point) -> Point:
    """Calculate midpoint between two points."""
    return lerp(p1, p2, 0.5)


def distance(p1: Point, p2: Point) -> float:
    """Calculate distance between two points."""
    dx = p2.X - p1.X
    dy = p2.Y - p1.Y
    dz = p2.Z - p1.Z
    return math.sqrt(dx*dx + dy*dy + dz*dz)


def centroid(points: List[Point]) -> Point:
    """Calculate centroid of a list of points."""
    if not points:
        raise ValueError("Cannot calculate centroid of empty list")
    
    sum_x = sum(p.X for p in points)
    sum_y = sum(p.Y for p in points)
    sum_z = sum(p.Z for p in points)
    n = len(points)
    
    return Point.ByCoordinates(sum_x / n, sum_y / n, sum_z / n)


def regular_polygon_points(center: Point, radius: float, sides: int, rotation: float = 0) -> List[Point]:
    """
    Generate vertices of a regular polygon.
    
    Args:
        center: Center of polygon
        radius: Circumscribed radius
        sides: Number of sides
        rotation: Rotation offset in radians
        
    Returns:
        List of vertex points
    """
    points = []
    for i in range(sides):
        angle = (2 * math.pi * i / sides) + rotation - math.pi / 2
        points.append(point_at_angle(center, angle, radius))
    return points


def map_value(value: float, in_min: float, in_max: float, out_min: float, out_max: float) -> float:
    """
    Map a value from one range to another.
    
    Args:
        value: Input value
        in_min: Input range minimum
        in_max: Input range maximum
        out_min: Output range minimum
        out_max: Output range maximum
        
    Returns:
        Mapped value
    """
    return out_min + (value - in_min) * (out_max - out_min) / (in_max - in_min)


def clamp(value: float, min_val: float, max_val: float) -> float:
    """Clamp a value to a range."""
    return max(min_val, min(max_val, value))


def flatten_2d(nested_list: List[List]) -> List:
    """Flatten a 2D list into 1D."""
    return [item for sublist in nested_list for item in sublist]


def chunks(lst: List, n: int) -> List[List]:
    """Split a list into chunks of size n."""
    return [lst[i:i + n] for i in range(0, len(lst), n)]


def rotate_point_2d(point: Point, center: Point, angle: float) -> Point:
    """
    Rotate a point around a center in 2D.
    
    Args:
        point: Point to rotate
        center: Center of rotation
        angle: Rotation angle in radians
        
    Returns:
        Rotated point
    """
    cos_a = math.cos(angle)
    sin_a = math.sin(angle)
    
    dx = point.X - center.X
    dy = point.Y - center.Y
    
    new_x = center.X + dx * cos_a - dy * sin_a
    new_y = center.Y + dx * sin_a + dy * cos_a
    
    return Point.ByCoordinates(new_x, new_y, point.Z)


def scale_point(point: Point, center: Point, scale: float) -> Point:
    """
    Scale a point relative to a center.
    
    Args:
        point: Point to scale
        center: Center of scaling
        scale: Scale factor
        
    Returns:
        Scaled point
    """
    dx = point.X - center.X
    dy = point.Y - center.Y
    dz = point.Z - center.Z
    
    return Point.ByCoordinates(
        center.X + dx * scale,
        center.Y + dy * scale,
        center.Z + dz * scale
    )


def golden_ratio() -> float:
    """Return the golden ratio (phi)."""
    return (1 + math.sqrt(5)) / 2


def fibonacci_sequence(n: int) -> List[int]:
    """Generate first n Fibonacci numbers."""
    if n <= 0:
        return []
    if n == 1:
        return [0]
    if n == 2:
        return [0, 1]
    
    seq = [0, 1]
    for i in range(2, n):
        seq.append(seq[-1] + seq[-2])
    return seq

