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

Push-Location $PSScriptRoot

Import-Module .\psake.psm1

Invoke-Psake -taskList Test,Pack -properties @{ buildNumber=$buildNumber; tagBuild=$tagBuild }

Pop-Location