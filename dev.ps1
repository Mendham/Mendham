Param(
	[string]$buildNumber = "15",
	[string]$tagBuild = $false
)

Write-Output "Starting local build"

Invoke-Psake -taskList Restore,Build,Test,Pack -properties @{ buildNumber=$buildNumber; tagBuild=$tagBuild }