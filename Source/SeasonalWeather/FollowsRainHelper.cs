using Verse;

namespace SeasonalWeather;

internal class FollowsRainHelper
{
    //check that all predicates are true...
    public static bool FollowsRain(Map map, WeatherDef weather)
    {
        var ext = weather.GetModExtension<FollowsRainExtension>();
        return ext is not { followsRain: true } || !(map.weatherManager.lastWeather.rainRate <= 0.1f);
        // CurrentWeatherCommonality => defines `rain` vs `sprinkle`
    }
}