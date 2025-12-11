"""
Morpho L-Systems Module
=======================
Lindenmayer System (L-System) pattern generation for Dynamo.

This module provides:
- String rewriting engine with configurable rules
- 2D and 3D turtle graphics interpretation
- Preset patterns (trees, ferns, dragon curves, etc.)
- Custom rule support for user-defined L-systems

Usage in Dynamo Python node:
    import sys
    sys.path.append(r'path\to\Morpho\extra\python')
    from morpho_lsystems import LSystem, generate_2d, generate_3d, PRESETS
"""

import clr
clr.AddReference('ProtoGeometry')
from Autodesk.DesignScript.Geometry import Point, Line, Vector, PolyCurve

import math
from typing import List, Dict, Tuple, Optional


# =============================================================================
# L-System Presets
# =============================================================================

PRESETS: Dict[str, Dict] = {
    "tree": {
        "axiom": "F",
        "rules": {"F": "FF+[+F-F-F]-[-F+F+F]"},
        "angle": 25,
        "description": "Realistic tree branching pattern"
    },
    "bush": {
        "axiom": "F",
        "rules": {"F": "FF-[-F+F+F]+[+F-F-F]"},
        "angle": 22.5,
        "description": "Dense bush pattern"
    },
    "fern": {
        "axiom": "X",
        "rules": {"X": "F+[[X]-X]-F[-FX]+X", "F": "FF"},
        "angle": 25,
        "description": "Barnsley fern-like pattern"
    },
    "seaweed": {
        "axiom": "F",
        "rules": {"F": "FF-[XY]+[XY]", "X": "+FY", "Y": "-FX"},
        "angle": 22.5,
        "description": "Swaying seaweed pattern"
    },
    "dragon": {
        "axiom": "FX",
        "rules": {"X": "X+YF+", "Y": "-FX-Y"},
        "angle": 90,
        "description": "Dragon curve fractal"
    },
    "sierpinski": {
        "axiom": "F-G-G",
        "rules": {"F": "F-G+F+G-F", "G": "GG"},
        "angle": 120,
        "description": "Sierpinski triangle using L-system"
    },
    "hilbert": {
        "axiom": "A",
        "rules": {"A": "-BF+AFA+FB-", "B": "+AF-BFB-FA+"},
        "angle": 90,
        "description": "Hilbert space-filling curve"
    },
    "gosper": {
        "axiom": "A",
        "rules": {"A": "A-B--B+A++AA+B-", "B": "+A-BB--B-A++A+B"},
        "angle": 60,
        "description": "Gosper curve (flowsnake)"
    },
    "penrose": {
        "axiom": "[7]++[7]++[7]++[7]++[7]",
        "rules": {
            "6": "81telefonf]++telefonf]----7telefonf][--8telefonf]",
            "7": "+81telefonf]--7telefonf]----6telefonf]++6telefonf]++7telefonf]-",
            "8": "-81telefonf]++7telefonf]++++6telefonf]--6telefonf]--7telefonf]+",
            "telefonf]": ""
        },
        "angle": 36,
        "description": "Penrose tiling pattern"
    },
    "binary_tree": {
        "axiom": "0",
        "rules": {"0": "1[+0]-0", "1": "11"},
        "angle": 45,
        "description": "Simple binary tree"
    },
    "crystal": {
        "axiom": "F+F+F+F",
        "rules": {"F": "FF+F++F+F"},
        "angle": 90,
        "description": "Crystal-like growth pattern"
    },
    "snowflake": {
        "axiom": "F++F++F",
        "rules": {"F": "F-F++F-F"},
        "angle": 60,
        "description": "Koch snowflake variation"
    }
}


# =============================================================================
# L-System Engine
# =============================================================================

class LSystem:
    """
    Lindenmayer System (L-System) string rewriting engine.
    
    Attributes:
        axiom: The initial string (starting pattern)
        rules: Dictionary mapping characters to replacement strings
        angle: Default turning angle in degrees
    """
    
    def __init__(self, axiom: str, rules: Dict[str, str], angle: float = 25):
        """
        Initialize an L-System.
        
        Args:
            axiom: Starting string
            rules: Production rules as {symbol: replacement}
            angle: Default angle for turtle graphics (degrees)
        """
        self.axiom = axiom
        self.rules = rules
        self.angle = angle
    
    @classmethod
    def from_preset(cls, preset_name: str) -> 'LSystem':
        """
        Create an L-System from a preset name.
        
        Args:
            preset_name: Name of the preset (e.g., "tree", "fern")
            
        Returns:
            Configured LSystem instance
        """
        if preset_name not in PRESETS:
            available = ", ".join(PRESETS.keys())
            raise ValueError(f"Unknown preset '{preset_name}'. Available: {available}")
        
        preset = PRESETS[preset_name]
        return cls(
            axiom=preset["axiom"],
            rules=preset["rules"],
            angle=preset["angle"]
        )
    
    def generate(self, iterations: int) -> str:
        """
        Generate the L-System string after n iterations.
        
        Args:
            iterations: Number of rule applications
            
        Returns:
            The resulting string
        """
        current = self.axiom
        
        for _ in range(iterations):
            next_string = ""
            for char in current:
                # Apply rule if exists, otherwise keep character
                next_string += self.rules.get(char, char)
            current = next_string
        
        return current
    
    def interpret_2d(
        self,
        commands: str,
        start_point: Point,
        initial_angle: float = 90,
        length: float = 10,
        length_decay: float = 1.0
    ) -> List[Line]:
        """
        Interpret L-System string as 2D turtle graphics.
        
        Args:
            commands: L-System generated string
            start_point: Starting position
            initial_angle: Initial direction (degrees, 90 = up)
            length: Step length
            length_decay: Length multiplier when pushing state
            
        Returns:
            List of Line segments
        """
        lines = []
        stack = []
        
        x, y = start_point.X, start_point.Y
        z = start_point.Z
        direction = initial_angle
        current_length = length
        
        for cmd in commands:
            if cmd == 'F' or cmd == 'G':
                # Move forward and draw
                rad = math.radians(direction)
                nx = x + current_length * math.cos(rad)
                ny = y + current_length * math.sin(rad)
                
                start = Point.ByCoordinates(x, y, z)
                end = Point.ByCoordinates(nx, ny, z)
                lines.append(Line.ByStartPointEndPoint(start, end))
                
                x, y = nx, ny
                
            elif cmd == 'f':
                # Move forward without drawing
                rad = math.radians(direction)
                x += current_length * math.cos(rad)
                y += current_length * math.sin(rad)
                
            elif cmd == '+':
                # Turn right
                direction -= self.angle
                
            elif cmd == '-':
                # Turn left
                direction += self.angle
                
            elif cmd == '[':
                # Push state onto stack
                stack.append((x, y, direction, current_length))
                current_length *= length_decay
                
            elif cmd == ']':
                # Pop state from stack
                if stack:
                    x, y, direction, current_length = stack.pop()
                    
            elif cmd == '|':
                # Turn around (180 degrees)
                direction += 180
        
        return lines
    
    def interpret_3d(
        self,
        commands: str,
        start_point: Point,
        initial_direction: Vector = None,
        length: float = 10,
        length_decay: float = 1.0
    ) -> List[Line]:
        """
        Interpret L-System string as 3D turtle graphics.
        
        Additional commands for 3D:
            & - Pitch down
            ^ - Pitch up
            \\ - Roll left
            / - Roll right
        
        Args:
            commands: L-System generated string
            start_point: Starting position
            initial_direction: Initial direction vector (default: Z-up)
            length: Step length
            length_decay: Length multiplier for branches
            
        Returns:
            List of Line segments in 3D
        """
        if initial_direction is None:
            initial_direction = Vector.ByCoordinates(0, 0, 1)
        
        lines = []
        stack = []
        
        position = start_point
        heading = initial_direction.Normalized()
        left = Vector.ByCoordinates(-1, 0, 0)
        up = Vector.ByCoordinates(0, 1, 0)
        current_length = length
        
        angle_rad = math.radians(self.angle)
        
        for cmd in commands:
            if cmd == 'F' or cmd == 'G':
                # Move forward and draw
                new_pos = Point.ByCoordinates(
                    position.X + heading.X * current_length,
                    position.Y + heading.Y * current_length,
                    position.Z + heading.Z * current_length
                )
                lines.append(Line.ByStartPointEndPoint(position, new_pos))
                position = new_pos
                
            elif cmd == 'f':
                # Move forward without drawing
                position = Point.ByCoordinates(
                    position.X + heading.X * current_length,
                    position.Y + heading.Y * current_length,
                    position.Z + heading.Z * current_length
                )
                
            elif cmd == '+':
                # Turn right (yaw)
                heading = self._rotate_vector(heading, up, -angle_rad)
                left = self._rotate_vector(left, up, -angle_rad)
                
            elif cmd == '-':
                # Turn left (yaw)
                heading = self._rotate_vector(heading, up, angle_rad)
                left = self._rotate_vector(left, up, angle_rad)
                
            elif cmd == '&':
                # Pitch down
                heading = self._rotate_vector(heading, left, -angle_rad)
                up = self._rotate_vector(up, left, -angle_rad)
                
            elif cmd == '^':
                # Pitch up
                heading = self._rotate_vector(heading, left, angle_rad)
                up = self._rotate_vector(up, left, angle_rad)
                
            elif cmd == '\\':
                # Roll left
                left = self._rotate_vector(left, heading, angle_rad)
                up = self._rotate_vector(up, heading, angle_rad)
                
            elif cmd == '/':
                # Roll right
                left = self._rotate_vector(left, heading, -angle_rad)
                up = self._rotate_vector(up, heading, -angle_rad)
                
            elif cmd == '[':
                # Push state
                stack.append((position, heading, left, up, current_length))
                current_length *= length_decay
                
            elif cmd == ']':
                # Pop state
                if stack:
                    position, heading, left, up, current_length = stack.pop()
        
        return lines
    
    def _rotate_vector(self, v: Vector, axis: Vector, angle: float) -> Vector:
        """Rotate a vector around an axis by an angle (radians)."""
        cos_a = math.cos(angle)
        sin_a = math.sin(angle)
        
        # Rodrigues' rotation formula
        v_rot_x = (v.X * cos_a + 
                   (axis.Y * v.Z - axis.Z * v.Y) * sin_a + 
                   axis.X * (axis.X * v.X + axis.Y * v.Y + axis.Z * v.Z) * (1 - cos_a))
        v_rot_y = (v.Y * cos_a + 
                   (axis.Z * v.X - axis.X * v.Z) * sin_a + 
                   axis.Y * (axis.X * v.X + axis.Y * v.Y + axis.Z * v.Z) * (1 - cos_a))
        v_rot_z = (v.Z * cos_a + 
                   (axis.X * v.Y - axis.Y * v.X) * sin_a + 
                   axis.Z * (axis.X * v.X + axis.Y * v.Y + axis.Z * v.Z) * (1 - cos_a))
        
        return Vector.ByCoordinates(v_rot_x, v_rot_y, v_rot_z)


# =============================================================================
# Convenience Functions for Dynamo
# =============================================================================

def generate_2d(
    preset_or_axiom: str,
    start_point: Point = None,
    iterations: int = 4,
    length: float = 10,
    length_decay: float = 0.7,
    custom_rules: str = None,
    custom_angle: float = None
) -> List[Line]:
    """
    Generate a 2D L-System pattern.
    
    Args:
        preset_or_axiom: Either a preset name ("tree", "fern", etc.) or a custom axiom
        start_point: Starting position (default: origin)
        iterations: Number of iterations
        length: Initial segment length
        length_decay: Length reduction factor per branch level
        custom_rules: Custom rules as "F=FF+F,X=FX" format (if not using preset)
        custom_angle: Custom angle in degrees (if not using preset)
        
    Returns:
        List of Line segments
    """
    if start_point is None:
        start_point = Point.ByCoordinates(0, 0, 0)
    
    if preset_or_axiom in PRESETS:
        # Use preset
        lsystem = LSystem.from_preset(preset_or_axiom)
    else:
        # Custom L-System
        if custom_rules is None:
            raise ValueError("Custom rules required when not using a preset")
        
        rules = {}
        for rule in custom_rules.split(","):
            parts = rule.strip().split("=")
            if len(parts) == 2:
                rules[parts[0].strip()] = parts[1].strip()
        
        angle = custom_angle if custom_angle is not None else 25
        lsystem = LSystem(preset_or_axiom, rules, angle)
    
    commands = lsystem.generate(iterations)
    return lsystem.interpret_2d(commands, start_point, 90, length, length_decay)


def generate_3d(
    preset_or_axiom: str,
    start_point: Point = None,
    iterations: int = 3,
    length: float = 10,
    length_decay: float = 0.7,
    custom_rules: str = None,
    custom_angle: float = None
) -> List[Line]:
    """
    Generate a 3D L-System pattern.
    
    Args:
        preset_or_axiom: Either a preset name or a custom axiom
        start_point: Starting position
        iterations: Number of iterations
        length: Initial segment length
        length_decay: Length reduction per level
        custom_rules: Custom rules if not using preset
        custom_angle: Custom angle if not using preset
        
    Returns:
        List of Line segments in 3D
    """
    if start_point is None:
        start_point = Point.ByCoordinates(0, 0, 0)
    
    if preset_or_axiom in PRESETS:
        lsystem = LSystem.from_preset(preset_or_axiom)
    else:
        if custom_rules is None:
            raise ValueError("Custom rules required when not using a preset")
        
        rules = {}
        for rule in custom_rules.split(","):
            parts = rule.strip().split("=")
            if len(parts) == 2:
                rules[parts[0].strip()] = parts[1].strip()
        
        angle = custom_angle if custom_angle is not None else 25
        lsystem = LSystem(preset_or_axiom, rules, angle)
    
    commands = lsystem.generate(iterations)
    return lsystem.interpret_3d(commands, start_point, None, length, length_decay)


def get_preset_info(preset_name: str = None) -> Dict:
    """
    Get information about L-System presets.
    
    Args:
        preset_name: Specific preset name, or None for all presets
        
    Returns:
        Dictionary with preset information
    """
    if preset_name is not None:
        if preset_name not in PRESETS:
            return {"error": f"Unknown preset: {preset_name}"}
        return {preset_name: PRESETS[preset_name]}
    
    return PRESETS


def list_presets() -> List[str]:
    """
    Get a list of all available preset names.
    
    Returns:
        List of preset names
    """
    return list(PRESETS.keys())


# =============================================================================
# Dynamo Entry Point
# =============================================================================

# When used as a Dynamo Python node, these inputs are expected:
# IN[0] = preset or axiom (string)
# IN[1] = start_point (Point)
# IN[2] = iterations (int)
# IN[3] = length (double)
# IN[4] = length_decay (double)
# IN[5] = mode: "2d" or "3d" (string, optional)
# IN[6] = custom_rules (string, optional)
# IN[7] = custom_angle (double, optional)

if __name__ == "__main__" or "IN" in dir():
    try:
        # Get inputs with defaults
        preset = IN[0] if len(IN) > 0 else "tree"
        start_pt = IN[1] if len(IN) > 1 else Point.ByCoordinates(0, 0, 0)
        iters = IN[2] if len(IN) > 2 else 4
        seg_length = IN[3] if len(IN) > 3 else 10
        decay = IN[4] if len(IN) > 4 else 0.7
        mode = IN[5] if len(IN) > 5 else "2d"
        rules = IN[6] if len(IN) > 6 else None
        angle = IN[7] if len(IN) > 7 else None
        
        if mode.lower() == "3d":
            OUT = generate_3d(preset, start_pt, iters, seg_length, decay, rules, angle)
        else:
            OUT = generate_2d(preset, start_pt, iters, seg_length, decay, rules, angle)
            
    except NameError:
        # Not running in Dynamo, skip
        pass

