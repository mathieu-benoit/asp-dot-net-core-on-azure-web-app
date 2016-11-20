#FYI, this script doesn't work for now.

Param(
    [string] [Parameter(Mandatory=$true)] $SiteName,
    [string] [Parameter(Mandatory=$true)] $SlotName,
    [ValidateRange(0,100)] [Int] [Parameter(Mandatory=$true)] $ReroutePercentage
)

$rule = New-Object Microsoft.WindowsAzure.Commands.Utilities.Websites.Services.WebEntities.RampUpRule
$rule.Name = "$SlotName"
$rule.ActionHostName = "$SiteName-$SlotName.azurewebsites.net"
$rule.ReroutePercentage = $ReroutePercentage
Set-AzureWebsite $SiteName -Slot Production -RoutingRules $rule  

#Set-AzureWebsite $siteName -Slot Production -RoutingRules @()