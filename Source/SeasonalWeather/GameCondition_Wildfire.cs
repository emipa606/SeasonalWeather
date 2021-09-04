using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace SeasonalWeather
{
    public class GameCondition_Wildfire : NaturalDisaster
    {
        private static readonly IntRange TicksBetweenFires = new IntRange(320, 800);
        private static readonly bool noFirewatcher;
        private readonly SkyColorSet AshCloudColors;

        private readonly List<SkyOverlay> overlays;
        private Rot4 direction;
        private int fires;
        private int nextFireTicks;
        private bool seedingFires = true;

        static GameCondition_Wildfire()
        {
            noFirewatcher = ModLister.AllInstalledMods.FirstOrDefault(m => m.Name == "No Firewatcher")?.Active == true;
        }

        public GameCondition_Wildfire()
        {
            AshCloudColors = new SkyColorSet(new ColorInt(216, 255, 150).ToColor, new ColorInt(234, 200, 255).ToColor,
                new Color(0.7f, 0.85f, 0.65f), 0.85f);
            overlays = new List<SkyOverlay> { new WeatherOverlay_DustCloud() };
        }

        public override List<SkyOverlay> SkyOverlays(Map map)
        {
            return overlays;
        }

        public override float SkyTargetLerpFactor(Map map)
        {
            return GameConditionUtility.LerpInOutValue(this, 5000f, 0.5f);
        }

        public override SkyTarget? SkyTarget(Map map)
        {
            return new SkyTarget(0.85f, AshCloudColors, 1f, 1f);
        }

        public override void Init()
        {
            base.Init();
            var range = new IntRange((int)(SingleMap.Size.x * 0.23f), (int)(SingleMap.Size.x * 0.4f));
            fires = range.RandomInRange;
            Log.Message($"{fires}");
            // how to find out if this side is a mountain face?
            direction = Rot4.Random;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref direction, "direction");
            Scribe_Values.Look(ref nextFireTicks, "nextFireTicks");
        }

        public override void GameConditionTick()
        {
            if (seedingFires)
            {
                if (Find.TickManager.TicksGame > nextFireTicks)
                {
                    SpawnFire(SingleMap);
                    nextFireTicks = Find.TickManager.TicksGame + TicksBetweenFires.RandomInRange;
                    fires--;
                    if (fires <= 0)
                    {
                        seedingFires = false;
                    }
                }
            }
            else
            {
                if (noFirewatcher)
                {
                    SingleMap.fireWatcher.FireWatcherTick();
                }

                if (!SingleMap.fireWatcher.LargeFireDangerPresent)
                {
                    Duration = 0; // Expired => true
                }
            }

            foreach (var skyOverlay in overlays)
            {
                skyOverlay.TickOverlay(SingleMap);
            }
        }

        public override void GameConditionDraw(Map map)
        {
            foreach (var skyOverlay in overlays)
            {
                skyOverlay.DrawOverlay(map);
            }
        }

        private void SpawnFire(Map map)
        {
            var cell = CellFinder.RandomEdgeCell(direction, map);
            var fire = (Fire)ThingMaker.MakeThing(ThingDefOf.Fire);
            fire.fireSize = 1.0f;
            GenSpawn.Spawn(fire, cell, map, Rot4.North);
        }
    }
}