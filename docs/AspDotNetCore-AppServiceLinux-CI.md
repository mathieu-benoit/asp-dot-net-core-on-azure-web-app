Here is one example to Build an ASP.NET Core 2.0 web application to an App Service (Linux) via VSTS. You could adapt it with your own context, needs and constraints.

For now, because we canno't yet reference the Azure Container Registry via a variable within the VSTS Docker task, [you will need to create manually the Azure Container Registry like explained here](#prerequisities-create-mmanually-an-azure-container-registry).

2 ways to create the associated Build Definition:

- [Import the Build Definition](#import-the-build-definition)
- [Create manually the Build Definition](#create-manually-the-build-definition)

![Build Overview](/docs/imgs/AspDotNetCore-AppServiceLinux-CI.PNG)

# Prerequisities - Create manually an Azure Container Registry

Because [we cannot dynamicaly incorporate the Container Registry name within the associated VSTS task](https://blogs.msdn.microsoft.com/devops/2017/06/09/deploying-applications-to-azure-container-service/#comment-90545), currently the only way to use it is to create and configure manually the Azure Container Registry service prior defining the VSTS Build definition. Otherwise, and when it will be available, the goal will be to add in the Build/CI Definition the CreateOrUpdate VSTS task to manage the Container Resitry ARM Templates.

I would suggest 2 ways to accomplish that:
- Via Azure CLI 2.0 by using the Azure Cloud Shell
- Via a "Deploy to Azure" button

## Create an Azure Container Service via Azure CLI 2.0 by using the Azure Cloud Shell

Create an Azure Container Service via [Azure CLI 2.0](https://aka.ms/azcli-docs) by using the [Azure Cloud Shell](https://azure.microsoft.com/en-us/features/cloud-shell/) with the following steps:

- Go to [https://portal.azure.com](https://portal.azure.com)
- Launch [Cloud Shell](https://docs.microsoft.com/en-us/azure/cloud-shell/quickstart) from the top navigation of the Azure portal
- List the subsriptions you have access to:
```
az account list
```
- Set your preferred subscription:
```
az account set --subscription my-subscription-name
```
- Create a resource group:
```
az group create -l westus -n MyRG
```
- Create a Container Registry:
```
az acr create --admin-enabled --sku Basic --verbose -l westus -n demoregfoo123 -g MyRG 
```

## Create an Azure Container Service via a "Deploy to Azure" button

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmathieu-benoit%2Fasp-dot-net-core-on-azure-web-app%2Fmaster%2Finfra%2Ftemplates%2FContainerRegistry.json" target="_blank">![Deploy to Azure](http://azuredeploy.net/deploybutton.png)</a>

# Import the Build Definition

You could import [the associated Build Definition stored in this repository](/vsts/AspDotNetCore-AppServiceLinux-CI.json) and then follow these steps to adapt it to your current project, credentials, etc.:

TODO

# Create manually the Build Definition

## Variables
- BuildConfiguration = release
- DOTNET_SKIP_FIRST_TIME_EXPERIENCE = true
- ValidateTemplatesResourceGroup = validate-templates-rg

## Repository
- Repository Type = GitHub
- Connection = set appropriate
- Repository = mathieu-benoit/asp-dot-net-core-on-azure-web-app
- Default branch = master

## Triggers
- Continuous Integration (CI) = true

## Process - Build process
- Name = AspDotNetCore-AppServiceLinux-CI
- Default agent queue = Hosted Linux Preview

## Steps 
- Restore
  - Type = .NET Core
  - Version = 1.*
  - Command = restore
  - Project(s) = **/*.csproj
- Build
  - Type = .NET Core
  - Version = 1.*
  - Command = build
  - Project(s) = src/AspNetCoreApplication/AspNetCoreApplication.csproj\ntest/AspNetCoreApplication.UnitTests/AspNetCoreApplication.UnitTests.csproj
  - Arguments = --configuration $(BuildConfiguration)
- Run UnitTests
  - Type = .NET Core
  - Version = 1.*
  - Command = test
  - Project(s) = **/*UnitTests/*.csproj
  - Arguments = --configuration $(BuildConfiguration) --logger:trx
- Publish Test Results
  - Type = Publish Test Results
  - Version = 2.*
  - Test Result Format = VSTest
  - Test Results Files = **/*.trx
  - Search folder = $(System.DefaultWorkingDirectory)
  - Merge test results = true
  - Test run title = UnitTests
  - Platform = $(BuildPlatform)
  - Configuration = $(BuildConfiguration)
  - Upload test results files = true
- Package Web App
  - Type = .NET Core
  - Version = 1.*
  - Command = publish
  - Publish Web Projects = true
  - Arguments = --configuration $(BuildConfiguration) --output pub
  - Zip Published Projects = false
- Build Docker image
  - Type = Docker (PREVIEW)
  - Version = 0.* (preview)
  - Container Registry Type = Azure Container Registry
  - Azure subscription = set appropriate
  - Azure Container Registry = set appropriate
  - Action = Build an image
  - Docker File = **/[Dockerfile](../../src/AspNetCoreApplication/Dockerfile)
  - Build Arguments = source=pub
  - Image Name = $(Build.Repository.Name):$(Build.BuildId)
  - Qualify Image Name = true
  - Include Latest Tags = true
- Push Docker image
  - Type = Docker (PREVIEW)
  - Version = 0.* (preview)
  - Container Registry Type = Azure Container Registry
  - Azure subscription = set appropriate
  - Azure Container Registry = set appropriate
  - Action = Push an image
  - Image Name = $(Build.Repository.Name):$(Build.BuildId)
  - Qualify Image Name = true
  - Include Latest Tags = true
- Validate ARM Templates
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure subscription = set appropriate
  - Action = Create or update resource group
  - Resource group = $(ValidateTemplatesResourceGroup)
  - Location = West US
  - Template location = Linked artifact
  - Template = infra/templates/[deploy-linux.json](../infra/templates/deploy-linux.json)
  - Override template parameters = -appServicePlanName test -webAppName test -registryName test -dockerImageName test
  - Deployment mode = Validation only
- Remove temporary ValidateTemplatesResourceGroup
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure subscription = set appropriate
  - Action = Delete resource group
  - Resource group = $(ValidateTemplatesResourceGroup)
- Publish Artifact: infra
  - Type = Publish Build Artifacts
  - Version = 1.*
  - Path to publish = infra/templates
  - Artifact Name = infra
  - Artifact Type = Server
- Publish Artifact: scripts
  - Type = Publish Build Artifacts
  - Version = 1.*
  - Path to publish = infra/scripts
  - Artifact Name = scripts
  - Artifact Type = Server