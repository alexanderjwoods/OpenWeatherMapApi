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
		private const string _apiKey = "YOURAPIKEY";
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

		[TestCase("Detroit", "", "")]
		[TestCase("Detroit", "Michigan", "")]
		[TestCase("Detroit", "Michigan", "USA")]
		[TestCase("Dublin", "", "IE")]
		public async Task GetCurrentWeatherByCityName_CurrentWeatherResponse_ValidCity(string city, string state, string country)
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
			var response = await client.GetCurrentWeatherByCityName(city, state, country);

			// Assert
			Assert.IsInstanceOf<CurrentWeatherResponse>(response);

		}

		[Test]
		public void GetCurrentWeatherByCityName_Exception_InvalidCity()
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
			Assert.ThrowsAsync<Exception>(async () => await client.GetCurrentWeatherByCityName("123"));
		}

		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		public void GetCurrentWeatherByCityName_ArgNullEx_NullOrWhitespaceCity(string city)
        {
			// Arrange
			var client = new OpenWeatherMapClient(_apiKey);
			
			// Assert
			Assert.ThrowsAsync<ArgumentNullException>(async () => await client.GetCurrentWeatherByCityName(city));
        }

		[Test]
		public void GetCurrentWeatherByCityId_ArgEx_IdLessThanOne()
        {
			// Arrange
			var client = new OpenWeatherMapClient(_apiKey);

			// Assert
			Assert.ThrowsAsync<ArgumentException>(async () => await client.GetCurrentWeatherByCityId(-1));
        }

		[Test]
		public void GetCurrentWeatherByCityId_Exception_BadId()
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
			Assert.ThrowsAsync<Exception>(async () => await client.GetCurrentWeatherByCityId(1));
		}

		[Test]
		public async Task GetCurrentWeatherByCityId_CurrentWeatherResponse_ValidId()
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
			var response = await client.GetCurrentWeatherByCityId(1);

			// Assert
			Assert.IsInstanceOf<CurrentWeatherResponse>(response);
		}
	}
}