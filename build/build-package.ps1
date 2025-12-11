<#
.SYNOPSIS
    Build script for Morpho Dynamo package.

.DESCRIPTION
    This script builds the Morpho C# project and assembles the complete
    Dynamo package ready for distribution.

.PARAMETER Configuration
    Build configuration: Debug or Release. Default is Release.

.PARAMETER Clean
    If specified, cleans the build output before building.

.EXAMPLE
    .\build-package.ps1
    Builds the package in Release configuration.

.EXAMPLE
    .\build-package.ps1 -Configuration Debug -Clean
    Cleans and builds in Debug configuration.
#>

param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release",
    
    [switch]$Clean
)

$ErrorActionPreference = "Stop"

# Paths
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RootDir = Split-Path -Parent $ScriptDir
$SrcDir = Join-Path $RootDir "src"
$PythonDir = Join-Path $RootDir "python"
$SamplesDir = Join-Path $RootDir "samples"
$PackageDir = Join-Path $RootDir "package"
$BinDir = Join-Path $PackageDir "bin"
$ExtraPythonDir = Join-Path $PackageDir "extra\python"
$ExtraSamplesDir = Join-Path $PackageDir "extra\samples"
$DyfDir = Join-Path $PackageDir "dyf"
$DocDir = Join-Path $PackageDir "doc"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Morpho Package Build Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Configuration: $Configuration"
Write-Host "Root Directory: $RootDir"
Write-Host ""

# Clean if requested
if ($Clean) {
    Write-Host "Cleaning build output..." -ForegroundColor Yellow
    
    if (Test-Path $BinDir) {
        Remove-Item -Path $BinDir -Recurse -Force
    }
    
    # Clean dotnet build
    dotnet clean (Join-Path $SrcDir "Morpho\Morpho.csproj") -c $Configuration
    
    Write-Host "Clean completed." -ForegroundColor Green
    Write-Host ""
}

# Ensure package directories exist
Write-Host "Creating package directories..." -ForegroundColor Yellow
New-Item -ItemType Directory -Force -Path $BinDir | Out-Null
New-Item -ItemType Directory -Force -Path $ExtraPythonDir | Out-Null
New-Item -ItemType Directory -Force -Path $ExtraSamplesDir | Out-Null
New-Item -ItemType Directory -Force -Path $DyfDir | Out-Null

# Build C# project
Write-Host ""
Write-Host "Building C# project..." -ForegroundColor Yellow
$CsprojPath = Join-Path $SrcDir "Morpho\Morpho.csproj"

dotnet build $CsprojPath -c $Configuration

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "C# build completed successfully." -ForegroundColor Green

# Copy Python scripts
Write-Host ""
Write-Host "Copying Python scripts..." -ForegroundColor Yellow

$PythonFiles = Get-ChildItem -Path $PythonDir -Filter "*.py"
foreach ($file in $PythonFiles) {
    Copy-Item -Path $file.FullName -Destination $ExtraPythonDir -Force
    Write-Host "  Copied: $($file.Name)"
}

Write-Host "Python scripts copied." -ForegroundColor Green

# Copy sample files
Write-Host ""
Write-Host "Copying sample files..." -ForegroundColor Yellow

$SampleFiles = Get-ChildItem -Path $SamplesDir -Filter "*.dyn" -ErrorAction SilentlyContinue
foreach ($file in $SampleFiles) {
    Copy-Item -Path $file.FullName -Destination $ExtraSamplesDir -Force
    Write-Host "  Copied: $($file.Name)"
}

Write-Host "Sample files copied." -ForegroundColor Green

# Copy customization XML if exists
$CustomizationXml = Join-Path $PackageDir "Morpho_DynamoCustomization.xml"
if (Test-Path $CustomizationXml) {
    Copy-Item -Path $CustomizationXml -Destination $BinDir -Force
    Write-Host "Copied customization XML to bin folder." -ForegroundColor Green
}

# Verify package structure
Write-Host ""
Write-Host "Verifying package structure..." -ForegroundColor Yellow

$RequiredFiles = @(
    (Join-Path $PackageDir "pkg.json"),
    (Join-Path $BinDir "Morpho.dll")
)

$AllFilesExist = $true
foreach ($file in $RequiredFiles) {
    if (Test-Path $file) {
        Write-Host "  [OK] $file" -ForegroundColor Green
    } else {
        Write-Host "  [MISSING] $file" -ForegroundColor Red
        $AllFilesExist = $false
    }
}

# List package contents
Write-Host ""
Write-Host "Package Contents:" -ForegroundColor Cyan
Write-Host "  bin/" -ForegroundColor White
Get-ChildItem -Path $BinDir | ForEach-Object { Write-Host "    $($_.Name)" }

Write-Host "  extra/python/" -ForegroundColor White
Get-ChildItem -Path $ExtraPythonDir | ForEach-Object { Write-Host "    $($_.Name)" }

Write-Host "  extra/samples/" -ForegroundColor White
Get-ChildItem -Path $ExtraSamplesDir -ErrorAction SilentlyContinue | ForEach-Object { Write-Host "    $($_.Name)" }

if (Test-Path $DocDir) {
    $DocFiles = Get-ChildItem -Path $DocDir -Filter "*.md"
    if ($DocFiles.Count -gt 0) {
        Write-Host "  doc/ ($($DocFiles.Count) files)" -ForegroundColor White
        $DocFiles | Select-Object -First 5 | ForEach-Object { Write-Host "    $($_.Name)" }
        if ($DocFiles.Count -gt 5) {
            Write-Host "    ... and $($DocFiles.Count - 5) more" -ForegroundColor DarkGray
        }
    }
}

if (Test-Path $DyfDir) {
    $DyfFiles = Get-ChildItem -Path $DyfDir -Filter "*.dyf"
    if ($DyfFiles.Count -gt 0) {
        Write-Host "  dyf/" -ForegroundColor White
        $DyfFiles | ForEach-Object { Write-Host "    $($_.Name)" }
    }
}

# Summary
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
if ($AllFilesExist) {
    Write-Host "  Build Successful!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Package location: $PackageDir"
    Write-Host ""
    Write-Host "To install:" -ForegroundColor Yellow
    Write-Host "  1. Copy the 'package' folder to your Dynamo packages directory"
    Write-Host "  2. Rename it to 'Morpho'"
    Write-Host "  3. Restart Dynamo"
    Write-Host ""
} else {
    Write-Host "  Build completed with warnings!" -ForegroundColor Yellow
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Some expected files are missing. Please check the build output."
    exit 1
}

