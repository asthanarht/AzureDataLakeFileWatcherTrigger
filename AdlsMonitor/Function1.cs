using System;
using System.IO;
using AdlsFileWatcherTriggerExtension;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AdlsMonitor
{
    public static class Function1
    {
        static int count = 1;
        [FunctionName("Function1")]
        
        public static void Run([AdlsWatcherTrigger(FolderName ="/")]AdlsFileChangeEvent myfileEvent, ILogger log)
        {
            log.LogInformation($"file found and name is {myfileEvent.FileFullPath} and count is  {count++}");
        }
    }
}
