Here is one example to Build an ASP.NET Core 2.0 web application to an App Service (Windows) via VSTS. You could adapt it with your own context, needs and constraints.

2 ways to create the associated Build Definition:

- [Import the Build Definition](#import-the-build-definition)
- [Create manually the Build Definition](#create-manually-the-build-definition)

![Build Overview](/docs/imgs/AspDotNetCore-AppServiceWindows-CI.PNG)

# Import the Build Definition

You could import [the associated Build Definition stored in this repository](/vsts/AspDotNetCore-AppServiceWindows-CI.json) and then follow these steps to adapt it to your current project, credentials, etc.:

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
- Name = AspDotNetCore-AppServiceWindows-CI
- Default agent queue = Hosted VS2017

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
- Publish UnitTests Results
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
  - Arguments = --configuration $(BuildConfiguration) --output $(build.artifactstagingdirectory)
  - Zip Published Projects = true
- NuGet restore - UITests
  - Type = NuGet Installer
  - Version = 0.*
  - Path to solution or packages.config = test/AspNetCoreApplication.UITests/packages.config
  - Installation type = Restore
  - NuGet arguments = -PackagesDirectory $(System.DefaultWorkingDirectory)/packages
  - NuGet Version (Advanced) = 4.0.0
- Build UITests
  - Type = MSBuild
  - Version 1.*
  - Project = test/AspNetCoreApplication.UITests/AspNetCoreApplication.UITests.csproj
  - MSBuild = Version
  - MSBuild Version = Latest
  - MSBuild Architecture = MSBuild x86
  - Configuration = $(BuildConfiguration)
- Validate ARM Templates
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure subscription = set appropriate
  - Action = Create or update resource group
  - Resource group = $(ValidateTemplatesResourceGroup)
  - Location = East US
  - Template location = Linked artifact
  - Template = infra/templates/[deploy-windows.json](../infra/templates/deploy-windows.json)
  - Override template parameters = -appServicePlanName test -webAppName test
  - Deployment mode = Validation only
- Remove temporary ValidateTemplatesResourceGroup
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure subscription = set appropriate
  - Action = Delete resource group
  - Resource group = $(ValidateTemplatesResourceGroup)
- Publish Artifact: web-app
  - Type = Publish Build Artifacts
  - Version = 1.*
  - Path to publish = $(build.artifactstagingdirectory)
  - Artifact Name = web
  - Artifact Type = Server
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
- Copy Files: ui-tests
  - Type = Copy Files
  - Version = 2.*
  - Source Folder = $(build.sourcesdirectory)/test/AspNetCoreApplication.UITests/bin/$(BuildConfiguration)
  - Content = **
  - Target Folder = $(build.artifactstagingdirectory)/ui-tests
- Publish Artifact: ui-tests
  - Type = Publish Build Artifacts
  - Version = 1.*
  - Path to Publish = $(build.artifactstagingdirectory)/ui-tests
  - Artifact Name = ui-tests
  - Artifact Type = Server