$buildNumber = 0;
$tagBuild = $false;

if ($env:APPVEYOR_BUILD_NUMBER) {
    $buildNumber = $env:APPVEYOR_BUILD_NUMBER
}

if ($env:APPVEYOR) {
    
    Write-Output "Building branch: $env:APPVEYOR_REPO_BRANCH"

    Write-Output "Current Environment Variables"
    # Dumps Environment
    ls env:

    Write-Output "dotnet CLI version"
    dotnet --version

    if ($env:APPVEYOR_REPO_TAG -eq $true -or $env:APPVEYOR_REPO_TAG -eq "True") {
        $tagBuild = $true;
    }

    if ($tagBuild) {

        $tagText = $env:APPVEYOR_REPO_TAG_NAME
        Write-Output "Building tag: $tagText"

        if ($tagText -match "[a-zA-Z]") {
            Write-Output "Is Prerelease: $preReleaseLabel"
            $preRelease = $true
        }
        else {
            Write-Output "Not PreRelease"
            $preRelease = $false
        }
    }
    else {
        Write-Output "No tag applied to build. Is PreRelease"
        $preRelease = $true
    }
}

function Install-DotnetCLI() {
    nuget update -self

    Invoke-WebRequest "https://raw.githubusercontent.com/dotnet/cli/rel/1.0.0-preview2/scripts/obtain/dotnet-install.ps1" -OutFile ".\dotnet-install.ps1"

    $env:DOTNET_INSTALL_DIR = "$pwd\.dotnetcli"
    
    & .\dotnet-install.ps1 -Channel "preview" -Version "1.0.0-preview2-003121" -InstallDir "$env:DOTNET_INSTALL_DIR"
}

Push-Location $PSScriptRoot

Install-DotnetCLI

Import-Module .\psake.psm1

Invoke-Psake -taskList Restore,Build,Test,Pack -properties @{ buildNumber=$buildNumber; tagBuild=$tagBuild }

Pop-Location