Here is one example to Release an ASP.NET Core 1.1 web application to an App Service (Linux). You could adapt it with your own context, needs and constraints.

# Import the Release Definition

You could import [the associated Release Definition stored in this repository](/vsts/AspDotNetCore-AppServiceLinux-CD.json) and then follow these steps to adapt it to your current project, credentials, etc.:

TODO

# Create manually the Release Definition

## Overview
![Release Overview](/docs/imgs/AspDotNetCore-AppServiceLinux-CD.PNG)

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
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[deploy-linux.json](../infra/templates/deploy-linux.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -appServicePlanName $(ResourceGroupName)
  - Deployment Mode = Incremental
- Slot
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[WebAppSlot.json](../infra/templates/WebAppSlot.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -slotName $(SlotName)
  - Deployment Mode = Incremental
- App Insights
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[ApplicationInsights.json](../infra/templates/ApplicationInsights.json)
  - Override Template Parameters = -appInsightsName $(ResourceGroupName)-$(SlotName)
  - Deployment Mode = Incremental
- Sql Database
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[SqlDatabase.json](../infra/templates/SqlDatabase.json)
  - Override Template Parameters = -databaseName $(ResourceGroupName)-$(SlotName) -adminLogin $(AdministratorLogin) -adminLoginPassword (ConvertTo-SecureString -String '$(AdministratorLoginPassword)' -AsPlainText -Force)
  - Deployment Mode = Incremental
- Slot App Settings
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[WebAppSlotSettings.json](../infra/templates/WebAppSlotSettings.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -slotName $(SlotName) -adminLogin $(AdministratorLogin) -adminLoginPassword (ConvertTo-SecureString -String '$(AdministratorLoginPassword)' -AsPlainText -Force)
  - Deployment Mode = Incremental
- Restart Web App
  - Type = Azure App Service Manage (PREVIEW)
  - Version = 0.*
  - Azure Subscription = set appropriate
  - Action = Restart App Service
  - App Service Name = $(ResourceGroupName)
  - Specify Slot = true
  - Resource Group = $(ResourceGroupName)
  - Slot = $(SlotName)
- Quick Web Performance Test Load
  - Type = Cloud-based Web Performance Test
  - Version = 1.*
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
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[deploy-linux.json](../infra/templates/deploy-linux.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -appServicePlanName $(ResourceGroupName)
  - Deployment Mode = Incremental
- App Insights
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[ApplicationInsights.json](../infra/templates/ApplicationInsights.json)
  - Override Template Parameters = -appInsightsName $(ResourceGroupName)
  - Deployment Mode = Incremental
- Sql Database
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[SqlDatabase.json](../infra/templates/SqlDatabase.json)
  - Override Template Parameters = -databaseName $(ResourceGroupName) -adminLogin $(AdministratorLogin) -adminLoginPassword (ConvertTo-SecureString -String '$(AdministratorLoginPassword)' -AsPlainText -Force)
  - Deployment Mode = Incremental
- App Settings
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/infra/[WebAppSettings.json](../infra/templates/WebAppSettings.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName)  -adminLogin $(AdministratorLogin) -adminLoginPassword (ConvertTo-SecureString -String '$(AdministratorLoginPassword)' -AsPlainText -Force)
  - Deployment Mode = Incremental
- Swap Staging to Production
  - Type = Azure App Service Manage (PREVIEW)
  - Version = 0.*
  - Azure Subscription = set appropriate
  - Action = Swap Slots
  - App Service Name = $(ResourceGroupName)
  - Resource Group = $(ResourceGroupName)
  - Source Slot = $(SlotToSwap)
  - Swap with Production = true
- Set Resource Group Lock
  - Type = Azure PowerShell
  - Version = 1.*
  - Azure Connection Type = set appropriate
  - Azure RM Subscription = set appropriate
  - Script Path = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceLinux-CI/scripts/[AddResourceGroupLock.ps1](../infra/scripts/AddResourceGroupLock.ps1)
  - Script Arguments = $(ResourceGroupName)

  # "Deploy to Azure" buttons

By using the buttons below it's another way to deploy the Azure services without VSTS and without taking into account the web app, just deploying the infrastructure within Azure.

Deploy the Azure App Service (Linux) and its associated services within the Azure portal:

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmathieu-benoit%2Fasp-dot-net-core-on-azure-web-app%2Fmaster%2Finfra%2FAspNetCoreApplication.Infrastructure%2Ftemplates%2Fdeploy-linux.json" target="_blank">![Deploy to Azure](http://azuredeploy.net/deploybutton.png)</a>

Deploy the Azure Container Registry and its associated Blob Storage within the Azure portal:

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmathieu-benoit%2Fasp-dot-net-core-on-azure-web-app%2Fmaster%2Finfra%2FAspNetCoreApplication.Infrastructure%2Ftemplates%2FContainerRegistry.json" target="_blank">![Deploy to Azure](http://azuredeploy.net/deploybutton.png)</a>