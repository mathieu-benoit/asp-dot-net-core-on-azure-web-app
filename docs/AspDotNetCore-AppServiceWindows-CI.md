Here is one example to Build an ASP.NET Core 1.1 web application to an App Service (Windows) via VSTS. You could adapt it with your own context, needs and constraints.

![Build Overview](/docs/imgs/AspDotNetCore-AppServiceWindows-CI.PNG)

# Import the Build Definition

You could import [the associated Build Definition stored in this repository](/vsts/ApsNetCore-AppServiceWindows-CI.json) and then follow these steps to adapt it to your current project, credentials, etc.:

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
- Name = ApsNetCore-AppServiceWindows-CI
- Default agent queue = Hosted VS2017

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
- Validate ARM Templates
  - Azure subscription = set appropriate
  - Action = Create or update resource group
  - Resource group = test
  - Location = East US
  - Template location = Linked artifact
  - Template = infra/AspNetCoreApplication.Infrastructure/templates/deploy-windows.json
  - Override template parameters = -appServicePlanName test -webAppName test
  - Deployment mode = Validation only
- Publish Artifact: web
  - Type = Publish Build Artifacts
  - Path to publish = $(build.artifactstagingdirectory)
  - Artifact Name = web
  - Artifact Type = Server
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