#FYI, this script doesn't work for now.

Param(
    [string] [Parameter(Mandatory=$true)] $SiteName,
    [string] [Parameter(Mandatory=$true)] $SlotName,
    [ValidateRange(0,100)] [Int] [Parameter(Mandatory=$true)] $ReroutePercentage
)

$rule = New-Object -TypeName 'Microsoft.WindowsAzure.Commands.Utilities.Websites.Services.WebEntities.RampUpRule'
$rule.Name = $SlotName
$rule.ActionHostName = "$SiteName-$SlotName.azurewebsites.net"
$rule.ReroutePercentage = $ReroutePercentage
Set-AzureWebsite -RoutingRules $rule -Name $SiteName -Slot Production