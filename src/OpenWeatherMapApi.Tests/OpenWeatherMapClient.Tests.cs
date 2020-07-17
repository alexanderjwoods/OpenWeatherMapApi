using NUnit.Framework;

using OpenWeatherMapApi.Responses;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenWeatherMapApi.Tests
{
	public class OpenWeatherMapClientTests
	{
		private const string _apiKey = "YOURAPIKEY"; //YOUR API KEY HERE
		private static string _goodCurrentWeatherResponsePath;
		private static string _badCurrentWeatherResponsePath;

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			_goodCurrentWeatherResponsePath = $"{Directory.GetCurrentDirectory()}\\Responses\\GoodCurrentWeatherResponse.json";
			_badCurrentWeatherResponsePath = $"{Directory.GetCurrentDirectory()}\\Responses\\BadCurrentWeatherResponse.json";
		}

		[TestCase("")]
		[TestCase(null)]
		public void Ctor_ArgumentNullException_NullAppiKey(string apiKey)
		{
			Assert.Throws<ArgumentNullException>(() => new OpenWeatherMapClient(apiKey));
		}

		[Test]
		public void Ctor_Instantiates_NonEmptyApiKey()
		{
			// Act
			var result = new OpenWeatherMapClient(_apiKey);

			// Assert
			Assert.IsInstanceOf<OpenWeatherMapClient>(result);
		}

		[TestCase("")]
		[TestCase(null)]
		public void GetCurrentWeatherByZip_ArgumentNullException_NullZipCode(string zip)
		{
			// Arrange
			var client = new OpenWeatherMapClient(_apiKey);

			// Assert
			Assert.ThrowsAsync<ArgumentNullException>(async () => await client.GetCurrentWeatherByZip(zip));
		}

		[Test]
		public async Task GetCurrentWeatherByZip_CurrentWeatherResponse_ValidZipCode()
		{
			// Arrange
			var json = File.ReadAllText(_goodCurrentWeatherResponsePath);
			var stringContent = new StringContent(json);
			var fakeHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = stringContent
			};
			var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpResponseMessage);
			var httpClient = new HttpClient(fakeHttpMessageHandler);

			var client = new OpenWeatherMapClient(_apiKey, httpClient);

			// Act
			var response = await client.GetCurrentWeatherByZip("12345");

			// Assert
			Assert.IsInstanceOf<CurrentWeatherResponse>(response);
		}

		[Test]
		public void GetCurentWeatherByZip_Exception_InvalidZip()
		{
			// Arrange
			var json = File.ReadAllText(_badCurrentWeatherResponsePath);
			var stringContent = new StringContent(json);
			var fakeHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
			{
				Content = stringContent
			};
			var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpResponseMessage);
			var httpClient = new HttpClient(fakeHttpMessageHandler);

			var client = new OpenWeatherMapClient(_apiKey, httpClient);

			// Assert
			Assert.ThrowsAsync<Exception>(async () => await client.GetCurrentWeatherByZip("55555"));
		}

		[Test]
		public async Task GetCurrentWeatherByCoords_CurrentWeatherResponse_ValidCoords()
		{
			// Arrange
			var json = File.ReadAllText(_goodCurrentWeatherResponsePath);
			var stringContent = new StringContent(json);
			var fakeHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = stringContent
			};
			var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpResponseMessage);
			var httpClient = new HttpClient(fakeHttpMessageHandler);

			var client = new OpenWeatherMapClient(_apiKey, httpClient);

			// Act
			var response = await client.GetCurrentWeatherByCoords(139, 35);

			// Assert
			Assert.IsInstanceOf<CurrentWeatherResponse>(response);
		}

		[Test]
		public void GetCurrentWeatherByCoords_Exception_InvalidCoords()
		{
			// Arrange
			var json = File.ReadAllText(_badCurrentWeatherResponsePath);
			var stringContent = new StringContent(json);
			var fakeHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
			{
				Content = stringContent
			};
			var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpResponseMessage);
			var httpClient = new HttpClient(fakeHttpMessageHandler);

			var client = new OpenWeatherMapClient(_apiKey, httpClient);

			// Assert
			Assert.ThrowsAsync<Exception>(async () => await client.GetCurrentWeatherByCoords(1390, 350));
		}
	}
}
