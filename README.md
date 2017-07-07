Sample ASP.NET Core 1.1 app which was used during my demonstration for [my presentation "Your DevOps journey starts with ALM!" at the Agile Tour Quebec city 2016](http://aka.ms/mabenoit-atq2016) and updated according the history of changes below.

# History of changes
- (in progress) July 2017 - Setup deployment as App Service (Linux)
- June 2017 - Update to ASP.NET Core 1.1.
- November 2016 - Initial setup.

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

# Build Definition with VSTS

- [On an App Service (Windows)](/docs/AspDotNetCore-AppServiceWindows-CI.md)
- [On an App Service (Linux)](/docs/AspDotNetCore-AppServiceLinux-CI.md)

# Release Definition with VSTS

- [On an App Service (Windows)](/docs/AspDotNetCore-AppServiceWindows-CD.md)
- [On an App Service (Linux)](/docs/AspDotNetCore-AppServiceLinux-CD.md)

# "Deploy to Azure" buttons

By using the buttons below it's another way to deploy the Azure services without VSTS and without taking into account the web app, just deploying the infrastructure within Azure.

Deploy the Azure App Service (Windows) and its associated services within the Azure portal:

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmathieu-benoit%2Fasp-dot-net-core-on-azure-web-app%2Fmaster%2Finfra%2FAspNetCoreApplication.Infrastructure%2Ftemplates%2Fdeploy-windows.json" target="_blank">![Deploy to Azure](http://azuredeploy.net/deploybutton.png)</a>

Deploy the Azure App Service (Linux) and its associated services within the Azure portal:

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmathieu-benoit%2Fasp-dot-net-core-on-azure-web-app%2Fmaster%2Finfra%2FAspNetCoreApplication.Infrastructure%2Ftemplates%2Fdeploy-linux.json" target="_blank">![Deploy to Azure](http://azuredeploy.net/deploybutton.png)</a>

Deploy the Azure Container Registry and its associated Blob Storage within the Azure portal:

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmathieu-benoit%2Fasp-dot-net-core-on-azure-web-app%2Fmaster%2Finfra%2FAspNetCoreApplication.Infrastructure%2Ftemplates%2FContainerRegistry.json" target="_blank">![Deploy to Azure](http://azuredeploy.net/deploybutton.png)</a>
