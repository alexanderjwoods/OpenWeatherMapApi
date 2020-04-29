# OpenWeatherMapApi
Classes to access and consume the Open Weather Map Api, found [here](https://openweathermap.org/api)

## Usage
Using this package is very simple.  Set up a client using your api key, and call the method you need.

```C#
var client = new OpenWeatherMapClient("YOURAPIKEY");

CurrentWeatherResponse response = await client.GetCurrentWeatherByZip("12345");
```
That's it!

#### Dev Notes
This package currently only supports retrieving Current Weather by zip code.  It is being used in a personal project, but I do plan on including all available api endpoints in the free version.

#### Contributers
If you would like to contribute, simply send a PR!
