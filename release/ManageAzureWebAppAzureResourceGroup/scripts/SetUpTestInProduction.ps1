Param(
    [string] [Parameter(Mandatory=$true)] $SiteName,
    [string] [Parameter(Mandatory=$true)] $SlotName,
    [ValidateRange(0,100)] [Int] [Parameter(Mandatory=$true)] $ReroutePercentage
)

Login-AzureRmAccount
#Select-AzureSubscription -Default

$rulesList = New-Object -TypeName 'System.Collections.Generic.List[Microsoft.WindowsAzure.Commands.Utilities.Websites.Services.WebEntities.RampUpRule]'
$rule = New-Object -TypeName 'Microsoft.WindowsAzure.Commands.Utilities.Websites.Services.WebEntities.RampUpRule'
$rule.Name = $SlotName
$rule.ActionHostName = "$SiteName-$SlotName.azurewebsites.net"
$rule.ReroutePercentage = $ReroutePercentage
$rulesList.Add($rule)
Set-AzureWebsite -RoutingRules $rulesList -Name $SiteName -Slot $SlotName