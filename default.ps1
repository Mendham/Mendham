properties {
    $buildNumber = 0
    $tagBuild = $false
}

function Restore-Packages ([string] $DirectoryName)
{
    & dotnet restore ("""" + $DirectoryName + """") --source "https://www.nuget.org/api/v2/"
}

function Build-Projects ([string] $DirectoryName, [string] $ProjectName)
{
    Write-Output "Directory: $DirectoryName"
    & dotnet build ("""" + $DirectoryName + """") --configuration Release; if($LASTEXITCODE -ne 0) { exit 1 }
}

function Pack-Projects ([string] $DirectoryName, [string] $ProjectName, [string] $VersionSuffix)
{
    $packCmd = "dotnet pack """ + $DirectoryName + """ --no-build --output "".\artifacts\packages\$ProjectName"""

    if ($VersionSuffix) {
        $packCmd += " --version-suffix $VersionSuffix"
    }

    Write-Output "Directory: $DirectoryName"
    iex $packCmd; if($LASTEXITCODE -ne 0) { exit 1 }
}

function Test-Projects ([string] $Project)
{
    $testCmd = "dotnet test """ + $Project + """ --no-build"

    Write-Output "Testing Project: $Project"
    Write-Output $testCmd

    iex $testCmd; if($LASTEXITCODE -ne 0) { exit 2 }
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

task Build -depends Clean -description "Builds every source project" {
    Get-ChildItem -Path .\src -Filter *.xproj -Recurse |
        % { Build-Projects $_.DirectoryName $_.Directory.Name }
}

task Pack -depends Build -description "Builds every source project" {
    $versionSuffix = $null

    if ($tagBuild -eq $false) {
        if ($buildNumber -ne 0) {
        
            $versionSuffix = "build-" + $buildNumber.ToString().PadLeft(5,'0')
            Write-Output "Using version suffix $versionSuffix" 
        }
    }
    else {
        Write-Output "Tagged Build. Not applying a version suffix"
    }

    Get-ChildItem -Path .\src -Filter *.xproj -Recurse |
        % { Pack-Projects $_.DirectoryName $_.Directory.Name $versionSuffix }
}

task Test -depends Build -description "Runs tests" {
    Get-ChildItem -Path .\test -Filter project.json -Recurse |
        ? {$_.Directory.FullName -notmatch "test\\Resources" } |
        % { Test-Projects $_.FullName }
}