using RimWorld;
using Verse;

namespace SeasonalWeather
{
    public class IncidentWorker_Wildfire : IncidentWorker
    {
        private int duration;

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return SeasonalWeatherMod.settings.enableWildfires
                   && !((Map) parms.target).gameConditionManager.ConditionIsActive(GameConditionDefOf.Wildfire);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            var map = (Map) parms.target;
            //float points = parms.points;
            duration = def.GetDuration();
            var gameCondition_Wildfire =
                (GameCondition_Wildfire) GameConditionMaker.MakeCondition(GameConditionDefOf.Wildfire, duration);
            map.gameConditionManager.RegisterCondition(gameCondition_Wildfire);
            return true;
        }
    }
}