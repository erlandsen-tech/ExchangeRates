using BLL;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ScheduledFetch
{
    public static class FetchAndUpdate
    {
        [FunctionName("FetchAndUpdate")]
        public static async void Run([TimerTrigger("0 00 00 * * *")] TimerInfo myTimer, ILogger log)
        {
            ScheduledLogic scl = new ScheduledLogic();
            if (await scl.GetData())
            {
                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
                log.LogInformation("The task was a success. Database updated.");
            }
            else
            {
                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
                log.LogInformation("The task was a failure. Database not updated.");
            }
        }
        private class ScheduledLogic
        {
            private NetworkHandler handler = new NetworkHandler();
            private DbLogic dbLogic = new DbLogic();

            public async Task<bool> GetData()
            {
                var rates = await handler.GetRates();
                return dbLogic.Insert(rates);
            }
        }

    }
}