properties {
    $buildNumber = 0
    $preRelease = $true
    $tagBuild = $false
}

function Restore-Packages ([string] $DirectoryName)
{
    & dnu restore ("""" + $DirectoryName + """") --parallel --source "https://www.nuget.org/api/v2/" --source "https://www.myget.org/F/aspnetmaster/api/v2/"
}

function Build-Projects ([string] $DirectoryName, [string] $ProjectName)
{
    Write-Output "Directory: $DirectoryName"
    & dnu pack ("""" + $DirectoryName + """") --configuration Release --out ".\artifacts\packages\$ProjectName"; if($LASTEXITCODE -ne 0) { exit 1 }
}

function Test-Projects ([string] $Project)
{
    Write-Output "Testing Project: $Project"
    & dnx -p ("""" + $Project + """") test; if($LASTEXITCODE -ne 0) { exit 2 }
}

task ValidateConfig -description "Checking values in config" {
    assert ( 'debug', 'release' -contains $config) 'Config value ($config) is not valid';

    if ($preRelease -eq $null) {
		Write-Output "Prerelease: n/a"
	}
	else {
		Write-Output "PreRelease: $preRelease"
	}

    Write-Output 'Config value is valid';
}

task Clean -description "Deletes all build artifacts" {
    if(Test-Path .\artifacts)
    {
        Remove-Item .\artifacts -Force -Recurse
    }
}

task Restore -description "Restores packages for all projects" {
    Restore-Packages (Get-Item -Path ".\" -Verbose).FullName
}

task SetBuildSuffix -description "Sets the build suffix that may be added to the version" {
    if ($tagBuild -eq $false)
    {
        if ($buildNumber -ne 0) {
        
            $env:DNX_BUILD_VERSION = $buildNumber.ToString().PadLeft(5,'0')
        }
    }
}

task Build -depends Clean,Restore,SetBuildSuffix -description "Builds every source project" {
    Get-ChildItem -Path .\src -Filter *.xproj -Recurse |
        % { Build-Projects $_.DirectoryName $_.Directory.Name }
}

task Test -depends Restore -description "Runs tests" {
    Get-ChildItem -Path .\test -Filter project.json -Recurse |
        % { Test-Projects $_.FullName }
}