Here is one example to Release an ASP.NET Core 1.1 web application to an App Service (Windows). You could adapt it with your own context, needs and constraints.

2 ways to create the associated Release Definition + another one just to deploy the Azure services:

- [Import the Release Definition](#import-the-release-definition)
- [Create manually the Release Definition](#create-manually-the-release-definition)
- [Deploy to Azure buttons](#deploy-to-azure-buttons)

# Import the Release Definition

You could import [the associated Release Definition stored in this repository](/vsts/AspDotNetCore-AppServiceWindows-CD.json) and then follow these steps to adapt it to your current project, credentials, etc.:

TODO

# Create manually the Release Definition

## Overview
![Release Overview](/docs/imgs/AspDotNetCore-AppServiceWindows-CD.PNG)

## Staging Environment
![Staging Release Overview](/docs/imgs/AspDotNetCore-AppServiceWindows-CD-Staging.PNG)

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
- Ensure Production Web App exists
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceWindows-CI/infra/[deploy-windows.json](../infra/templates/deploy-windows.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -appServicePlanName $(ResourceGroupName)
  - Deployment Mode = Incremental
- Provision Staging
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceWindows-CI/infra/[WebAppSlot.json](../infra/templates/WebAppSlot.json)
  - Override Template Parameters = -webAppName $(ResourceGroupName) -slotName $(SlotName)
  - Deployment Mode = Incremental
- Copy database from Production
  - Type = Azure PowerShell
  - Version = 0.*
  - Azure Connection Type = Azure Resource Manager
  - Azure Subscription = set appropriate
  - Script Type = Script File Path
  - Script Path = TODO
  - Script Arguments = TODO
- Deploy Web App
  - Type = Azure App Service Deploy
  - Version = 3.*
  - Azure Subscription = set appropriate
  - App Service Name = $(ResourceGroupName)
  - Deploy to Slot = true
  - Resource Group = $(ResourceGroupName)
  - Slot = $(SlotName)
  - Package or Folder = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceWindows-CI/infra/AspNetCoreApplication.zip
  - Publish using Web Deploy = true
  - Take App Offline = true
- Run UITests
  - Type = Visual Studio Test
  - Version = 2.*
  - Select tests using = Test assemblies
  - Test assemblies = *UITests.dll
  - Search folder = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceWindows-CI/ui-tests
  - Test filter criteria = TestCategory=UITests
  - Select test platform using = Version
  - Test paltform version = Latest
  - Settings file = $(System.DefaultWorkingDirectory)/ApsNetCore-AppServiceWindows-CI/ui-tests/TestRunParameters.runsettings
  - Override test run parameters = -BaseUrl https://$(ResourceGroupName)-$(SlotName).azurewebsites.net -Browser PhantomJS
  - Test run title = UITests
  - Build Platform = $(ReleasePlatform)
  - Build Configuration = $(ReleaseConfiguration)
  - Upload test attachments = true
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
![Production Release Overview](/docs/imgs/AspDotNetCore-AppServiceWindows-CD-Production.PNG)

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

- Stop Staging
  - Type = Azure App Service Manage (PREVIEW)
  - Version = 0.*
  - Azure Subscription = set appropriate
  - Action = Stop App Service
  - App Service Name = $(ResourceGroupName)
  - Specify Slot = true
  - Resource Group = $(ResourceGroupName)
  - Slot = $(SlotToSwap)
- Copy database from Production (Backup plan)
  - Type = Azure PowerShell
  - Version = 0.*
  - Azure Connection Type = Azure Resource Manager
  - Azure Subscription = set appropriate
  - Script Type = Script File Path
  - Script Path = TODO
  - Script Arguments = TODO
- Swap Staging to Production
  - Type = Azure App Service Manage (PREVIEW)
  - Version = 0.*
  - Azure Subscription = set appropriate
  - Action = Swap Slots
  - App Service Name = $(ResourceGroupName)
  - Resource Group = $(ResourceGroupName)
  - Source Slot = $(SlotToSwap)
  - Swap with Production = true
- Check Production URL
  - Type = [Check URL Status](https://marketplace.visualstudio.com/items?itemName=saeidbabaei.checkUrl)
  - URL = https://$(ResourceGroupName).azurewebsites.net
- Start Staging
  - Type = Azure App Service Manage (PREVIEW)
  - Version = 0.*
  - Azure Subscription = set appropriate
  - Action = Start App Service
  - App Service Name = $(ResourceGroupName)
  - Specify Slot = true
  - Resource Group = $(ResourceGroupName)
  - Slot = $(SlotToSwap)
- Set Resource Group Lock
  - Type = Azure PowerShell
  - Version = 1.*
  - Azure Connection Type = Azure Resource Manager
  - Azure Subscription = set appropriate
  - Script Type = Script File Path
  - Script Path = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceWindows-CI/scripts/[AddResourceGroupLock.ps1](../infra/scripts/AddResourceGroupLock.ps1)
  - Script Arguments = $(ResourceGroupName)

### General remark

  For the "Set Resource Group Lock" step, you will need to make sure that your default Service Principal user created by VSTS (during the Azure RM service endpoint creation) has the Owner role and not by default the Contributor role. Otherwise this task will fail. To assign the Owner role, you could go to the Access control (IAM) blade of your Azure subscription within the new Azure portal and then Assign (Add button) the associated VisualStudioSPN... user to the Owner role.

## Rollback Environment

This environment should be used just if necessary when the bad things happened in Production just after this release pipeline. It allows to be prepared by automation to rollback the changed and get back to the previous version.

![Rollback Release Overview](/docs/imgs/AspDotNetCore-AppServiceWindows-CD-Rollback.PNG)

### Pre-deploymnet conditions

- Triggers
  - Select the source of the trigger = Environment
  - Environment(s) that will trigger a deployment = Production
- Pre-deployment approvers
  - Approval type = Specific users
  - Select approvers = set appropriate

### Tasks

- Rollback Swap
  - Type = Azure App Service Manage (PREVIEW)
  - Version = 0.*
  - Azure Subscription = set appropriate
  - Action = Swap Slots
  - App Service Name = $(ResourceGroupName)
  - Resource Group = $(ResourceGroupName)
  - Source Slot = $(SlotToSwap)
  - Swap with Production = true
- Check Production URL
  - Type = [Check URL Status](https://marketplace.visualstudio.com/items?itemName=saeidbabaei.checkUrl)
  - URL = https://$(ResourceGroupName).azurewebsites.net

# Deploy to Azure buttons

By using the buttons below it's another way to deploy the Azure services without VSTS and without taking into account the web app, just deploying the infrastructure within Azure.

Deploy the Azure App Service (Windows) and its associated services within the Azure portal:

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmathieu-benoit%2Fasp-dot-net-core-on-azure-web-app%2Fmaster%2Finfra%2Fdeploy-windows.json" target="_blank">![Deploy to Azure](http://azuredeploy.net/deploybutton.png)</a>