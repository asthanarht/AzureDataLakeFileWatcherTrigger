using AdlsFileWatcherTriggerExtension;
using Microsoft.Azure.WebJobs.Host.Triggers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdlsFileWatcherTriggerExtension.Bindings
{
    public class AdlsFileWatcherAttributeBindingProvider : ITriggerBindingProvider
    {
        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            //Retrieve Parameter
            var parameter = context.Parameter;
            AdlsWatcherTriggerAttribute attribute = parameter.GetCustomAttribute<AdlsWatcherTriggerAttribute>(inherit: false);
            if (attribute == null)
            {
                return Task.FromResult<ITriggerBinding>(null);
            }

            //Validate Trigger
            if (!IsSupportedBindingType(parameter.ParameterType))
            {
                throw new InvalidOperationException($"Can't bind TwitterTriggerAttribute to type '{parameter.ParameterType}'");
            }

            return Task.FromResult<ITriggerBinding>(new AdlsFileWatcherTriggerBinding(parameter));
        }

        private bool IsSupportedBindingType(Type t)
        {
            return t == typeof(AdlsFileChangeEvent);
        }
    }
}
