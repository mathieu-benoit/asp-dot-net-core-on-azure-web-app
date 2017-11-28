Here is one example to Build an ASP.NET Core 2.0 web application to an App Service (Windows) via VSTS. You could adapt it with your own context, needs and constraints.

2 ways to create the associated Build Definition:

- [Import the Build Definition](#import-the-build-definition)
- [Create manually the Build Definition](#create-manually-the-build-definition)
  - [Repository](#repository)
  - [Triggers](#triggers)
  - [Process - Build process](#process---build-process)
  - [Steps](#steps)

![Build Overview](/docs/imgs/AspDotNetCore-AppServiceWindows-CI.PNG)

# Import the Build Definition

You could import [the associated Build Definition stored in this repository](/vsts/AspDotNetCore-AppServiceWindows-CI.json) and then follow these steps to adapt it to your current project, credentials, etc.:

TODO

# Create manually the Build Definition

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

TODO