param
(
	[string]$Path
)

if(!$Path)
{
	Write-Host "Path is not set."
	return
}

if(!(Test-Path $Path))
{
	Write-Host "Path not found."
	return
}

New-item alias:git -value 'C:\Program Files\Git\bin\git.exe' | Out-Null

if(!(Get-Command git -TotalCount 1 -ErrorAction SilentlyContinue)) 
{
	Write-Host "git command could not be found."
	return
}

$revision = git describe --long --always --dirty
if($?) 
{
	(Get-Content $Path -Encoding UTF8) |
	Foreach-Object {$_ -replace "\[assembly: AssemblyInformationalVersion\("".*""\)\]", "[assembly: AssemblyInformationalVersion(""${revision}"")]"} |
	Set-Content $Path -Encoding UTF8
	Write-Host "successfully updated to: $revision."
} 
else
{
	Write-Host "git returned an error: $lastexitcode."
}
