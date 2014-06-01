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

$content = Get-Content $file
$replace = @()
foreach($line in $content) {
	if($line -match $pattern) {
		$value = [int]$matches[3] # read the 3rd part from version string
		$value = $value + 1
		$line = "[assembly: AssemblyFileVersion(""{0}.{1}.{2}.{3}"")]" -f $matches[1], $matches[2], $value, $matches[4]
		Write-Host "successfully updated to: $value"
		$update = $true
	}
	$replace += [Array]$line
}

### update only if pattern found
if($update -eq $true)
{
	$replace | Out-File $file -Encoding UTF8 -force
}
