using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace buttonCSharp.Tests.Models
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }
}
