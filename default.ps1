properties {
    $buildNumber = 0
    $preRelease = $null
}

function Restore-Packages
{
    param([string] $DirectoryName)
    & dnu restore ("""" + $DirectoryName + """") --parallel --source "https://www.nuget.org/api/v2/" --source "https://www.myget.org/F/aspnetmaster/api/v2/"
}

function Build-Projects
{
    param([string] $DirectoryName)
    & dnu pack ("""" + $DirectoryName + """") --configuration Release --out .\artifacts\packages; if($LASTEXITCODE -ne 0) { exit 1 }
}

function Test-Projects
{
    param([string] $DirectoryName)
    & dnx ("""" + $DirectoryName + """") test; if($LASTEXITCODE -ne 0) { exit 2 }
}

task ValidateConfig -description "Checking values in config" {
    assert ( 'debug', 'release' -contains $config) 'Config value ($config) is not valid';

    if ($preRelease -eq $null) {
		Write-Output "Prerelease: n/a"
	}
	else {
		Write-Output "Prerelease: $preRelease"
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

task SetBuildNumber -description "Sets the build number that may be added to the version" {
    $buildSuffix = $null

    if($preRelease) {
        $buildNumber = "$preRelease"
    }

    if ($buildNumber -ne 0) {
        
        if ($buildSuffix -ne $null) {
            $buildSuffix = "$buildSuffix-"
        }

        $buildSuffix = $buildSuffix + $buildNumber.ToString().PadLeft(5,'0')

        if ($buildSuffix -ne $null) {
            Write-Output "Set build number to $buildSuffix"
            $env:DNX_BUILD_VERSION = $buildSuffix
        }
    }
}

task Build -depends Clean,Restore,SetBuildNumber -description "Builds every source project" {
    Get-ChildItem -Path .\src -Filter *.xproj -Recurse |
        % { Build-Projects $_.DirectoryName}
}

task Test -depends Restore -description "Runs tests" {
    Get-ChildItem -Path .\test -Filter *.xproj -Recurse |
        % { Test-Projects $_.DirectoryName }
}