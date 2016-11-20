Here is one example to Release this Web App via VSTS. You could adapt it with your own context, needs and constraints.

#Staging Environment
![Staging Release Overview](/docs/StagingRelease.PNG)

##Deployment conditions
- Trigger = After release creation

##Approvals
- Pre-deployment approver = Automatic
- Post-deployment approver = Automatic

##Variables
- AdministratorLogin = set appropriate
- AdministratorLoginPassword = set appropriate
- ResourceGroupName = set appropriate
- SlotName = staging
- Location = East US

##Steps 
- (Ensure) App Service Plan
  - Type = Azure Resource Group Deployment
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/arm-templates/[AppServicePlan.json](../release/ManageAzureWebAppAzureResourceGroup/templates/AppServicePlan.json)
  - Override Template Parameters = -appServicePlanName $(ResourceGroupName)
  - Deployment Mode = Incremental
- (Ensure) Web App
  - Type = Azure Resource Group Deployment
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/arm-templates/[WebApp.json](../release/ManageAzureWebAppAzureResourceGroup/templates/WebApp.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -appServicePlanName $(ResourceGroupName)
  - Deployment Mode = Incremental
- Slot
  - Type = Azure Resource Group Deployment
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/arm-templates/[WebAppSlot.json](../release/ManageAzureWebAppAzureResourceGroup/templates/WebAppSlot.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -slotName $(SlotName)
  - Deployment Mode = Incremental
- App Insights
  - Type = Azure Resource Group Deployment
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/arm-templates/[ApplicationInsights.json](../release/ManageAzureWebAppAzureResourceGroup/templates/ApplicationInsights.json)
  - Override Template Parameters = -appInsightsName $(ResourceGroupName)-$(SlotName)
  - Deployment Mode = Incremental
- Sql Database
  - Type = Azure Resource Group Deployment
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/arm-templates/[SqlDatabase.json](../release/ManageAzureWebAppAzureResourceGroup/templates/SqlDatabase.json)
  - Override Template Parameters = -databaseName $(ResourceGroupName)-$(SlotName) -adminLogin $(AdministratorLogin) -adminLoginPassword (ConvertTo-SecureString -String '$(AdministratorLoginPassword)' -AsPlainText -Force)
  - Deployment Mode = Incremental
- Slot App Settings
  - Type = Azure Resource Group Deployment
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/arm-templates/[WebAppSlotSettings.json](../release/ManageAzureWebAppAzureResourceGroup/templates/WebAppSlotSettings.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -slotName $(SlotName) -adminLogin $(AdministratorLogin) -adminLoginPassword (ConvertTo-SecureString -String '$(AdministratorLoginPassword)' -AsPlainText -Force)
  - Deployment Mode = Incremental
- Deploy Web App
  - Type = Azure App Service Deploy
  - AzureRM Subscription = set appropriate
  - App Service Name = $(ResourceGroupName)
  - Deploy to Slot = true
  - Resource Group = $(ResourceGroupName)
  - Slot = $(SlotName)
  - Package or Folder = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/web-app/AspNetCoreApplication.zip
  - Publish using Web Deploy = true
  - Take App Offline = true
- Quick Web Performance Test Load
  - Type = Cloud-based Web Performance Test
  - VS Team Services Connection = set appropriate
  - Website Url = http://$(ResourceGroupName)-$(SlotName).azurewebsites.net/
  - Test name = Load Tests on $(ResourceGroupName)-$(SlotName) Homepage
  - User Load = 25
  - Run Duration (sec) = 60
  - Location = Default
  - Run load test using = Automatically provisioned agents 

#Preview Environment
![Preview Release Overview](/docs/PreviewRelease.PNG)

##Deployment conditions
- Trigger = After successful deployment to another environment ("Staging")

##Approvals
- Pre-deployment approver = Specific Users (set appropriate users)
- Post-deployment approver = Automatic

##Variables
- AdministratorLogin = set appropriate
- AdministratorLoginPassword = set appropriate
- ResourceGroupName = set appropriate
- SlotName = preview
- Location = East US

##Steps 
- Same than the "Staging Environment" but replace the last one: "Quick Web Performance Test Load" step by the one following:
- Add Test In Production - disabled for now because the script doesn't work.
  - Type = Azure Powershell
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Script Path = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/scripts/[SetUpTestInProduction.ps1](../release/ManageAzureWebAppAzureResourceGroup/scripts/SetUpTestInProduction.ps1)
  - Script Arguments = $(ResourceGroupName) $(SlotName) 70

#Production Environment
![Production Release Overview](/docs/ProductionRelease.PNG)

##Deployment conditions
- Trigger = After successful deployment to another environment ("Staging")

##Approvals
- Pre-deployment approver = Specific Users (set appropriate users)
- Post-deployment approver = Automatic

##Variables
- AdministratorLogin = set appropriate
- AdministratorLoginPassword = set appropriate
- ResourceGroupName = set appropriate
- SlotToSwap = staging
- Location = East US

##Steps
- (Ensure) App Service Plan
  - Type = Azure Resource Group Deployment
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/arm-templates/[AppServicePlan.json](../release/ManageAzureWebAppAzureResourceGroup/templates/AppServicePlan.json)
  - Override Template Parameters = -appServicePlanName $(ResourceGroupName)
  - Deployment Mode = Incremental
- Web App
  - Type = Azure Resource Group Deployment
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/arm-templates/[WebApp.json](../release/ManageAzureWebAppAzureResourceGroup/templates/WebApp.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -appServicePlanName $(ResourceGroupName)
  - Deployment Mode = Incremental
- App Insights
  - Type = Azure Resource Group Deployment
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/arm-templates/[ApplicationInsights.json](../release/ManageAzureWebAppAzureResourceGroup/templates/ApplicationInsights.json)
  - Override Template Parameters = -appInsightsName $(ResourceGroupName)
  - Deployment Mode = Incremental
- Sql Database
  - Type = Azure Resource Group Deployment
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/arm-templates/[SqlDatabase.json](../release/ManageAzureWebAppAzureResourceGroup/templates/SqlDatabase.json)
  - Override Template Parameters = -databaseName $(ResourceGroupName) -adminLogin $(AdministratorLogin) -adminLoginPassword (ConvertTo-SecureString -String '$(AdministratorLoginPassword)' -AsPlainText -Force)
  - Deployment Mode = Incremental
- App Settings
  - Type = Azure Resource Group Deployment
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/arm-templates/[WebAppSettings.json](../release/ManageAzureWebAppAzureResourceGroup/templates/WebAppSettings.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName)  -adminLogin $(AdministratorLogin) -adminLoginPassword (ConvertTo-SecureString -String '$(AdministratorLoginPassword)' -AsPlainText -Force)
  - Deployment Mode = Incremental
- Remove Test In Production - disabled for now because the script doesn't work.
  - Type = Azure Powershell
  - Azure Connection Type = Azure Resource Manager
  - Azure RM Subscription = set appropriate
  - Script Path = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/scripts/[SetUpTestInProduction.ps1](../release/ManageAzureWebAppAzureResourceGroup/scripts/SetUpTestInProduction.ps1)
  - Script Arguments = $(ResourceGroupName) $(SlotToSwap) 0
- Swap Staging to Production
  - Type = Azure App Service Manage (PREVIEW)
  - AzureRM Subscription = set appropriate
  - App Service Name = $(ResourceGroupName)
  - Resource Group = $(ResourceGroupName)
  - Source Slot = $(SlotToSwap)
  - Swap with Production = true
- Set Resource Group Lock
  - Type = Azure PowerShell
  - Azure Connection Type = set appropriate
  - Azure RM Subscription = set appropriate
  - Script Path = $(System.DefaultWorkingDirectory)/Build AspNetCoreApplication/scripts/[AddResourceGroupLock.ps1](../release/ManageAzureWebAppAzureResourceGroup/scripts/AddResourceGroupLock.ps1)
  - Script Arguments = $(ResourceGroupName)
