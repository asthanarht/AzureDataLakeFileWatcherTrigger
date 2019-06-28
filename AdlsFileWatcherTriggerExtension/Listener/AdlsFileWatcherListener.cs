using AdlsFileWatcherTriggerExtension.Service;
using Microsoft.Azure.DataLake.Store;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdlsFileWatcherTriggerExtension.Listener
{
    public class AdlsFileWatcherListener : IListener
    {
        private readonly AdlsWatcherTriggerAttribute adlsWatcherTriggerAttribute;
        private CancellationTokenSource _listenerStoppingTokenSource;
        DateTime? lastScanDateTime = null;
        public AdlsFileWatcherListener(ITriggeredFunctionExecutor executor, AdlsWatcherTriggerAttribute adlsWatcherTriggerAttribute)
        {
            Executor = executor;
            this.adlsWatcherTriggerAttribute = adlsWatcherTriggerAttribute;
        }

        public ITriggeredFunctionExecutor Executor { get; }

        public void Cancel()
        {
        }

        public void Dispose()
        {
            //todo
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _listenerStoppingTokenSource = new CancellationTokenSource();
                string ClientId = Environment.GetEnvironmentVariable("ClientId");
                string TenantId = Environment.GetEnvironmentVariable("TenantId");
                string AppKey = Environment.GetEnvironmentVariable("AppKey");
                string DataLakeName = Environment.GetEnvironmentVariable("DataLakeName");

                //Create AdlsClientObject
                AdlsClient adlsClient = await DataLakeAdlsService.CreateAdlsClientAsync(TenantId, AppKey, ClientId, DataLakeName);

                if (adlsClient != null)
                {
                    while (!_listenerStoppingTokenSource.IsCancellationRequested)
                    {
                        EnumerateFile(adlsClient, adlsWatcherTriggerAttribute.FolderName, this.lastScanDateTime, DateTime.UtcNow,

                           async (fileName,fileFullName,modifiedName) =>
                            {

                                var data = new AdlsFileChangeEvent
                                {
                                    FileName = fileName,
                                    FileFullPath = fileFullName,
                                    FileStram = await adlsClient.GetReadStreamAsync(fileFullName)
                                
                                 };

                                var triggerData = new TriggeredFunctionData
                                {
                                    TriggerValue = data
                                };

                                await Executor.TryExecuteAsync(triggerData, CancellationToken.None);

                            }
                            );
                      
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public void EnumerateFile(AdlsClient adlsClient, string adlsFolderPath, DateTime? lastScanDateTime, DateTime? currentScanDateTime, Action<string, string, DateTime?> callbackFile)
        {
            try
            {
                var enumerateDir = adlsClient.EnumerateDirectory(adlsFolderPath);

               Parallel.ForEach(enumerateDir, dict =>
                {
                    if (dict.Type == DirectoryEntryType.DIRECTORY)
                    {
                        //Console.WriteLine($"dict name is {dict.Name} and {dict.FullName}");
                        EnumerateFile(adlsClient,$"{adlsFolderPath}/{dict.Name}", lastScanDateTime, currentScanDateTime, callbackFile);
                    }
                    else if (dict.Type == DirectoryEntryType.FILE)
                    {
                            /// Condition to enumerate all file on first sucessful service start
                            if (!lastScanDateTime.HasValue)
                            {
                                callbackFile(dict.Name, dict.FullName, dict.LastModifiedTime);
                            }
                            else if (dict.LastModifiedTime >= lastScanDateTime && dict.LastModifiedTime <= currentScanDateTime)
                            {
                                callbackFile(dict.Name, dict.FullName, dict.LastModifiedTime);
                            }
                    }

                });

            }
            catch (AdlsException adlsException)
            {
                if (adlsException.HttpStatus.Equals(HttpStatusCode.Forbidden))
                {
                    //Do nothing. Need to fix for access control error handle for few folders. For now just skip it 
                }
                else
                {
                    throw;
                }

            }

            this.lastScanDateTime = currentScanDateTime;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
