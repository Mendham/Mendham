Param(
	[string]$buildNumber = "1",
	[string]$preRelease = $true
)

Write-Output "Starting local build"

Invoke-Psake -taskList Build,Test -properties @{ buildNumber=$buildNumber; preRelease=$preRelease }

if ($env:DNX_BUILD_VERSION)
{
    Write-Output "Removing DNX_BUILD_VERSION"
    $env:DNX_BUILD_VERSION = $null
}