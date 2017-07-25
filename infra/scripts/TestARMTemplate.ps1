#Powershell script to use locally as example. Let's adapt it for your own needs.

Param(
    [string] $ResourceGroupLocation = 'East US',
    [string] $TemplateFile = '..\templates\deploy.json'
)

$temporaryResourceGroupName = "tmp-rg-for-templates-validation"

#Login-AzureRmAccount;
#Select-AzureRmSubscription -SubscriptionId $SubscriptionId;
New-AzureRmResourceGroup -Name $temporaryResourceGroupName -Location $ResourceGroupLocation;
Test-AzureRmResourceGroupDeployment -ResourceGroupName $temporaryResourceGroupName `
                                       -TemplateFile $TemplateFile `
                                       -Verbose;
Remove-AzureRmResourceGroup -Name $temporaryResourceGroupName;
