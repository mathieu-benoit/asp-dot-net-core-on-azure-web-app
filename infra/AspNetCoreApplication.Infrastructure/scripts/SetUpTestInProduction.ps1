#FYI, this script doesn't work for now. I got this error:
#Cannot find type [Microsoft.WindowsAzure.Commands.Utilities.Websites.Services.WebEntities.RampUpRule]: verify that the assembly containing this type is loaded.

Param(
    [string] [Parameter(Mandatory=$true)] $SiteName,
    [string] [Parameter(Mandatory=$true)] $SlotName,
    [ValidateRange(0,100)] [Int] [Parameter(Mandatory=$true)] $ReroutePercentage
)

if($ReroutePercentage -eq 0)
{
    Set-AzureWebsite $siteName -Slot Production -RoutingRules @()
}
else
{
    $rule = New-Object Microsoft.WindowsAzure.Commands.Utilities.Websites.Services.WebEntities.RampUpRule
    $rule.Name = "$SlotName"
    $rule.ActionHostName = "$SiteName-$SlotName.azurewebsites.net"
    $rule.ReroutePercentage = $ReroutePercentage
    
    #JFYI, other feature which could used:
    #$rule.ChangeIntervalInMinutes = 10;
    #$rule.ChangeStep = 5;
    #$rule.MinReroutePercentage = 5;
    #$rule.MaxReroutePercentage = 50;

    Set-AzureWebsite $SiteName -Slot Production -RoutingRules $rule  
}