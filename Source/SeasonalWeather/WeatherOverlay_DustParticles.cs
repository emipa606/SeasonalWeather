using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SeasonalWeather
{
    [StaticConstructorOnStartup]
    public class WeatherOverlay_DustParticles : SkyOverlay
    {
        //private static readonly Material DustParticlesOverlay = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1); 
        private static readonly Material DustParticlesOverlay;

        static WeatherOverlay_DustParticles()
        {
            DustParticlesOverlay = (Material)AccessTools.Field(typeof(WeatherOverlay_SnowHard), "SnowOverlayWorld")
                .GetValue(new WeatherOverlay_SnowHard());
        }

        public WeatherOverlay_DustParticles()
        {
            worldOverlayMat = DustParticlesOverlay;
            worldOverlayPanSpeed1 = 0.018f;
            worldPanDir1 = new Vector2(-1f, -0.26f); //new Vector2(1f, 1f);
            worldPanDir1.Normalize();
            worldOverlayPanSpeed2 = 0.022f;
            worldPanDir2 = new Vector2(-1f, -0.24f); //new Vector2(1f, -1f);
            worldPanDir2.Normalize();
        }
    }
}