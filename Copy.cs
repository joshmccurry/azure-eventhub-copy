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
                    // Replace these two lines with your processing logic.
                    log.LogInformation($"C# Event Hub trigger function processed a message: {eventData.EventBody}");
                    await outputEvents.AddAsync(eventData);
                } catch (Exception e) {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }

        [FunctionName("Listen")]
        public static async Task RunListen(
                [EventHubTrigger("dest", Connection = "DESEventHubConnString")] EventData[] events,
                ILogger log) {
            var exceptions = new List<Exception>();
            foreach (EventData eventData in events) {
                try {
                    // Replace these two lines with your processing logic.
                    log.LogInformation($"C# Event Hub trigger function processed a message: {eventData.EventBody}");
                } catch (Exception e) {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
