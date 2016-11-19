Here is one example to Release this Web App via VSTS. You could adapt it with your own context, needs and constraints.

#Staging Environment
![Staging Release Overview](/docs/StagingRelease.PNG)

##Deployment options
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

#Preview Environment
![Preview Release Overview](/docs/PreviewRelease.PNG)

##Variables
- TODO

##Steps 
- TODO

#Live Environment
![Live Release Overview](/docs/LiveRelease.PNG)

##Variables
- TODO

##Steps 
- TODO

#Delete Environment
![Delete Overview](/docs/DeleteRelease.PNG)

##Variables
- TODO

##Steps 
- TODO