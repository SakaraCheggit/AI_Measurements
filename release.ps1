param (
  [string[]]$Games,
  [string]$Version,
  [string]$DotnetTarget = 'net46'
)

$versionPattern = "\d\.\d\.\d"
$tempDir = "$env:TEMP/Measurements"
$7zip = "$env:ProgramFiles/7-zip/7z.exe"

if ($Games.Length -eq 0) {
  $Games = Get-ChildItem "./src/*_Measurements" `
  | ForEach-Object { ($_.Name -split "_")[0] }
}
if ($Version -notmatch $versionPattern) {
  $matchInfo = Select-String -Path .\src\Measurements.Core\MeasurementsPlugin.cs "Version = `"($versionPattern)`";"
  $Version = $matchInfo.Matches.Groups[1].Value
}

if (Test-Path ./dist) { Remove-Item -Recurse -Force ./dist }
if ((Test-Path $tempDir/BepinEx/Plugins) -eq $false) {
  New-Item -ItemType Directory $tempDir/BepinEx/Plugins | Out-Null
}

$maxNameLen = ($Games | Measure-Object -Maximum -Property Length).Maximum

Write-Host "Creating plugins:"
$Games | ForEach-Object {
  $project = "$($_)_Measurements"
  Write-Host "  $project..." -NoNewline
  & dotnet build .\src\$project -c Release -f $DotnetTarget | Out-Null
  $dll = Get-ChildItem ./src/$project/bin/Release/$DotnetTarget/$project.dll
  $tempDll = "$tempDir/BepinEx/Plugins/$($dll.Name)"
  Copy-Item $dll.FullName $tempDll
  $zipFile = "$project v$Version.zip"
  & $7zip a -tzip "./dist/$zipFile" "$tempDir/BepinEx" -mx0 | Out-Null
  Remove-Item $tempDll 
  Write-Host ("`b" * ($project.Length + 3)) -NoNewline
  Write-Host "$project$(" " * ($maxNameLen - $_.Length)) -> ./dist/$zipFile" -ForegroundColor Green
}

Remove-Item -Recurse -Force $tempDir
