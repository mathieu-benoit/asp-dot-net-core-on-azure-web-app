ALM and DevOps practices with a sample ASP.NET Core 1.1 web app hosted in Azure Web App (Windows and Linux).

TOC

- [History of changes](#history-of-changes)
- [Overview](#overview)
- [App Service (Windows) - Build and Release Definitions with VSTS](#app-service-windows---build-and-release-definitions-with-vsts)
- [App Service (Linux) - Build and Release Definitions with VSTS](#app-service-linux---build-and-release-definitions-with-vsts)
- [Other Misc DevOps practices implemented](#other-misc-devops-practices-implemented)
- [Alternatives and potantial further considerations](#alternatives-and-potantial-further-considerations)
- [Resources](#resources)

# History of changes

- (in progress) July 2017 - Setup deployment as App Service (Linux)
- June 2017 - Update to ASP.NET Core 1.1.
- November 2016 - Initial setup deployment as App Service (Windows) [for my presentation "Your DevOps journey starts with ALM!" at the Agile Tour Quebec city 2016](http://aka.ms/mabenoit-atq2016)

# Overview

The goal of this GitHub repository is to demonstrate and use DevOps practices by leveraging a very simple ASP.NET Core web application on Azure Web Apps on both platform Windows and Linux. ASP.NET Core has this ability to be cross-platform, so throughout this GitHub repository, with one codebase/project we will see how deploy this web application on both platform: App Service Windows (WebDeploy package) and App Service Linux (Docker container).

By opening the .sln with Visual Studio 2017 you should see the structure of the solution like this:

![Visual Studio Solution Structure Overview](/docs/imgs/Visual-Studio-Solution-Structure-Overview.PNG)

To be able to setup the Build and Release definitions within VSTS described in the section below, you will need a Team Services (VSTS) account. If you don't have one, you could create it for free [here](https://www.visualstudio.com/team-services/).

To be able to deploy  the Azure services (Function App, Application Insights, etc.), you will need an Azure subscription. If you don't have one, you could create it for free [here](https://azure.microsoft.com/fr-ca/free/).

# App Service (Windows) - Build and Release Definitions with VSTS

![Architecture Overview](/docs/imgs/Process-Overview-Windows.PNG)

For the Build definition, details could be found here: [Build - CI](/docs/AspDotNetCore-AppServiceWindows-CI.md)

Here are the DevOps practices highlighted within this CI pipeline:
- CI/Build triggered at each commit on the master branch
- Compile the ASP.NET Core application
- Run unit tests
- Infrastructure as Code with the ARM Templates and the PowerShell scripts
- Run ARM Templates validation
- Expose artifacts to be used then by the CD pipeline (WebDeploy package, ARM Templates, PowerShell scripts and UITests (Selenium) dlls)
- Create a bug work item on build failure (assign to requestor)

For the Release definition, details could be found here: [Release - CD](/docs/AspDotNetCore-AppServiceWindows-CD.md)

Here are the DevOps practices highlighted within this CD pipeline:
- CD triggered at each CI/Build succesfully completed
- Deploy the Infrastructure as Code with the ARM Templates and the PowerShell scripts
- Deploy the ASP.NET Core application on an Azure PaaS service: Azure App Service (Web App) on Windows
- Run UITests (Selenium) once the Web App is deployed on Staging
- Use the Staging Slot mechanism with the associated Swap action to minimize downtime while upgrading the Production
- Use Entity Framework code-first approach to manage the database migrations
- Securing the production environment by adding a Lock on the associated Azure Resource Group
- Monitor the Web App by using Application Insights

# App Service (Linux) - Build and Release Definitions with VSTS

![Architecture Overview](/docs/imgs/Process-Overview-Linux.PNG)

For the Build definition, details could be found here: [Build - CI](/docs/AspDotNetCore-AppServiceLinux-CI.md)

Here are the DevOps practices highlighted within this CI pipeline:
- CI/Build triggered at each commit on the master branch
- Compile the ASP.NET Core application
- Run unit tests
- Infrastructure as Code with the ARM Templates and the PowerShell scripts
- Run ARM Templates validation
- Publish the web app as a Docker container in an Azure Container Registry
- Expose artifacts to be used then by the CD pipeline (ARM Templates, PowerShell scripts and UITests (Selenium) dlls)
- Create a bug work item on build failure (assign to requestor)

For the Release definition, details could be found here: [Release - CD](/docs/AspDotNetCore-AppServiceLinux-CD.md)

Here are the DevOps practices highlighted within this CD pipeline:
- CD triggered at each CI/Build succesfully completed
- Deploy the Infrastructure as Code with the ARM Templates and the PowerShell scripts
- Deploy the Docker container on an Azure PaaS service: Azure App Service (Web App) on Linux
- Run UITests (Selenium) once the Web App is deployed on Staging
- Use the Staging Slot mechanism with the associated Swap action to minimize downtime while upgrading the Production
- Use Entity Framework code-first approach to manage the database migrations
- Securing the production environment by adding a Lock on the associated Azure Resource Group
- Monitor the Web App by using Application Insights

# Other Misc DevOps practices implemented

- GitHub as source control to leverage key features for collaboration such as feature-branch with pull request, etc.
- CI/CD definitions as Code with the exported json file of the Build and Release Definitions

# Alternatives and potantial further considerations

- Improvements
    - [Configure VSTS and Microsoft Teams](https://almvm.azurewebsites.net/labs/vsts/teams/) (or Slack or HipChat, etc.) to add more collaboration by setting up notifications once a work item is updated, a commit is done, a build or release or done, etc.
    - Instead of just having a Production environment with its staging slot, having a QA environment with its associated staging too.
- Alternatives
    - Instead of using an Azure Container Registry to expose the Docker container, use DockerHub instead.
    - Instead of having an ASP.NET Core web application, use a NodeJS, Java, etc. web application instead.

# Resources

- TODO