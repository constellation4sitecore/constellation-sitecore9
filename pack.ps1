# Pack-ConstellationProjects-WithOutput.ps1

# Root path (current directory where you run the script)
$RootPath = Get-Location

# Output directory for all .nupkg files
$OutputDir = Join-Path $RootPath "nupkgoutput"

# Create the output directory if it doesn't exist
if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir | Out-Null
    Write-Host "Created output directory: $OutputDir" -ForegroundColor Cyan
}

Write-Host "Searching for Constellation projects in: $RootPath" -ForegroundColor Cyan
Write-Host "All .nupkg files will be copied to: $OutputDir" -ForegroundColor Cyan
Write-Host ""

# Find all directories starting with "Constellation"
$ConstellationFolders = Get-ChildItem -Path $RootPath -Directory | Where-Object { $_.Name -like "Constellation*" }

if ($ConstellationFolders.Count -eq 0) {
    Write-Warning "No folders starting with 'Constellation' found."
    exit
}

foreach ($folder in $ConstellationFolders) {
    Write-Host "Processing folder: $($folder.Name)" -ForegroundColor Yellow
    
    $csprojFiles = Get-ChildItem -Path $folder.FullName -Filter "*.csproj"
    
    if ($csprojFiles.Count -eq 0) {
        Write-Host "  No .csproj files found. Skipping." -ForegroundColor Gray
        Write-Host ""
        continue
    }
    
    foreach ($proj in $csprojFiles) {
        Write-Host "  Packing: $($proj.Name)" -ForegroundColor Green
        
        # Run nuget pack with Release configuration and explicit output directory
        & .\nuget pack $proj.FullName -Properties Configuration=Release -OutputDirectory $OutputDir
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  Successfully packed and saved to nupkgoutput: $($proj.BaseName).*.nupkg" -ForegroundColor Green
        } else {
            Write-Host "  Failed to pack $($proj.Name)" -ForegroundColor Red
        }
    }
    
    Write-Host ""
}

Write-Host "All done! Check the 'nupkgoutput' folder for your packages." -ForegroundColor Cyan
Write-Host "Path: $OutputDir" -ForegroundColor Cyan