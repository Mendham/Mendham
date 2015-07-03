Param(
	[string]$buildNumber = "1",
	[string]$preRelease = $null,
	[string]$version = "0.1.0.0"
)

Write-Output "Starting local build"

Invoke-Psake -taskList Build,Test -properties @{ buildNumber=$buildNumber; preRelease=$preRelease;version=$version }

if ($env:DNX_BUILD_VERSION)
{
    Write-Output "Removing DNX_BUILD_VERSION"
    $env:DNX_BUILD_VERSION = $null
}