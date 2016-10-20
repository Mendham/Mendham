Param(
	[string]$buildNumber = "16",
	[string]$tagBuild = $false
)

Write-Output "Starting local build"

Invoke-Psake -taskList Clean,Restore,Test,Pack -properties @{ buildNumber=$buildNumber; tagBuild=$tagBuild }