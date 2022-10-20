using Verse;

namespace SeasonalWeather;

public class SeasonalWeatherSettings : ModSettings
{
    public bool enableEarthquakes = true;
    public bool enableWildfires = true;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref enableEarthquakes, "enableEarthquakes", true);
        Scribe_Values.Look(ref enableWildfires, "enableWildfires", true);
    }
}