using Verse;

namespace SeasonalWeather;

public class SeasonalWeatherSettings : ModSettings
{
    public bool EnableEarthquakes = true;
    public bool EnableWildfires = true;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref EnableEarthquakes, "enableEarthquakes", true);
        Scribe_Values.Look(ref EnableWildfires, "enableWildfires", true);
    }
}