using System.Reflection;
using HarmonyLib;
using Mlie;
using RimWorld;
using SeasonalWeather.Utils;
using UnityEngine;
using Verse;

namespace SeasonalWeather;

internal class SeasonalWeatherMod : Mod
{
    public static SeasonalWeatherSettings Settings;
    private static string currentVersion;

    private static readonly MethodInfo miFindPlayerHomeWithMinTimezone =
        AccessTools.Method(typeof(DateNotifier), "FindPlayerHomeWithMinTimezone");

    public static readonly FieldInfo FiLastSeason = AccessTools.Field(typeof(DateNotifier), "lastSeason");

    public SeasonalWeatherMod(ModContentPack content) : base(content)
    {
        Settings = GetSettings<SeasonalWeatherSettings>();
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
            ref Settings.EnableEarthquakes);
        listing_Standard.CheckboxLabeled("EnableWildfiresLabel".Translate() + ": ",
            ref Settings.EnableWildfires);
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


    public static void CheckBaseWeatherCommonalities(DateNotifier instance)
    {
        var map = (Map)miFindPlayerHomeWithMinTimezone.Invoke(instance, []);
        if (map == null)
        {
            LogUtility.MessageOnce("No map found to check base weather commonalities? NomadsLand?", 8720412);
            return;
        }

        var ext = map.Biome.GetModExtension<SeasonalWeatherExtension>();
        if (ext == null)
        {
            LogUtility.MessageOnce("Custom biome does not have Seasonal Weather data.", map.Biome.GetHashCode());
            return;
        }


        Log.Message("SeasonalWeather: season changed");
        ext.AdjustBaseWeatherCommonalities(map, getSeason(map));
    }

    private static Season getSeason(Map map)
    {
        var latitude = Find.WorldGrid.LongLatOf(map.Tile).y;
        var longitude = Find.WorldGrid.LongLatOf(map.Tile).x;
        return GenDate.Season(Find.TickManager.TicksAbs, latitude, longitude);
    }
}