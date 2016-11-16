Here is one example to build this Web App via VSTS.

![Build Overview](/docs/Build.PNG)

#Variables
- BuildConfiguration = release
- DOTNET_SKIP_FIRST_TIME_EXPERIENCE = true

#Steps 
- .NET Core (Preview)
  - Command = restore
  - Project(s) = **/project.json
- .NET Core (Preview)
  - Command = build
  - Project(s) = **/project.json
  - Arguments = --configuration $(BuildConfiguration)
- .NET Core (Preview)
  - Command = test
  - Project(s) = **/*UnitTests/project.json
  - Arguments = --configuration $(BuildConfiguration) -xml TEST-TestResults.xml
- Publish Test Results
  - Test Result Format = XUnit
  - Test Results Files = **/TEST-*.xml
- .NET Core (Preview)
  - Command = publish
  - Publish Web Projects = true
  - Arguments = --configuration $(BuildConfiguration) --output $(build.artifactstagingdirectory)
  - Zip Published Projects = true
- Publish Build Artifacts
  - Path to publish = $(build.artifactstagingdirectory)
  - Artifact Name = drop-web-app
  - Artifact Type = Server
- Copy and Publish Build Artifacts
  - Copy Root = release/ManageAzureWebAppAzureResourceGroup
  - Contents = *.json
  - Artifact Name = drop-web-app
  - Artifact Type = Server
