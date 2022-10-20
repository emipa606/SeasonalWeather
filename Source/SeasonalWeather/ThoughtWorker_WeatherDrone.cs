using System;
using RimWorld;
using Verse;

namespace SeasonalWeather;

public class ThoughtWorker_WeatherDrone : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
        var weatherDroneLevel = WeatherDroneLevel.None;
        var activeCondition = p.Map.gameConditionManager.GetActiveCondition<GameCondition_WeatherEmanation>();
        if (activeCondition != null && activeCondition.weatherDroneLevel > weatherDroneLevel)
        {
            weatherDroneLevel = activeCondition.weatherDroneLevel;
        }

        switch (weatherDroneLevel)
        {
            case WeatherDroneLevel.None:
                return false;
            case WeatherDroneLevel.GoodLow:
                return ThoughtState.ActiveAtStage(0);
            default:
                throw new NotImplementedException();
        }
    }
}