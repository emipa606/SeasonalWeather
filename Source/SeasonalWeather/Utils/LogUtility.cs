using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace SeasonalWeather.Utils;

internal class LogUtility
{
    private static readonly FieldInfo FI_usedKeys = AccessTools.Field(typeof(Log), "usedKeys");
    private static readonly HashSet<int> usedKeys = (HashSet<int>)FI_usedKeys.GetValue(null);

    // Verse.Log
    public static void MessageOnce(string text, int key)
    {
        if (usedKeys.Contains(key))
        {
            return;
        }

        usedKeys.Add(key);
        Log.Message(text);
    }
}