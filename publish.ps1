# PowerShell script for publishing the NuGet package
param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
    
    [Parameter(Mandatory=$false)]
    [string]$Source = "https://api.nuget.org/v3/index.json"
)

# Read version from csproj file
$csprojPath = "src/PdfQrApiSharp.csproj"
$xml = [xml](Get-Content $csprojPath)
$version = $xml.Project.PropertyGroup.Version

# Build the project
dotnet build src/PdfQrApiSharp.csproj -c Release

# Pack the project
dotnet pack src/PdfQrApiSharp.csproj -c Release

# Publish the package
dotnet nuget push "src/bin/Release/PdfQrApiSharp.$version.nupkg" --api-key $ApiKey --source $Source

Write-Host "Package published successfully!" 