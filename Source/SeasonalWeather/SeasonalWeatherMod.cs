using SettingsHelper;
using UnityEngine;
using Verse;

namespace SeasonalWeather
{
    internal class SeasonalWeatherMod : Mod
    {
        public static SeasonalWeatherSettings settings;

        public SeasonalWeatherMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<SeasonalWeatherSettings>();
        }

        public override string SettingsCategory()
        {
            return "SeasonalWeatherSettingsCategoryLabel".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.AddLabeledCheckbox("EnableEarthquakesLabel".Translate() + ": ",
                ref settings.enableEarthquakes);
            listing_Standard.AddLabeledCheckbox("EnableWildfiresLabel".Translate() + ": ",
                ref settings.enableWildfires);
            //listing_Standard.AddLabelLine("DynamicDefNote".Translate());
            listing_Standard.End();
            settings.Write();
        }
    }
}