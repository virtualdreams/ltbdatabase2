param
(
	[string]$file
)

if(!$file)
{
	Write-Host "Path is not set."
	return
}

if(!(Test-Path $file))
{
	Write-Host "Path not found."
	return
}

### pattern to search
$pattern = "\[assembly: AssemblyFileVersion\(""(\d+)\.(\d+)\.(\d+)\.(\d+)\""\)\]"
$update = $false

$content = Get-Content $file -Encoding UTF8
$replace = @()
foreach($line in $content) {
	if($line -match $pattern) {
		$major = [int]$matches[1]
		$minor = [int]$matches[2]
		$build = [int]$matches[3]
		$revision = [int]$matches[4]
		
		$build = $build + 1
		
		$line = "[assembly: AssemblyFileVersion(""{0}.{1}.{2}.{3}"")]" -f $major, $minor, $build, $revision
		
		Write-Host([string]::Format("Version number updated to ""{0}.{1}.{2}.{3}""", $major, $minor, $build, $revision))
		
		$update = $true
	}
	$replace += [Array]$line
}

### update only if pattern found
if($update -eq $true)
{
	$replace | Out-File $file -Encoding UTF8 -force
}
