using RimWorld;
using Verse;

namespace SeasonalWeather;

public class NaturalDisaster : GameCondition
{
    public override float AnimalDensityFactor(Map m)
    {
        return 0.1f;
    }

    public override float PlantDensityFactor(Map m)
    {
        return 0.4f;
    }

    public override bool AllowEnjoyableOutsideNow(Map m)
    {
        return false;
    }
}