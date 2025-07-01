using HarmonyLib;
using RimWorld;

namespace SeasonalWeather;

[HarmonyPatch(typeof(Plant), nameof(Plant.GrowthRate), MethodType.Getter)]
public static class Plant_GrowthRate
{
    // NOTE: consider other ways of doing this math...
    public static void Postfix(Plant __instance, ref float __result)
    {
        if (__instance.Map.roofGrid.Roofed(__instance.Position))
        {
            return;
        }

        __result *= 1f + (__instance.Map.weatherManager.curWeather.rainRate * 0.2f);
    }
}