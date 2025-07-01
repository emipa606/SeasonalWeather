using HarmonyLib;
using RimWorld;

namespace SeasonalWeather;

[HarmonyPatch(typeof(DateNotifier), nameof(DateNotifier.DateNotifierTick))]
public static class DateNotifier_DateNotifierTick
{
    public static void Prefix(DateNotifier __instance, out Season __state)
    {
        __state = (Season)SeasonalWeatherMod.FiLastSeason.GetValue(__instance);
    }

    public static void Postfix(DateNotifier __instance, Season __state)
    {
        var currentLastSeason = (Season)SeasonalWeatherMod.FiLastSeason.GetValue(__instance);
        if (currentLastSeason == __state)
        {
            return;
        }

        SeasonalWeatherMod.CheckBaseWeatherCommonalities(__instance);
    }
}