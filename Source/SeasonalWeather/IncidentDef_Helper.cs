using RimWorld;
using UnityEngine;

namespace SeasonalWeather
{
    internal static class IncidentDef_Helper
    {
        public static int GetDuration(this IncidentDef def)
        {
            return Mathf.RoundToInt(def.durationDays.RandomInRange * 60000f);
        }
    }
}