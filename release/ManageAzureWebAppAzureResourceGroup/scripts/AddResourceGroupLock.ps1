Param(
    [string] [Parameter(Mandatory=$true)] $ResourceGroupName,
    [string] $LockName = 'DoNotDeleteLock'
)

#Login-AzureRmAccount
#Select-AzureSubscription "MySubscription"
New-AzureRMResourceLock -LockName $LockName -LockLevel CanNotDelete -ResourceGroupName $ResourceGroupName