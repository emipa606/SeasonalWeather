using Mlie;
using UnityEngine;
using Verse;

namespace SeasonalWeather;

internal class SeasonalWeatherMod : Mod
{
    public static SeasonalWeatherSettings settings;
    private static string currentVersion;

    public SeasonalWeatherMod(ModContentPack content) : base(content)
    {
        settings = GetSettings<SeasonalWeatherSettings>();
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override string SettingsCategory()
    {
        return "SeasonalWeatherSettingsCategoryLabel".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(inRect);
        listing_Standard.CheckboxLabeled("EnableEarthquakesLabel".Translate() + ": ",
            ref settings.enableEarthquakes);
        listing_Standard.CheckboxLabeled("EnableWildfiresLabel".Translate() + ": ",
            ref settings.enableWildfires);
        //listing_Standard.AddLabelLine("DynamicDefNote".Translate());
        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("Seasonal_CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }
}