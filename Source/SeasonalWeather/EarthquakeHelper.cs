using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

// NOTE: consider using factorials with magnitude?
// NOTE: Mercalli intensity scale
// NOTE: consider animal reaction
// NOTE: consider earthquake swarms

namespace SeasonalWeather;
// REFERNECE: https://en.wikipedia.org/wiki/Richter_magnitude_scale

internal static class EarthquakeHelper
{
    // REFERNECE: https://stackoverflow.com/questions/20147879/switch-case-can-i-use-a-range-instead-of-a-one-number
    private static readonly List<KeyValuePair<Func<float, bool>, RichterMagnitude>> richterSwitch =
    [
        new(x => x < 1.0f, RichterMagnitude.Undefined),
        new(x => x < 2.0f, RichterMagnitude.Micro),
        new(x => x < 4.0f, RichterMagnitude.Minor),
        new(x => x < 5.0f, RichterMagnitude.Light),
        new(x => x < 6.0f, RichterMagnitude.Moderate),
        new(x => x < 7.0f, RichterMagnitude.Strong),
        new(x => x < 8.0f, RichterMagnitude.Major),
        new(_ => true, RichterMagnitude.Great)
    ];

    private static readonly SimpleCurve curveMagnitudeScale =
    [
        new CurvePoint(0f, 1.0f),
        new CurvePoint(10f, 2.0f),
        new CurvePoint(100f, 3.0f),
        new CurvePoint(500f, 4.0f),
        new CurvePoint(1000f, 5.0f),
        new CurvePoint(1500f, 6.0f),
        new CurvePoint(2000f, 7.0f)
    ];

    public static RichterMagnitude GetMagnitudeType(float magnitude)
    {
        return richterSwitch.First(sw => sw.Key(magnitude)).Value;
    }

    private static float getMagnitude(float points)
    {
        return curveMagnitudeScale.Evaluate(points) + Rand.Range(0f, 1f);
    }

    public static float GetMagnitudeWithRand(float points)
    {
        return getMagnitude(points) + Rand.Range(0f, 1f);
    }
}