Sample ASP.NET Core app which was used during my demonstration for [my presentation "Your DevOps journey starts with ALM!" at the Agile Tour Quebec city 2016](http://aka.ms/mabenoit-atq2016).

# History of changes
- Update to ASP.NET Core 1.1.

# Architecture Overview
For this demonstration as Microsoft Azure services I use App Service (Web App), Application Insights and Sql Database. 
To manage them (create, update or delete) I use ARM Templates.

![Architecture Overview](/docs/imgs/Overview.PNG)

# Process Overview
The initial state of the demonstration is to have on one hand the source code of an ASP.NET application in GitHub and the associated Production environment.
Then I do one changing on one cshtml file and one changing on one ARM Template to change an infrastructure setting.
The commit of these changes will trigger automatically the Build (Continuous Integration - CI) and then the associated Release (Continuous Delivery - CD) accross the different environments until Production. 
Staging and Preview dont exist on the initial state of this demo, so here we are illustrating the creation of these environments "on-demand" before publishing our changes in Production to check and guarantee the quality of our application before being concretely used by our end-users.

![Process Overview](/docs/imgs/Process.PNG)

# Build with VSTS

[Here is the associated documentation for the AspDotNetCore-AppServiceWindows-CI Build Definition.](/docs/AspDotNetCore-AppServiceWindows-CI.md)

# Release with VSTS

[Here is the associated documentation for the AspDotNetCore-AppServiceWindows-CD Release Definition.](/docs/AspDotNetCore-AppServiceWindows-CD.md)

# Further steps:
- Deploy the application on Web App on Linux.
- Use Azure Key Vault to store secrets