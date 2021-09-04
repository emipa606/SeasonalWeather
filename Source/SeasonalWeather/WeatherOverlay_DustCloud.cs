using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SeasonalWeather
{
    // NOTE: These broke in A18... but lol this works instead.

    [StaticConstructorOnStartup]
    public class WeatherOverlay_DustCloud : SkyOverlay
    {
        //private static readonly Material DustCloudOverlay = MatLoader.LoadMat("Weather/FogOverlayWorld", -1);
        private static readonly Material DustCloudOverlay;

        static WeatherOverlay_DustCloud()
        {
            DustCloudOverlay = (Material)AccessTools.Field(typeof(WeatherOverlay_Fog), "FogOverlayWorld")
                .GetValue(new WeatherOverlay_Fog());
        }

        public WeatherOverlay_DustCloud()
        {
            worldOverlayMat = DustCloudOverlay;
            worldOverlayPanSpeed1 = 0.008f;
            worldPanDir1 = new Vector2(-1f, -0.26f); //new Vector2(1f, 1f);
            worldPanDir1.Normalize();
            worldOverlayPanSpeed2 = 0.012f;
            worldPanDir2 = new Vector2(-1f, -0.24f); //new Vector2(1f, -1f);
            worldPanDir2.Normalize();
        }
    }
}