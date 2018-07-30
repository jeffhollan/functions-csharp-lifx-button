using buttonCSharp.Tests.Models;
using buttonCSharp.Function;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace buttonCSharp.Tests
{
    public class IoTButtonTest
    {
        private ILogger logger;
        private Mock<MockHttpMessageHandler> mockHttpMessageHandler;
        private readonly ITestOutputHelper output;

        public IoTButtonTest(ITestOutputHelper output)
        {
            mockHttpMessageHandler = new Mock<MockHttpMessageHandler> { CallBase = true };
            logger = NullLoggerFactory.Instance.CreateLogger("Test");
            this.output = output;
        }
        [Fact]
        public async Task OKFromLIFX_Mock()
        {
            EventData data = new EventData(Encoding.UTF8.GetBytes("Test Message"));

            mockHttpMessageHandler.Setup(m => m.Send(It.IsAny<HttpRequestMessage>())).Returns(
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("[]", Encoding.UTF8, "application/json")
                });

            IoTButton.client = new Lazy<HttpClient>(() => { return new HttpClient(mockHttpMessageHandler.Object); });
            await IoTButton.ButtonPressAsync(data, logger);
            
        }

        [Fact]
        public async Task OKFromLIFX_Real()
        {
            EventData data = new EventData(Encoding.UTF8.GetBytes("Test Message"));

            IoTButton.client = new Lazy<HttpClient>(() => { return new HttpClient(); });
            await IoTButton.ButtonPressAsync(data, logger);
        }
    }
}
