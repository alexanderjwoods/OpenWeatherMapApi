﻿using OpenWeatherMapApi.Responses;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMapApi
{
	public class OpenWeatherMapClient
	{
		private readonly string _apiKey;
		private readonly HttpClient _client;

		/// <summary>
		/// Initializes a client to retireve data from OpenWeaterMap
		/// </summary>
		/// <param name="apiKey">Your OpenWeatherMap Api Key</param>
		public OpenWeatherMapClient(string apiKey, HttpClient client = null)
		{
			_apiKey = string.IsNullOrEmpty(apiKey) ? throw new ArgumentNullException(nameof(apiKey)) : apiKey;
			_client = client ?? new HttpClient();
		}

		/// <summary>
		/// Get Current Weather using a zip code
		/// </summary>
		/// <param name="zip">Zip code for current weather</param>
		/// <param name="countryCode">Optional: Country Code corresponding to zip.  US by default.</param>
		public async Task<CurrentWeatherResponse> GetCurrentWeatherByZip(string zip, string countryCode = "us")
		{
			if(string.IsNullOrEmpty(zip))
			{
				throw new ArgumentNullException(nameof(zip));
			}

			var parameters = new Dictionary<string, string>()
			{
				{ "zip", $"{zip},{countryCode}" }
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

		private Uri BuildUri(string baseUrl, Dictionary<string, string> parameters)
		{
			var sb = new StringBuilder();
			sb.Append($"{baseUrl}?");

			foreach(var parameter in parameters)
			{
				sb.Append($"{parameter.Key}={parameter.Value}&");
			}

			sb.Append($"appid={_apiKey}");

			return new Uri(sb.ToString());
		}
	}
}