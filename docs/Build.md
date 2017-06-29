Here is one example to Build this Web App via VSTS. You could adapt it with your own context, needs and constraints.

![Build Overview](/docs/Build.PNG)

# Variables
- BuildConfiguration = release
- DOTNET_SKIP_FIRST_TIME_EXPERIENCE = true

# Repository
- Repository Type = GitHub
- Connection = set appropriate
- Repository = mathieu-benoit/asp-dot-net-core-on-azure-web-app
- Default branch = master

# Triggers
- Continuous Integration (CI) = true

# Steps 
- Restore
  - Type = .NET Core (Preview)
  - Command = restore
  - Project(s) = **/project.json
- Build
  - Type = .NET Core (Preview)
  - Command = build
  - Project(s) = **/project.json
  - Arguments = --configuration $(BuildConfiguration)
- Test
  - Type = .NET Core (Preview)
  - Command = test
  - Project(s) = **/*UnitTests/project.json
  - Arguments = --configuration $(BuildConfiguration) -xml TEST-TestResults.xml
- Publish Test Results
  - Type = Publish Test Results
  - Test Result Format = XUnit
  - Test Results Files = **/TEST-*.xml
- Publish Web App
  - Type = .NET Core (Preview)
  - Command = publish
  - Publish Web Projects = true
  - Arguments = --configuration $(BuildConfiguration) --output $(build.artifactstagingdirectory)
  - Zip Published Projects = true
- Web App
  - Type = Publish Build Artifacts
  - Path to publish = $(build.artifactstagingdirectory)
  - Artifact Name = web-app
  - Artifact Type = Server
- ARM Templates
  - Type = Copy and Publish Build Artifacts
  - Copy Root = release/ManageAzureWebAppAzureResourceGroup/templates
  - Contents = *.json
  - Artifact Name = arm-templates
  - Artifact Type = Server
- PowerShell Scripts
  - Type = Copy and Publish Build Artifacts
  - Copy Root = release/ManageAzureWebAppAzureResourceGroup/scripts
  - Contents = *.ps1
  - Artifact Name = scripts
  - Artifact Type = Server
