param
(
	[string]$Path
)

if(!$Path)
{
	Write-Host "Path is not set."
	exit 1
}

if(!(Test-Path $Path))
{
	Write-Host "Path not found."
	exit 1
}

New-item alias:git -value 'C:\Program Files\Git\bin\git.exe' | Out-Null

if(!(Get-Command git -TotalCount 1 -ErrorAction SilentlyContinue)) 
{
	Write-Host "git command could not be found."
	exit 1
}

$revision = git describe --always
if($?) 
{
	(Get-Content $Path) |
	Foreach-Object {$_ -replace "\[assembly: AssemblyInformationalVersion\("".*""\)\]", "[assembly: AssemblyInformationalVersion(""${revision}"")]"} |
	Set-Content $Path
} 
else
{
	Write-Host "git returned an error"
	exit 1
}
