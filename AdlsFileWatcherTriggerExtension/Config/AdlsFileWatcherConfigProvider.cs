using AdlsFileWatcherTriggerExtension.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdlsFileWatcherTriggerExtension.Config
{
    public class AdlsFileWatcherConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var rule = context.AddBindingRule<AdlsWatcherTriggerAttribute>();

            rule.BindToTrigger<AdlsFileChangeEvent>(new AdlsFileWatcherAttributeBindingProvider());
        }
    }
}
