param(
    # Version to build
    [Parameter(Mandatory)]
    [string]
    [Alias("v")]
    $version,

    # Optional suffix for the version
    [string]
    $suffix,

    # configuration to build against
    [string]
    $config = 'release',

    # push to azure nightly feed ?
    [switch]
    $push = $false
)

$versionString = $version
if (![string]::IsNullOrWhiteSpace($suffix)) {
    $versionString = -join($version, '-', $suffix)
}

$package = "Our.Umbraco.BackOfficeThemes";
$umbpackFile = $package.Replace("-", "_") + "_" + $versionString.Replace("-", "_") + ".zip"
$project = "..\$package\$package.csproj"
$outFolder = ".\$versionString"

# pack the thing
dotnet pack $project -c $config -o $outFolder /p:ContinuousIntegrationBuild=true,version=$versionString

# v8 package
&umbpack pack .\package.xml -v $versionString -n $outFolder\$umbpackFile

if ($push) {
    # push to a nightly . 
    $feedUrl = "https://pkgs.dev.azure.com/jumoo/Public/_packaging/nightly/nuget/v3/index.json";
    &nuget.exe push "$outfolder\*.nupkg" -ApiKey AzureDevOps -src $feedUrl
}
