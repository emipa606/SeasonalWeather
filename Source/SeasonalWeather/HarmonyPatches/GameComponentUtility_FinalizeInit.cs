using HarmonyLib;
using Verse;

namespace SeasonalWeather;

[HarmonyPatch(typeof(GameComponentUtility), nameof(GameComponentUtility.FinalizeInit))]
public static class GameComponentUtility_FinalizeInit
{
    // NOTE: should this be somewhere else?
    public static void Postfix()
    {
        SeasonalWeatherMod.CheckBaseWeatherCommonalities(Find.DateNotifier);
    }
}