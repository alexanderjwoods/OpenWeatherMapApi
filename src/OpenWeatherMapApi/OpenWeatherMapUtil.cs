using OpenWeatherMapApi.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeatherMapApi
{
	/// <summary>
	/// Utilities for OpenWeatherMap.
	/// </summary>
	class Util
	{
		/// <summary>
		/// Converter to convert CurrentWeatherResponse's to string[]'s.
		/// </summary>
		public class Converter
		{
			/// <summary>
			/// Converts your weather response to a string[] with all information. View the code for the order the array is built.
			/// </summary>
			/// <param name="weatherresponse">Your weather response</param>
			/// <returns>string[]</returns>
			public string[] Convert(CurrentWeatherResponse weatherresponse)
			{
				List<string> w = new List<string>();
				/// Weather comes first in the list.
				foreach (Weather we in weatherresponse.Weather)
				{
					w.Add(we.Main.ToString());
				}
				/// Temperature comes second.
				w.Add(weatherresponse.Main.Temp.Value.ToString());
				/// TempMin comes third.
				w.Add(weatherresponse.Main.TempMin.Value.ToString());
				/// TempMax comes fourth.
				w.Add(weatherresponse.Main.TempMax.Value.ToString());
				/// FeelsLike comes fifth.
				w.Add(weatherresponse.Main.FeelsLike.Value.ToString());
				/// Wind speed comes sixth.
				w.Add(weatherresponse.Wind.Speed.Value.ToString());
				/// Wind gust comes seventh.
				w.Add(weatherresponse.Wind.Gust.Value.ToString());
				/// Rain 1H comes eighth.
				w.Add(weatherresponse.Rain.The1H.Value.ToString());
				/// Timezone comes ninth.
				w.Add(weatherresponse.Timezone.Value.ToString());
				/// Latitude comes tenth.
				w.Add(weatherresponse.Coord.Lat.Value.ToString());
				/// Longitude comes eleventh.
				w.Add(weatherresponse.Coord.Lon.Value.ToString());
				/// Finally, after the list is built, this method returns an array.
				return w.ToArray();
			}

		}
		/// <summary>
		/// Finds coords from CurrentWeatherResponse's.
		/// </summary>
		public class CoordFinder
        {
			/// <summary>
			/// Finds coords from CurrentWeatherResponse's. View the code for the order the array is built.
			/// </summary>
			/// <param name="response">Your CurrentWeatherResponse</param>
			/// <returns>double[]</returns>
			public double[] Find(CurrentWeatherResponse response)
            {
				List<double> coordlist = new List<double>();
				/// Latitude comes first in the list.
				coordlist.Add(response.Coord.Lat.Value);
				/// Longitude comes second.
				coordlist.Add(response.Coord.Lon.Value);
				/// Finally, after the list is built, this method returns an array.
				return coordlist.ToArray();
            }
        }
		public class Tester
        {
			public string TestIfWorks(CurrentWeatherResponse response)
            {
				try
                {
					if (response != null)
                    {
						return "Y";
                    }
					else
                    {
						return "N";
                    }
                }
				catch
                {
					return "N";
                }
            }
        }
	}
}
