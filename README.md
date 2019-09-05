# AzureDataLakeFileWatcherTrigger
Azure data lake file watcher is custom trigger extension for azure function and azure web job 

The following binding can be used with Azure Functions v2 C# Class Library. 

## Instructions to Use
Get it From Nuget 
## NuGet package

(Install-Package AdlsFileWatcherTriggerExtension -Version 0.0.1(https://www.nuget.org/packages/AdlsFileWatcherTriggerExtension/)

Clone repo and add a reference to the * AdlsFileWatcherTriggerExtension;* project. 

```c#
using AdlsFileWatcherTriggerExtension;
```
#### Output Binding
Add the following attributes that include the account FQDN, ApplicationId, Client Secret and Tenant Id.

```c#
[DataLakeStore(
  AccountFQDN = @"fqdn", 
  ApplicationId = @"applicationid", 
  ClientSecret = @"clientsecret", 
  TenantID = @"tentantid")]out DataLakeStoreOutput dataLakeStoreOutput
```
View a [sample function](samples/DataLakeExtensionSamples/OutputFromBlob.cs) using output binding.

#### Input Binding
Add *FileName* property to retrieve a specific file from your Datalake Store.

```c#
public static class Function1
    {
        static int count = 1;
        [FunctionName("Function1")]
        
        public static void Run([AdlsWatcherTrigger(FolderName ="/")]AdlsFileChangeEvent myfileEvent, ILogger log)
        {
            log.LogInformation($"file found and name is {myfileEvent.FileFullPath} and count is  {count++}");
        }
    }
```
View a [sample function  using input binding.

## Binding Requirements 

1. [Azure Data Lake Store](https://azure.microsoft.com/en-us/services/data-lake-store/)
2. Setup [Service to Service Auth](https://docs.microsoft.com/en-us/azure/data-lake-store/data-lake-store-service-to-service-authenticate-using-active-directory) using Azure AD
3. [Azure Functions and Webjobs tools](https://marketplace.visualstudio.com/items?itemName=VisualStudioWebandAzureTools.AzureFunctionsandWebJobsTools) extension 
4. Add the application settings noted below. 

### local.settings.json expected content
```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "AzureWebJobsDashboard": "UseDevelopmentStorage=true",
    "DataLakeName": "<your Data Lake Store Name>",
    "TenantId": "<Your Tenant Id>",
    "AppKey": "<your ADLS App Key>",
    "ClientId": "<your SPN Client id>"
  }
}
```
## End to End Testing

If you wish to run and or make modifications to the E2E testing you will need to create an appsettings.json  with all the required settings. Use the standard format for values instead of the functions formatting. 



## License

This project is under the benevolent umbrella of the [.NET Foundation](http://www.dotnetfoundation.org/) and is licensed under [the MIT License](https://github.com/Azure/azure-webjobs-sdk/blob/master/LICENSE.txt)

## Contributing

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
