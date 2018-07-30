using FunctionTestHelper;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FunctionApp.Tests.Integration
{
    [Collection("Function collection")]
    public class IoTEndToEndTests : EndToEndTestsBase<TestFixture>
    {
        private readonly ITestOutputHelper output;
        public IoTEndToEndTests(TestFixture fixture, ITestOutputHelper output) : base(fixture)
        {
            this.output = output;
        }

        [Fact]
        public async Task IoTHub_TriggerFires()
        {

            string connectionString = Environment.GetEnvironmentVariable("DeviceConnectionString");

            var iotClient = DeviceClient.CreateFromConnectionString(connectionString);
            string guid = Guid.NewGuid().ToString();
            try
            {
                await iotClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes("Test" + guid)));

                await WaitForTraceAsync("ButtonPressAsync", log => log.FormattedMessage.Contains(guid));
                output.WriteLine(Fixture.Host.GetLog());
            }
            catch(Exception ex)
            {
                output.WriteLine(Fixture.Host.GetLog());
                throw ex;
            }
        }
    }
}
