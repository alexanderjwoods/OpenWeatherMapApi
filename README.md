<div align="center">
# OpenWeatherMapApi
Classes to access and consume the Open Weather Map Api, found at https://www.openweathermap.org/api

## Usage
Using this package is very simple.  Set up a client using your api key, and call the method you need.

```C#
var apikey = new OpenWeatherMapAPIKey("YOURAPIKEY");
var client = new OpenWeatherMapClient(apikey);
CurrentWeatherResponse response = await client.GetCurrentWeatherByZip("12345");
```
That's it!

#### Dev Notes
Endpoints currently supported:
	City Name
	Coordinates
	Zip Code
	City Code
	
Language Support and XML support coming soon!

#### Contributers
If you would like to contribute, simply send a PR!
</div>
