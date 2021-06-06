using Verse;

namespace SeasonalWeather
{
    internal class FollowsRainHelper
    {
        //check that all predicates are true...
        public static bool FollowsRain(Map map, WeatherDef weather)
        {
            var ext = weather.GetModExtension<FollowsRainExtension>();
            if (ext != null && ext.followsRain && map.weatherManager.lastWeather.rainRate <= 0.1f)
            {
                return false; // CurrentWeatherCommonality => defines `rain` vs `sprinkle`
            }

            return true;
        }
    }
}