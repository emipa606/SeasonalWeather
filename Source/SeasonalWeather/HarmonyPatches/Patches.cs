using System.Reflection;
using HarmonyLib;
using Verse;

namespace SeasonalWeather;

[StaticConstructorOnStartup]
public class Patches
{
    static Patches()
    {
        new Harmony("rimworld.whyisthat.seasonalweather.growthrate").PatchAll(Assembly.GetExecutingAssembly());
    }
}