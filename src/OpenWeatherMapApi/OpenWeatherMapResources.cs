using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeatherMapApi
{
	public class APIKeys
	{
		public struct OpenWeatherMapAPIKey
		{
			private string KeyID { get; set; }
			public OpenWeatherMapAPIKey(string id)
			{
				KeyID = id;
			}
			public class KeyUtils
            {
				public static void DisposeKey(OpenWeatherMapAPIKey id)
                {
					id.KeyID = string.Empty;
                }
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
