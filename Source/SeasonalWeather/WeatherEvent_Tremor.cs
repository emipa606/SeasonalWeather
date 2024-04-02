using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace SeasonalWeather;

public class WeatherEvent_Tremor : WeatherEvent
{
    private static float noise;
    private static readonly HediffDef shredded = HediffDef.Named("Shredded");
    private readonly float magnitude;
    private readonly float xScale = 2.0f;
    private IntVec3 curPos;
    private Rot4 direction;
    private bool expired;

    private float time; // avoiding int to save from casting

    public WeatherEvent_Tremor(Map map) : this(map, 1.0f)
    {
    }

    private WeatherEvent_Tremor(Map map, float magnitude) : base(map)
    {
        this.map = map;
        this.magnitude = magnitude;
        xScale *= this.magnitude;
        curPos = CellFinder.RandomCell(this.map);
        direction = Rot4.Random;
        time = Find.TickManager.TicksGame;
    }

    public override bool Expired => expired;

    private float MinLoopNoiseThreshold => magnitude * 0.25f;

    private float ExpireNoiseThreshold => magnitude * 0.1f;

    // NOTE: sure what should go here yet.
    public override void FireEvent()
    {
        Find.CameraDriver.shaker.DoShake(magnitude);
        SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(map);
    }

    public override void WeatherEventTick()
    {
        do
        {
            CreateFaultCell(curPos);
            TremorWalk();
            if (!curPos.InBounds(map))
            {
                expired = true;
                return;
            }

            GetNoise(out noise);
        } while (noise > MinLoopNoiseThreshold);

        if (noise < ExpireNoiseThreshold)
        {
            expired = true;
        }
    }

    private void GetNoise(out float localNoise)
    {
        localNoise = Mathf.PerlinNoise(time, 0.0f);
        time += xScale;
    }

    private void TremorWalk()
    {
        GetNoise(out var localNoise);
        switch (localNoise)
        {
            case < 0.25f:
            case < 0.5f:
                direction.Rotate(RotationDirection.Clockwise);
                break;
        }

        curPos += direction.FacingCell;
    }

    private void CreateFaultCell(IntVec3 cell)
    {
        var roofCollapsed = cell.Roofed(map);

        var things = cell.GetThingList(map).ToArray();
        foreach (var building in things.OfType<Building>())
        {
            if (!building.def.destroyable || building.Destroyed)
            {
                continue;
            }

            if (building.def.holdsRoof || building.def.building.isNaturalRock)
            {
                roofCollapsed = false;
                continue;
            }

            var compRefuel = building.TryGetComp<CompRefuelable>();
            if (compRefuel is { HasFuel: true })
            {
                FireUtility.TryStartFireIn(cell, map, 3.0f * compRefuel.Fuel, null);
            }
            else
            {
                var compPower = building.TryGetComp<CompPower>();
                if (compPower != null
                   ) // consider more specific fires for different types. (explosions for power plants)
                {
                    FireUtility.TryStartFireIn(cell, map, 2.0f, null);
                }
            }

            building.Destroy();
        }

        foreach (var pawn in things.OfType<Pawn>())
        {
            if (roofCollapsed)
            {
                HediffGiverUtility.TryApply(pawn, shredded, null, true, 3);
            }
            else
            {
                HediffGiverUtility.TryApply(pawn, shredded, null, true);
            }
        }

        // TODO: consider type of roof in the drop.
        if (roofCollapsed)
        {
            map.roofCollapseBuffer.MarkToCollapse(cell);
            map.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
        }

        map.terrainGrid.RemoveTopLayer(cell, false);
        FilthMaker.TryMakeFilth(cell, map, ThingDefOf.Filth_RubbleRock);
    }
}