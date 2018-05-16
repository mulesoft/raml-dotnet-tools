param($installPath, $toolsPath, $package, $project)

$id = 'MuleSoft.RAML.ToolsPackage'
$extension = Get-InstalledVsix $id

if(!$extension) {
    Install-Vsix 'MuleSoft.RAML.ToolsPackage.vsix'
}