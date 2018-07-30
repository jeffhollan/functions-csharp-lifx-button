using IoTHubTrigger = Microsoft.Azure.WebJobs.ServiceBus.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace buttonCSharpVSTS
{
    public static class IoTButton
    {
        public static Lazy<HttpClient> client = new Lazy<HttpClient>(() => { return new HttpClient(); });

        [FunctionName(nameof(IoTButton.ButtonPressAsync))]
        public static async Task ButtonPressAsync(
            [IoTHubTrigger("messages/events", Connection = "EventHubConnectionString")]EventData message, 
            ILogger log)
        {
            // log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
            log.LogInformation("A change that should break the build");
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "https://api.lifx.com/v1/lights/all/toggle");
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("LIFX_TOKEN"));

            var res = await client.Value.SendAsync(req);
            
            log.LogInformation($"LINX response code: {res.StatusCode}");
        }
    }
}
