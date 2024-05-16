using System.Reflection;
using HarmonyLib;
using RimWorld;
using SeasonalWeather.Utils;
using Verse;

namespace SeasonalWeather;

[StaticConstructorOnStartup]
internal static class SeasonalWeatherExtensionPatches
{
    private static readonly MethodInfo MI_FindPlayerHomeWithMinTimezone =
        AccessTools.Method(typeof(DateNotifier), "FindPlayerHomeWithMinTimezone");

    private static readonly FieldInfo FI_lastSeason = AccessTools.Field(typeof(DateNotifier), "lastSeason");

    static SeasonalWeatherExtensionPatches()
    {
        var harmony = new Harmony("rimworld.whyisthat.seasonalweather.seasonalweatherextension");

        harmony.Patch(AccessTools.Method(typeof(GameComponentUtility), nameof(GameComponentUtility.FinalizeInit)),
            null, new HarmonyMethod(typeof(SeasonalWeatherExtensionPatches), nameof(FinalizeInit)));
        harmony.Patch(AccessTools.Method(typeof(DateNotifier), nameof(DateNotifier.DateNotifierTick)),
            new HarmonyMethod(typeof(SeasonalWeatherExtensionPatches), nameof(DateNotifierTickPrefix)),
            new HarmonyMethod(typeof(SeasonalWeatherExtensionPatches), nameof(DateNotifierTickPostfix)));
    }

    // NOTE: should this be somewhere else?
    public static void FinalizeInit()
    {
        CheckBaseWeatherCommonalities(Find.DateNotifier);
    }

    public static void DateNotifierTickPrefix(DateNotifier __instance, out Season __state)
    {
        __state = (Season)FI_lastSeason.GetValue(__instance);
    }

    public static void DateNotifierTickPostfix(DateNotifier __instance, Season __state)
    {
        var currentLastSeason = (Season)FI_lastSeason.GetValue(__instance);
        if (currentLastSeason == __state)
        {
            return;
        }

        CheckBaseWeatherCommonalities(__instance);
    }

    private static void CheckBaseWeatherCommonalities(DateNotifier __instance)
    {
        var map = (Map)MI_FindPlayerHomeWithMinTimezone.Invoke(__instance, []);
        if (map == null)
        {
            LogUtility.MessageOnce("No map found to check base weather commonalities? NomadsLand?", 8720412);
            return;
        }

        var ext = map.Biome.GetModExtension<SeasonalWeatherExtension>();
        if (ext == null)
        {
            LogUtility.MessageOnce("Custom biome does not have Seasonal Weather data.", map.Biome.GetHashCode());
            return;
        }


        Log.Message("SeasonalWeather: season changed");
        ext.AdjustBaseWeatherCommonalities(map, map.GetSeason());
    }

    private static Season GetSeason(this Map map)
    {
        var latitude = Find.WorldGrid.LongLatOf(map.Tile).y;
        var longitude = Find.WorldGrid.LongLatOf(map.Tile).x;
        return GenDate.Season(Find.TickManager.TicksAbs, latitude, longitude);
    }
}