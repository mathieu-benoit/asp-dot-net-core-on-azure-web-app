Here is one example to Build an ASP.NET Core 1.1 web application to an App Service (Linux) via VSTS. You could adapt it with your own context, needs and constraints.

![Build Overview](/docs/imgs/ApsNetCore-AppServiceLinux-CI.PNG)

# Prerequisities - Create manually an Azure Container Registry

Because [we cannot dynamicaly incorporate the Container Registry name within the associated VSTS task](https://blogs.msdn.microsoft.com/devops/2017/06/09/deploying-applications-to-azure-container-service/#comment-90545), currently the only way to use it is to create and configure manually the Azure Container Registry service. Otherwise, and when it will be available, the goal will be to add in the Build/CI Definition the CreateOrUpdate VSTS task to manage the Container Resitry ARM Templates.

I would suggest to do that via [Azure CLI 2.0](https://aka.ms/azcli-docs) by using the [Azure Cloud Shell](https://azure.microsoft.com/en-us/features/cloud-shell/) with the following steps:

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

# Import the Build Definition

You could import [the associated Build Definition stored in this repository](/vsts/ApsNetCore-AppServiceLinuxs-CI.json) and then follow these steps to adapt it to your current project, credentials, etc.:

TODO

# Create manually the Build Definition

## Variables
- BuildConfiguration = release
- DOTNET_SKIP_FIRST_TIME_EXPERIENCE = true

## Repository
- Repository Type = GitHub
- Connection = set appropriate
- Repository = mathieu-benoit/asp-dot-net-core-on-azure-web-app
- Default branch = master

## Triggers
- Continuous Integration (CI) = true

## Process - Build process
- Name = ApsNetCore-AppServiceLinux-CI
- Default agent queue = Hosted Linux Preview

## Steps 
- Restore
  - Type = .NET Core
  - Command = restore
  - Project(s) = **/*.csproj
- Build
  - Type = .NET Core
  - Command = build
  - Project(s) = src/AspNetCoreApplication/AspNetCoreApplication.csproj\ntest/AspNetCoreApplication.UnitTests/AspNetCoreApplication.UnitTests.csproj
  - Arguments = --configuration $(BuildConfiguration)
- UnitTests
  - Type = .NET Core
  - Command = test
  - Project(s) = **/*UnitTests/*.csproj
  - Arguments = --configuration $(BuildConfiguration) -xml TEST-TestResults.xml
- Publish Test Results
  - Type = Publish Test Results
  - Test Result Format = XUnit
  - Test Results Files = **/TEST-*.xml
- Publish Web App
  - Type = .NET Core
  - Command = publish
  - Publish Web Projects = true
  - Arguments = --configuration $(BuildConfiguration) --output $(build.artifactstagingdirectory)
  - Zip Published Projects = true
- Build Docker image
  - Type = Docker (PREVIEW)
  - Container Registry Type = Azure Container Registry
  - Azure subscription = set appropriate
  - Azure Container Registry = set appropriate
  - Action = Build an image
  - Docker File = **/[Dockerfile](../../src/AspNetCoreApplication/Dockerfile)
  - Image Name = $(Build.Repository.Name):$(Build.BuildId)
  - Qualify Image Name = true
  - Additional Image Tags = $(Build.BuildId)
- Push an image
  - Type = Docker (PREVIEW)
  - Container Registry Type = Azure Container Registry
  - Azure subscription = set appropriate
  - Azure Container Registry = set appropriate
  - Action = Push an image
  - Image Name = $(Build.Repository.Name):$(Build.BuildId)
  - Qualify Image Name = true
  - Additional Image Tags = $(Build.BuildId)
- Validate ARM Templates
  - Azure subscription = set appropriate
  - Action = Create or update resource group
  - Resource group = test
  - Location = West US
  - Template location = Linked artifact
  - Template = infra/AspNetCoreApplication.Infrastructure/templates/deploy-linux.json
  - Override template parameters = -appServicePlanName test -webAppName test -registryName test -dockerImageName test
  - Deployment mode = Validation only
- Publish Artifact: infra
  - Type = Publish Build Artifacts
  - Path to publish = infra/AspNetCoreApplication.Infrastructure/templates
  - Artifact Name = infra
  - Artifact Type = Server
- Publish Artifact: scripts
  - Type = Publish Build Artifacts
  - Copy Root = infra/AspNetCoreApplication.Infrastructure/scripts
  - Artifact Name = scripts
  - Artifact Type = Server