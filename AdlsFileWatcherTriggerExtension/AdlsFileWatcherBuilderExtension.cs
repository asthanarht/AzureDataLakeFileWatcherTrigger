using AdlsFileWatcherTriggerExtension.Config;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdlsFileWatcherTriggerExtension
{
    public static class AdlsFileWatcherBuilderExtension
    {
        public static IWebJobsBuilder AddAdlsFileWatcher(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddExtension<AdlsFileWatcherConfigProvider>();

            return builder;
        }
    }
}
