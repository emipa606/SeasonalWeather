using System.Collections.Generic;
using HarmonyLib;
using Verse;

// NOTE: no longer used but worth keeping around.
namespace SeasonalWeather.Utils;

public abstract class DynamicDefHelper<T> where T : Def, new()
{
    private readonly Dictionary<string, T> defs = new Dictionary<string, T>();

    public void SetDynamicDet(string defName, bool add = false)
    {
        if (add)
        {
            if (DefDatabase<T>.GetNamed(defName, false) != null)
            {
                return;
            }

            // Add def and references
            defs.TryGetValue(defName, out var def);
            DefDatabase<T>.Add(def);
        }
        else
        {
            if (DefDatabase<WeatherDef>.GetNamed(defName, false) == null)
            {
                return;
            }

            // Remove def and references
            var def = DefDatabase<T>.GetNamed(defName);
            defs.Add(defName, def);
            AccessTools.Method(typeof(DefDatabase<T>), "Remove").Invoke(null, new object[] { def });
        }
    }
}