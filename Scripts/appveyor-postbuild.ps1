$fileVersion = (Get-Item "$env:APPVEYOR_BUILD_FOLDER\Whisper.Apps.Desktop\bin\x64\Release\Whisper.exe").VersionInfo.ProductVersion

$installerSource = "$env:APPVEYOR_BUILD_FOLDER\Whisper.Installers.Desktop\bin\Release\Whisper.msi"
$installerName = "Whisper-x64-v$fileVersion.msi"

Rename-Item -Path $installerSource -NewName $installerName