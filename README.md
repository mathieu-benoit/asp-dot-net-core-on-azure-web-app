Sample ASP.NET Core app which was used during my demonstration for [my presentation "Your DevOps journey starts with ALM!" at the Agile Tour Quebec city 2016](http://aka.ms/mabenoit-atq2016).

#Architecture Overview
For this demonstration as Microsoft Azure services I use App Service (Web App), Application Insights and Sql Database. 
To manage them (create, update or delete) I use ARM Templates.

![Architecture Overview](/docs/Overview.PNG)

#Process Overview
The initial state of the demonstration is to have on one hand the source code of an ASP.NET application in GitHub and the associated Production environment.
Then I do one changing on one cshtml file and one changing on one ARM Template to change an infrastructure setting.
The commit of these changes will trigger automatically the Build (CI) and then the associated Release (CD) on QA. QA doesn't exist on the initial state of this demo, so here we are illustrating the creation of the QA environment "on-demand".

![Process Overview](/docs/Process.PNG)

#Build with VSTS

[Here is the associated documentation.](/docs/Build.md)

#Release with VSTS

[Here is the associated documentation.](/docs/Release.md)

#Further steps:
- Update to ASP.NET Core 1.1. Waiting for its [support in the default VSTS Build agent](https://www.visualstudio.com/en-us/docs/build/admin/agents/hosted-pool).
- Deploy the application on Web App on Linux.
- Use Azure Key Vault to store secrets