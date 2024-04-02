using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SeasonalWeather;

public class SeasonalWeatherExtension : DefModExtension
{
    private readonly List<WeatherCommonalityRecord> fall = [];
    private readonly List<WeatherCommonalityRecord> spring = [];
    private readonly List<WeatherCommonalityRecord> summer = [];
    private readonly List<WeatherCommonalityRecord> winter = [];

    public void AdjustBaseWeatherCommonalities(Map map, Season season)
    {
        Log.Message("SeasonalWeather: adjusting baseWeatherCommonalities");
        switch (season)
        {
            case Season.Spring:
                map.Biome.baseWeatherCommonalities = spring;
                break;
            case Season.Summer:
            case Season.PermanentSummer:
                map.Biome.baseWeatherCommonalities = summer;
                break;
            case Season.Fall:
                map.Biome.baseWeatherCommonalities = fall;
                break;
            case Season.Winter:
            case Season.PermanentWinter:
                map.Biome.baseWeatherCommonalities = winter;
                break;
        }
    }
}