Here is one example to Release an ASP.NET Core 1.1 web application to an App Service (Linux). You could adapt it with your own context, needs and constraints.

# Import the Release Definition

You could import [the associated Release Definition stored in this repository](/vsts/AspDotNetCore-AppServiceLinux-CD.json) and then follow these steps to adapt it to your current project, credentials, etc.:

TODO

# Create manually the Release Definition

## Staging Environment
![Staging Release Overview](/docs/imgs/AspDotNetCore-AppServiceLinux-CD-Staging.PNG)

### Deployment conditions
- Trigger = After release creation

### Approvals
- Pre-deployment approver = Automatic
- Post-deployment approver = Automatic

### Variables
- AdministratorLogin = set appropriate
- AdministratorLoginPassword = set appropriate
- ResourceGroupName = set appropriate
- SlotName = staging
- Location = East US

### Steps 
- (Ensure) App Service Plan and Web App
  - Type = Azure Resource Group Deployment
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[deploy-linux.json](../infra/ManageAzureWebAppAzureResourceGroup/templates/deploy-linux.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -appServicePlanName $(ResourceGroupName)
  - Deployment Mode = Incremental
- Slot
  - Type = Azure Resource Group Deployment
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[WebAppSlot.json](../infra/ManageAzureWebAppAzureResourceGroup/templates/WebAppSlot.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -slotName $(SlotName)
  - Deployment Mode = Incremental
- App Insights
  - Type = Azure Resource Group Deployment
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[ApplicationInsights.json](../infra/ManageAzureWebAppAzureResourceGroup/templates/ApplicationInsights.json)
  - Override Template Parameters = -appInsightsName $(ResourceGroupName)-$(SlotName)
  - Deployment Mode = Incremental
- Sql Database
  - Type = Azure Resource Group Deployment
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[SqlDatabase.json](../infra/ManageAzureWebAppAzureResourceGroup/templates/SqlDatabase.json)
  - Override Template Parameters = -databaseName $(ResourceGroupName)-$(SlotName) -adminLogin $(AdministratorLogin) -adminLoginPassword (ConvertTo-SecureString -String '$(AdministratorLoginPassword)' -AsPlainText -Force)
  - Deployment Mode = Incremental
- Slot App Settings
  - Type = Azure Resource Group Deployment
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[WebAppSlotSettings.json](../infra/ManageAzureWebAppAzureResourceGroup/templates/WebAppSlotSettings.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -slotName $(SlotName) -adminLogin $(AdministratorLogin) -adminLoginPassword (ConvertTo-SecureString -String '$(AdministratorLoginPassword)' -AsPlainText -Force)
  - Deployment Mode = Incremental
- Restart Web App
  - Type = Azure App Service Manage (PREVIEW)
  - Azure Subscription = set appropriate
  - Action = Restart App Service
  - App Service Name = $(ResourceGroupName)
  - Specify Slot = true
  - Resource Group = $(ResourceGroupName)
  - Slot = $(SlotName)
- Quick Web Performance Test Load
  - Type = Cloud-based Web Performance Test
  - VS Team Services Connection = set appropriate
  - Website Url = http://$(ResourceGroupName)-$(SlotName).azurewebsites.net/
  - Test name = Load Tests on $(ResourceGroupName)-$(SlotName) Homepage
  - User Load = 25
  - Run Duration (sec) = 60
  - Location = Default
  - Run load test using = Automatically provisioned agents 

## Production Environment
![Production Release Overview](/docs/imgs/AspDotNetCore-AppServiceLinux-CD-Production.PNG)

### Deployment conditions
- Trigger = After successful deployment to another environment ("Staging")

### Approvals
- Pre-deployment approver = Specific Users (set appropriate users)
- Post-deployment approver = Automatic

### Variables
- AdministratorLogin = set appropriate
- AdministratorLoginPassword = set appropriate
- ResourceGroupName = set appropriate
- SlotToSwap = staging
- Location = East US

### Steps
- (Ensure) App Service Plan and Web App
  - Type = Azure Resource Group Deployment
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[deploy-linux.json](../infra/ManageAzureWebAppAzureResourceGroup/templates/deploy-linux.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -appServicePlanName $(ResourceGroupName)
  - Deployment Mode = Incremental
- App Insights
  - Type = Azure Resource Group Deployment
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[ApplicationInsights.json](../infra/ManageAzureWebAppAzureResourceGroup/templates/ApplicationInsights.json)
  - Override Template Parameters = -appInsightsName $(ResourceGroupName)
  - Deployment Mode = Incremental
- Sql Database
  - Type = Azure Resource Group Deployment
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[SqlDatabase.json](../infra/ManageAzureWebAppAzureResourceGroup/templates/SqlDatabase.json)
  - Override Template Parameters = -databaseName $(ResourceGroupName) -adminLogin $(AdministratorLogin) -adminLoginPassword (ConvertTo-SecureString -String '$(AdministratorLoginPassword)' -AsPlainText -Force)
  - Deployment Mode = Incremental
- App Settings
  - Type = Azure Resource Group Deployment
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[WebAppSettings.json](../infra/ManageAzureWebAppAzureResourceGroup/templates/WebAppSettings.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName)  -adminLogin $(AdministratorLogin) -adminLoginPassword (ConvertTo-SecureString -String '$(AdministratorLoginPassword)' -AsPlainText -Force)
  - Deployment Mode = Incremental
- Swap Staging to Production
  - Type = Azure App Service Manage (PREVIEW)
  - Azure Subscription = set appropriate
  - Action = Swap Slots
  - App Service Name = $(ResourceGroupName)
  - Resource Group = $(ResourceGroupName)
  - Source Slot = $(SlotToSwap)
  - Swap with Production = true
- Set Resource Group Lock
  - Type = Azure PowerShell
  - Azure Connection Type = set appropriate
  - Azure RM Subscription = set appropriate
  - Script Path = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/scripts/[AddResourceGroupLock.ps1](../infra/ManageAzureWebAppAzureResourceGroup/scripts/AddResourceGroupLock.ps1)
  - Script Arguments = $(ResourceGroupName)