using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Eventhub_CopyNamespace {
    public static class Copy {
        [FunctionName("Copy")]
        public static async Task RunCopy(
            [EventHubTrigger("source", Connection = "SRCEventHubConnString")] EventData[] events,
            [EventHub("dest", Connection = "DESEventHubConnString")] IAsyncCollector<EventData> outputEvents,
            ILogger log) {
            var exceptions = new List<Exception>();
            foreach (EventData eventData in events) {
                try {
                    log.LogInformation($"C# Event Hub trigger function processed a message: {eventData.EventBody}");
                    await outputEvents.AddAsync(eventData);
                } catch (Exception e) {
                    exceptions.Add(e);
                }
            }
            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
        /**
        [FunctionName("Listen")]
        public static async Task RunListen(
                [EventHubTrigger("dest", Connection = "DESEventHubConnString")] EventData[] events,
                ILogger log) {
            var exceptions = new List<Exception>();
            foreach (EventData eventData in events) {
                try {
                    log.LogInformation($"C# Event Hub trigger function processed a message: {eventData.EventBody}");
                } catch (Exception e) {
                   exceptions.Add(e);
                }
            }
            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }**/
    }
}
