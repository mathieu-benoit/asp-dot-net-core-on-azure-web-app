#Powershell script to copy one Azure Sql Database to another.
#Inspired by https://docs.microsoft.com/en-us/azure/sql-database/sql-database-copy#copy-a-database-by-using-powershell

Param(
    [string][Parameter(Mandatory=$true)] $ResourceGroupName,
    [string] $ServerName = $ResourceGroupName,
    [string] $DatabaseName = $ResourceGroupName,
    [string][Parameter(Mandatory=$true)] $CopyResourceGroupName,
    [string] $CopyServerName = $CopyResourceGroupName,
    [string] $CopyDatabaseName = $CopyResourceGroupName
)

#Login-AzureRmAccount;
#Select-AzureRmSubscription -SubscriptionId $SubscriptionId;
New-AzureRmSqlDatabaseCopy -ResourceGroupName $ResourceGroupName `
    -ServerName $ServerName `
    -DatabaseName $DatabaseName `
    -CopyResourceGroupName $CopyResourceGroupName `
    -CopyServerName $CopyServerName `
    -CopyDatabaseName $CopyDatabaseName;
