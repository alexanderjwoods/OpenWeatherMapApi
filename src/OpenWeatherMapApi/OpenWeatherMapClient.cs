using OpenWeatherMapApi.Responses;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static OpenWeatherMapApi.Keys;
using static OpenWeatherMapApi.Keys.OpenWeatherMapAPIKey;

namespace OpenWeatherMapApi
{
	public class OpenWeatherMapClient
	{
		private readonly string _apiKey;
		private readonly HttpClient _client;

		/// <summary>
		/// Initializes a client to retrieve data from OpenWeaterMap
		/// </summary>
		/// <param name="apiKey">Your OpenWeatherMap Api Key in the struct OpenWeatherMapAPIKey.</param>
		public OpenWeatherMapClient(OpenWeatherMapAPIKey apiKey, HttpClient client = null)
		{
			_apiKey = string.IsNullOrEmpty(PreparationEngine.PrepareClientAPIKey(apiKey)) ? throw new ArgumentNullException(nameof(apiKey)) : PreparationEngine.PrepareClientAPIKey(apiKey);
			_client = client ?? new HttpClient();
		}

		/// <summary>
		/// Get Current Weather using a zip code
		/// </summary>
		/// <param name="zip">Zip code for current weather</param>
		/// <param name="countryCode">Optional: Country Code corresponding to zip.  US by default.</param>
		/// <param name="temperatureUnit">Temperature Unit - Imperial by default.</param>
		/// <returns></returns>
		public async Task<CurrentWeatherResponse> GetCurrentWeatherByZip(string zip, string countryCode = "us", TemperatureUnit temperatureUnit = TemperatureUnit.Imperial)
		{
			if(string.IsNullOrEmpty(zip))
			{
				throw new ArgumentNullException(nameof(zip));
			}

			var parameters = new Dictionary<string, string>()
			{
				{ "zip", $"{zip},{countryCode}" },
				{ "units", temperatureUnit.ToString() }
			};

			HttpResponseMessage response;

			using(_client)
			{
				response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, BuildUri(@"https://api.openweathermap.org/data/2.5/weather", parameters)));
			}

			if (response.IsSuccessStatusCode)
			{
				return CurrentWeatherResponse.FromJson(await response.Content.ReadAsStringAsync());
			}

			throw new Exception(await response.Content.ReadAsStringAsync());
		}

		/// <summary>
		/// Get Current Weather using longitude and latitude
		/// </summary>
		/// <param name="longitude">Longitude</param>
		/// <param name="latitude">Latitude</param>
		/// <param name="temperatureUnit">Temperature Unit - Imperial by default.</param>
		/// <returns></returns>
		public async Task<CurrentWeatherResponse> GetCurrentWeatherByCoords(double longitude, double latitude, TemperatureUnit temperatureUnit = TemperatureUnit.Imperial)
		{
			var parameters = new Dictionary<string, string>()
			{
				{ "lat", $"{latitude}" },
				{ "lon", $"{longitude}" },
				{ "units", temperatureUnit.ToString() }
			};

			HttpResponseMessage response;

			using (_client)
			{
				response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, BuildUri(@"https://api.openweathermap.org/data/2.5/weather", parameters)));
			}

			if (response.IsSuccessStatusCode)
			{
				return CurrentWeatherResponse.FromJson(await response.Content.ReadAsStringAsync());
			}

			throw new Exception(await response.Content.ReadAsStringAsync());
		}

		/// <summary>
		/// Get Current weather using a city name
		/// </summary>
		/// <param name="city">City Name</param>
		/// <param name="state">Optional: State Abbreviation (Only available for United States)</param>
		/// <param name="country">Optional: ISO 3166 Country Code</param>
		/// <returns></returns>
		public async Task<CurrentWeatherResponse> GetCurrentWeatherByCityName(string city, string state =  "", string country = "")
        {
			if(string.IsNullOrWhiteSpace(city))
            {
				throw new ArgumentNullException(nameof(city));
            }

			var parameters = $"?q={city}";

			if(!string.IsNullOrWhiteSpace(state))
            {
				parameters += $",{state}";
            }

			if (!string.IsNullOrWhiteSpace(country))
			{
				parameters += $",{country}";
			}

			parameters += $"&appId={_apiKey}";

			HttpResponseMessage response;

			using(_client)
            {
				response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $@"https://api.openweathermap.org/data/2.5/weather{parameters}"));
            }

			if (response.IsSuccessStatusCode)
			{
				return CurrentWeatherResponse.FromJson(await response.Content.ReadAsStringAsync());
			}

			throw new Exception(await response.Content.ReadAsStringAsync());
		}

		/// <summary>
		/// Get Weather based on City Id.  City ID's available at http://bulk.openweathermap.org/sample/
		/// </summary>
		/// <param name="id">City Id</param>
		/// <returns></returns>
		public async Task<CurrentWeatherResponse> GetCurrentWeatherByCityId(int id)
        {
			if (id < 1)
            {
				throw new ArgumentException(nameof(id));
            }

			var parameters = new Dictionary<string, string>()
			{
				{ "id", $"{id}" }
			};

			HttpResponseMessage response;

			using (_client)
			{
				response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, BuildUri(@"https://api.openweathermap.org/data/2.5/weather", parameters)));
			}

			if (response.IsSuccessStatusCode)
			{
				return CurrentWeatherResponse.FromJson(await response.Content.ReadAsStringAsync());
			}

			throw new Exception(await response.Content.ReadAsStringAsync());
		}
		private Uri BuildUri(string baseUrl, Dictionary<string, string> parameters)
		{
			var sb = new StringBuilder();
			sb.Append($"{ baseUrl}?");

			foreach(var parameter in parameters)
			{
				sb.Append($"{parameter.Key}={parameter.Value}&");
			}

			sb.Append($"appid={_apiKey}");

			return new Uri(sb.ToString().ToLower());
		}
	}
}