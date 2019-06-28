using AdlsFileWatcherTriggerExtension;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: WebJobsStartup(typeof(AdlsFileWatcherWebJobStartup), "AdlsFileWatcher")]
namespace AdlsFileWatcherTriggerExtension
{
    public class AdlsFileWatcherWebJobStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddAdlsFileWatcher();
        }
    }
}
