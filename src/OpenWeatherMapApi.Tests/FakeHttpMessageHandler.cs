using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWeatherMapApi.Tests
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private HttpResponseMessage _httpResponseMessage;

		public FakeHttpMessageHandler(HttpResponseMessage httpResponseMessage)
		{
            _httpResponseMessage = httpResponseMessage;
		}

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_httpResponseMessage);
        }
    }
}