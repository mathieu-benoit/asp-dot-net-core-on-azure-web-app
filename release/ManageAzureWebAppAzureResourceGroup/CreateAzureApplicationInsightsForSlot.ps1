#Powershell script to use locally as example. Let's adapt it for your own needs.

Param(
    [string] $ResourceGroupLocation = 'East US',
    [string] [Parameter(Mandatory=$true)] $ResourceGroupName,
    [string] $TemplateFile = 'CreateAzureApplicationInsightsForSlot.json',
	[string] $TemplateParameterFile = 'CreateAzureApplicationInsightsForSlot.parameters.json'
)

#Login-AzureRmAccount
#Select-AzureSubscription "MySubscription"
#New-AzureRmResourceGroup -Name $ResourceGroupName -Location $ResourceGroupLocation
New-AzureRmResourceGroupDeployment -Name $ResourceGroupName `
                                       -ResourceGroupName $ResourceGroupName `
                                       -TemplateFile $TemplateFile `
									   -TemplateParameterFile $TemplateParameterFile