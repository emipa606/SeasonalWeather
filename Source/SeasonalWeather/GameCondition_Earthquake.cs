using UnityEngine;
using Verse;

namespace SeasonalWeather
{
    public class GameCondition_Earthquake : NaturalDisaster
    {
        private static readonly IntRange TicksBetweenTremors = new IntRange(800, 1800);
        private float magnitude;
        private int nextTremorTicks;

        // sets magnitude based on points
        public float Magnitude
        {
            set => magnitude = value;
        } // get?

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref nextTremorTicks, "nextTremorTicks");
        }

        public override void Init()
        {
            base.Init();
            Foreshock();
        }

        // NOTE: provides a kind of warning before things get too bad.
        private void Foreshock()
        {
            Find.CameraDriver.shaker.DoShake(magnitude / 2.0f);
        }

        public override void GameConditionTick()
        {
            base.GameConditionTick();
            if (Find.TickManager.TicksGame <= nextTremorTicks)
            {
                return;
            }

            SingleMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_Tremor(SingleMap));
            nextTremorTicks =
                Mathf.FloorToInt((Find.TickManager.TicksGame + TicksBetweenTremors.RandomInRange) / magnitude);
        }
    }
}