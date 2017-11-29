Here is one example to Build an ASP.NET Core 2.0 web application to an App Service (Linux) via VSTS. You could adapt it with your own context, needs and constraints.

![Build Overview](/docs/imgs/AspDotNetCore-AppServiceLinux-CI.PNG)

# Prerequisities - Create manually an Azure Container Registry

Checkout the CLI 2.0 commands to accomplish that [here](https://github.com/mathieu-benoit/nodejs-on-azure-containers/blob/master/docs/AzureContainerRegistry.md).

# Create the Build Definition

Let's [create a new YAML build definition](https://docs.microsoft.com/en-us/vsts/build-release/actions/build-yaml#manually-create-a-yaml-build-definition).

*For now, the graphical representation of the tasks doesn't exist with the YAML definition. If you would like you could manually reproduce the tasks defined [in this file](../vsts/AspDotNetCore-AppServiceLinux-CI.yml) via the UI editor.*

- **Repository**
  - Repository Type = GitHub
  - Connection = set appropriate
  - Repository = `mathieu-benoit/asp-dot-net-core-on-azure-web-app`
  - Default branch = `master`
- **Process - Build process**
  - Name = `AspDotNetCore-AppServiceLinux-CI`
  - Default agent queue = `Hosted Linux Preview`
  - YAML path = [`vsts/AspDotNetCore-AppServiceLinux-CI.yml`](../vsts/AspDotNetCore-AppServiceLinux-CI.yml)
- **Triggers**
  - Continuous Integration = Enabled
    - Choose the correct repository
    - Branch Filters
      - Type = Include ; Branch specification = master
- **Options** - *For now, not available with the YAML build definition.*
  - Create work item on failure = Enabled
    - Type = Bug
    - Assign to requestor = true