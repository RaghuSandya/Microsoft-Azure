# Microsoft Azure Storage Table SDK

 The Microsoft Azure Storage SDK allows you to build Azure applications that take advantage of scalable cloud computing resources.

## Features

- Tables
    - Create/Delete Tables
    - Query/Create/Read/Update/Delete Entities
    
## Environment Setup

- You can use NuGet to obtain both packages. Follow these steps:
  - Right-click your project in Solution Explorer, and choose Manage NuGet Packages.
  - Search online for "Microsoft.Azure.Storage.Common (9.0.0.1)", and select Install to install the Azure Storage Common Library for .NET (Preview) and its dependencies. Ensure the Include prerelease box is checked as this is a preview package.
  - Search online for "Microsoft.Azure.CosmosDB.Table (1.1.2)", and select Install to install the Microsoft Azure CosmosDB Table Library.
  - Search online for "WindowsAzure.ConfigurationManager (3.2.3)", and select Install to install the Microsoft Azure Configuration Manager Library.


## App.config

- Add the following code after `<startup></startup>` in App.config file of the console application 

    `<appSettings>`
    
        <add key="connectionString" value="Connection String from Access Keys or Shared Access Signature"/>
    
    `</appSettings>`
