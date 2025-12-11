# Morpho

**Generative pattern tools for Dynamo** - A hybrid package featuring both Zero Touch C# nodes and Python nodes for creating Islamic geometry, fractals, L-systems, and more.

## Features

### Zero Touch Nodes (C#)
- **Islamic Patterns**: Star polygons, rosettes, girih tiles, tessellation grids
- **Fractals**: Koch snowflakes, Sierpinski triangles/tetrahedrons, Menger sponges

### Python Nodes
- **L-Systems**: Configurable Lindenmayer systems with preset patterns (trees, ferns, dragon curves)
- **Pattern Generators**: Reaction-diffusion, wave interference, spiral patterns

## Installation

### From Package Manager
1. Open Dynamo
2. Go to **Packages** → **Search for a Package**
3. Search for "Morpho"
4. Click **Install**

### Manual Installation
1. Download the latest release from GitHub
2. Extract to your Dynamo packages folder:
   - `%AppData%\Dynamo\Dynamo Core\2.x\packages\Morpho`
   - Or for Revit: `%AppData%\Dynamo\Dynamo Revit\2.x\packages\Morpho`

## Requirements

- Dynamo 4.0+
- .NET 10.0

## Usage Examples

### Islamic Star Pattern
```
// Create an 8-pointed star
StarPolygon.Create(
    center: Point.ByCoordinates(0, 0, 0),
    radius: 50,
    points: 8,
    innerRatio: 0.4
);
```

### Koch Snowflake
```
// Create a Koch snowflake fractal
Koch.Snowflake(
    center: Point.ByCoordinates(0, 0, 0),
    radius: 100,
    iterations: 4
);
```

### L-System Tree (Python)
Use the `Morpho.Pattern.LSystemTree` custom node with preset options:
- `tree` - Realistic tree branching
- `fern` - Barnsley fern
- `bush` - Dense bush pattern
- `dragon` - Dragon curve

## Building from Source

```powershell
# Clone the repository
git clone https://github.com/yourusername/Morpho.git
cd Morpho

# Build the solution
dotnet build Morpho.sln -c Release

# Run the package build script
.\build\build-package.ps1
```

## Project Structure

```
Morpho/
├── src/Morpho/           # C# Zero Touch library
│   ├── Patterns/         # Pattern generation classes
│   └── Core/             # Shared utilities
├── python/               # Python scripts
├── package/              # Dynamo package output
│   ├── bin/              # Compiled DLLs
│   ├── dyf/              # Custom nodes
│   └── extra/python/     # Python scripts
├── samples/              # Example .dyn files
└── tests/                # Unit tests
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

MIT License - see [LICENSE](LICENSE) for details.

