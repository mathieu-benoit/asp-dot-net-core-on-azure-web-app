Param(
    [string] [Parameter(Mandatory=$true)] $ResourceGroupLocation,
    [string] [Parameter(Mandatory=$true)] $ResourceGroupName,
    [string] $TemplateFile = 'CreateAzureWebAppWithAssociatedPlan.json'
)

Login-AzureRmAccount
#Select-AzureSubscription "MySubscription"
New-AzureRmResourceGroup -Name $ResourceGroupName -Location $ResourceGroupLocation
New-AzureRmResourceGroupDeployment -Name $ResourceGroupName `
                                       -ResourceGroupName $ResourceGroupName `
                                       -TemplateFile $TemplateFile