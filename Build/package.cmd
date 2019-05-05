
@Echo Packaging
nuget pack ..\Our.Umbraco.BackOfficeThemes\Our.Umbraco.BackOfficeThemes.nuspec -build  -OutputDirectory %1 -version %1  -properties depends=%1

XCOPY %1\*.nupkg c:\source\localgit /y