using RimWorld;
using UnityEngine;
using Verse;

namespace SeasonalWeather
{
    public class IncidentWorker_Earthquake : IncidentWorker
    {
        private int duration;

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return SeasonalWeatherMod.settings.enableEarthquakes
                   && !((Map) parms.target).gameConditionManager.ConditionIsActive(GameConditionDefOf.Earthquake);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            var map = (Map) parms.target;
            var points = parms.points;
            var richterMagnitude = EarthquakeHelper.GetMagnitudeWithRand(points);
            GetDuration(richterMagnitude);
            var gameCondition_Earthquake =
                (GameCondition_Earthquake) GameConditionMaker.MakeCondition(GameConditionDefOf.Earthquake, duration);
            gameCondition_Earthquake.Magnitude = richterMagnitude;
            map.gameConditionManager.RegisterCondition(gameCondition_Earthquake);
            return true;
        }

        // multiply duration by magnitude
        private void GetDuration(float magnitude)
        {
            duration = Mathf.CeilToInt(def.GetDuration() * magnitude);
        }
    }
}