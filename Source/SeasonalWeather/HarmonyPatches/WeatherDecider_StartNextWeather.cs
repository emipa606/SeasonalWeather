using HarmonyLib;
using RimWorld;
using Verse;

namespace SeasonalWeather;

[HarmonyPatch(typeof(WeatherDecider), nameof(WeatherDecider.StartNextWeather))]
public static class WeatherDecider_StartNextWeather
{
    // NOTE: avoiding use of instance here to add debug compatibility.
    public static void Postfix()
    {
        // NOTE: there should be a better home for this code...
        var map = Find.CurrentMap;
        var curWeather = map.weatherManager.curWeather;
        if (curWeather.favorability != Favorability.VeryGood)
        {
            return;
        }

        // NOTE: look up weather condition def based on weather name.
        // NOTE: if more of these are created, consider a better location
        var def = DefDatabase<WeatherConditionDef>.GetNamed(curWeather.defName);
        var gameConditionWeatherEmanation =
            (GameCondition_WeatherEmanation)GameConditionMaker.MakeCondition(def,
                Traverse.Create(map.weatherDecider).Field("curWeatherDuration").GetValue<int>());
        gameConditionWeatherEmanation.weatherDroneLevel = def.weatherDroneLevel;
        Find.CurrentMap.gameConditionManager.RegisterCondition(gameConditionWeatherEmanation);
        Find.LetterStack.ReceiveLetter(def.label, def.description, LetterDefOf.PositiveEvent);
    }
}