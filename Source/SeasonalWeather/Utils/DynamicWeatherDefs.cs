using Verse;

namespace SeasonalWeather.Utils;

public static class DynamicWeatherDefs
{
    public static DynamicWeatherDefHelper dynamicWeatherDefHelper = new DynamicWeatherDefHelper();

    public class DynamicWeatherDefHelper : DynamicDefHelper<WeatherDef>
    {
    }
}