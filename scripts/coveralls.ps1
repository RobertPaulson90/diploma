# CONFIGURATION
if (!$env:APPVEYOR) { $env:CONFIGURATION = "Debug" }

Write-Host "Configuration: " -NoNewline
Write-Host "$env:CONFIGURATION" -ForegroundColor "Green";

$ToolsFolderName = "packages"
$TestsResultsFolderName = "TestsResults"

$FilesForTest = (ls -r .\bin\$env:CONFIGURATION\*\*.Tests.dll | % FullName | sort-object -Unique) -join " "

$FilesForTestString = $FilesForTest -replace ' ',"`n"
Write-Host "Files to test: " -NoNewline
Write-Host "`n$FilesForTestString" -ForegroundColor "Green";

$Filters = "+[Diploma.*]* -[Diploma.*.Tests]*"
# STEP 1. Get solution and tools folders

$SolutionFolderPath  = Convert-Path (Resolve-Path -Path ".\")
Write-Host "Solution folder path: "   -NoNewline
Write-Host "$SolutionFolderPath" -ForegroundColor "Green";

$ToolsFolderPath = Convert-Path (Resolve-Path -Path "$SolutionFolderPath\$ToolsFolderName")
Write-Host "Tools folder path: " -NoNewline
Write-Host "$ToolsFolderPath" -ForegroundColor "Green";


# STEP 2. Get TestRunner, OpenCover, Coveralls

$TestRunnerPath = Get-ChildItem -Path "$ToolsFolderPath" -Filter "NUnit.ConsoleRunner*" -Directory | % { "$($_.FullName)\tools\nunit3-console.exe" }
Write-Host "TestRunner path: " -NoNewline
Write-Host "$TestRunnerPath" -ForegroundColor "Green";

$OpenCoverPath = Get-ChildItem -Path "$ToolsFolderPath" -Filter "Opencover*" -Directory | % { "$($_.FullName)\tools\OpenCover.Console.exe" }
Write-Host "OpenCover path: " -NoNewline
Write-Host "$OpenCoverPath" -ForegroundColor "Green";

$CoverallsPath = Get-ChildItem -Path "$ToolsFolderPath" -Filter "coveralls.net.*" -Directory | % { "$($_.FullName)\tools\csmacnz.Coveralls.exe" }
Write-Host "Coveralls path: " -NoNewline
Write-Host "$CoverallsPath" -ForegroundColor "Green";


# STEP 3. Create TestResults folder

$TestsResultsFolderPath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("$SolutionFolderPath\$TestsResultsFolderName")
Write-Host "TestsResults folder path: " -NoNewline
Write-Host "$TestsResultsFolderPath" -ForegroundColor "Green";

If (Test-Path -Path "$TestsResultsFolderPath")
{
    Remove-Item "$TestsResultsFolderPath" -Recurse
}

New-Item -ItemType Directory -Force -Path $TestsResultsFolderPath | Out-Null


# STEP 4. Setup OpenCover output

$OpenCoverOutputFilePath = "$TestsResultsFolderPath\OpenCoverOutput.xml"
Write-Host "OpenCover output file path: " -NoNewline
Write-Host "$OpenCoverOutputFilePath" -ForegroundColor "Green";


# STEP 5. Execute OpenCover

Write-Host "Running OpenCover" -BackgroundColor "Yellow" -ForegroundColor "Black";

& $OpenCoverPath "-register:user" "-target:$TestRunnerPath" "-targetargs:`"$FilesForTest`"" "-filter:$Filters" "-output:$OpenCoverOutputFilePath" "-excludebyfile:*.Designer.cs"


#STEP 6. Send results to coveralls

Write-Host "Sending results to coveralls.net" -BackgroundColor "Yellow" -ForegroundColor "Black";


& $CoverallsPath --opencover "-i $OpenCoverOutputFilePath" --repoToken $env:COVERALLS_REPO_TOKEN --useRelativePaths --commitId $env:APPVEYOR_REPO_COMMIT --commitBranch $env:APPVEYOR_REPO_BRANCH --commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR --commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL --commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE --jobId $env:APPVEYOR_BUILD_NUMBER --serviceName appveyor

# STEP 7. Uploading test results to AppVeyor

Write-Host "Uploading test results to AppVeyor" -BackgroundColor "Yellow" -ForegroundColor "Black";

$wc = New-Object 'System.Net.WebClient'
$wc.UploadFile("https://ci.appveyor.com/api/testresults/nunit3/$($env:APPVEYOR_JOB_ID)", (Resolve-Path -Path ".\TestResult.xml"))