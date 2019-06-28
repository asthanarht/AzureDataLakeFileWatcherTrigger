using AdlsFileWatcherTriggerExtension.Listener;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdlsFileWatcherTriggerExtension.Bindings
{
    public class AdlsFileWatcherTriggerBinding : ITriggerBinding
    {
        private readonly ParameterInfo _parameter;
        private readonly AdlsWatcherTriggerAttribute _attribute;
        public Type TriggerValueType => typeof(AdlsFileChangeEvent);

        public IReadOnlyDictionary<string, Type> BindingDataContract => CreateBindingContract();
        private readonly Task<ITriggerData> _emptyBindingDataTask = Task.FromResult<ITriggerData>(new TriggerData(null, new Dictionary<string, object>()));
        public AdlsFileWatcherTriggerBinding(ParameterInfo parameter)
        {
            _parameter = parameter;
            _attribute = parameter.GetCustomAttribute<AdlsWatcherTriggerAttribute>(inherit: false);
        }

        private IReadOnlyDictionary<string, Type> CreateBindingContract()
        {
            Dictionary<string, Type> contract = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            contract.Add("AdlsFileWatcherTrigger", typeof(AdlsFileChangeEvent));
            return contract;
        }

        public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
        {
            return _emptyBindingDataTask;
        }

        public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return Task.FromResult<IListener>(new AdlsFileWatcherListener(context.Executor, _attribute));
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            string FolderName = _attribute.FolderName;
            string FileName = _attribute.FileName;

            return new AdlsFileWatcherTriggerParameterDescriptor
            {
                Name = _parameter.Name,
                DisplayHints = new ParameterDisplayHints
                {
                    Prompt = "Enter a folder name to watch on",
                    Description = $"watch enable on folder {FolderName}",
                    DefaultValue = "Test"
                }
            };
        }

        private class AdlsFileWatcherTriggerParameterDescriptor : TriggerParameterDescriptor
        {
            public override string GetTriggerReason(IDictionary<string, string> arguments)
            {
                if (arguments != null && arguments.TryGetValue(Name, out var filter))
                {
                    return $"New File detected at {DateTime.Now.ToString("o")}";
                }
                return null;
            }
        }
    }
}
