using HarmonyLib;
using RimWorld;
using Verse;

namespace SeasonalWeather;

[StaticConstructorOnStartup]
internal class WeatherSoothe
{
    static WeatherSoothe()
    {
        var harmony = new Harmony("rimworld.whyisthat.seasonalweather.weathersoothe");
        harmony.Patch(AccessTools.Method(typeof(WeatherDecider), nameof(WeatherDecider.StartNextWeather)), null,
            new HarmonyMethod(typeof(WeatherSoothe), nameof(StartNextWeatherPostfix)));

        // NOTE: why am I patching this here?
#if DEBUG
            harmony.Patch(AccessTools.Method(typeof(WeatherDebugActionFix), nameof(WeatherDebugActionFix.Postfix)),
                null, new HarmonyMethod(typeof(WeatherSoothe), nameof(StartNextWeatherPostfix)));
#endif
    }

    // NOTE: avoiding use of instance here to add debug compatibility.
    public static void StartNextWeatherPostfix()
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
        var gameCondition_WeatherEmanation =
            (GameCondition_WeatherEmanation)GameConditionMaker.MakeCondition(def,
                Traverse.Create(map.weatherDecider).Field("curWeatherDuration").GetValue<int>());
        gameCondition_WeatherEmanation.weatherDroneLevel = def.weatherDroneLevel;
        Find.CurrentMap.gameConditionManager.RegisterCondition(gameCondition_WeatherEmanation);
        Find.LetterStack.ReceiveLetter(def.label, def.description, LetterDefOf.PositiveEvent);
    }
}