using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace SeasonalWeather.Utils;

internal static class LogUtility
{
    private static readonly FieldInfo fiUsedKeys = AccessTools.Field(typeof(Log), "usedKeys");
    private static readonly HashSet<int> usedKeys = (HashSet<int>)fiUsedKeys.GetValue(null);

    // Verse.Log
    public static void MessageOnce(string text, int key)
    {
        if (!usedKeys.Add(key))
        {
            return;
        }

        Log.Message(text);
    }
}