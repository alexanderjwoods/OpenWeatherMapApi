using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeatherMapApi
{
	public class Keys
	{
		public struct OpenWeatherMapAPIKey
		{
			public string KeyID { get; set; }
			public OpenWeatherMapAPIKey(string id)
			{
				KeyID = id;
			}
			public class PreparationEngine
			{
				public static string PrepareClientAPIKey(OpenWeatherMapAPIKey id)
				{
					return Convert.ToString(id.KeyID);
				}
			}
		}
	}
}
