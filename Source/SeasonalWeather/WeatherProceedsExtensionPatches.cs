using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SeasonalWeather;

[StaticConstructorOnStartup]
internal class WeatherProceedsExtensionPatches
{
    static WeatherProceedsExtensionPatches()
    {
        new Harmony("rimworld.whyisthat.seasonalweather.weatherproceedsextension").Patch(
            AccessTools.Method(typeof(WeatherDecider), "CurrentWeatherCommonality"), null, null,
            new HarmonyMethod(typeof(WeatherProceedsExtensionPatches), nameof(Transpiler)));
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        var instructionList = instructions.ToList();
        int i;
        for (i = 0; i < instructionList.Count; i++)
        {
            yield return instructionList[i];
            if (instructionList[i].opcode != OpCodes.Stloc_0)
            {
                continue;
            }

            yield return new CodeInstruction(OpCodes.Ldarg_0);
            yield return new CodeInstruction(OpCodes.Ldfld, typeof(WeatherManager).GetField("map"));
            yield return new CodeInstruction(OpCodes.Ldarg_1);
            yield return new CodeInstruction(OpCodes.Call,
                typeof(FollowsRainHelper).GetMethod(nameof(FollowsRainHelper.FollowsRain)));
            var @continue = il.DefineLabel();
            yield return new CodeInstruction(OpCodes.Brtrue, @continue);
            yield return new CodeInstruction(OpCodes.Ldc_R4, 0.0f);
            yield return new CodeInstruction(OpCodes.Ret);
            instructionList[++i].labels.Add(@continue);
            yield return instructionList[i];
            // NOTE: could break here for avoiding future check...
        }
    }
}