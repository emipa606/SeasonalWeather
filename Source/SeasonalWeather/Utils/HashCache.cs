using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SeasonalWeather.Utils;

/// <summary>
///     provides IsHashIntervalTick implementations with caching
/// </summary>
internal static class HashCache
{
    private static readonly Dictionary<string, int> hashCache = new Dictionary<string, int>();

    public static bool IsHashIntervalTick(this DateNotifier dn, int interval)
    {
        return dn.HashOffsetTicks() % interval == 0;
    }

    private static int HashOffsetTicks(this DateNotifier dn)
    {
        return Find.TickManager.TicksGame + dn.GetHashOffset();
    }

    private static int GetHashOffset(this DateNotifier dn)
    {
        if (hashCache.TryGetValue(dn.ToString(), out var val))
        {
            return val;
        }

        val = dn.GetHashCode().HashOffset();
        hashCache.Add(dn.ToString(), val);

        return val;
    }

    private static int HashOffsetTicks(this WeatherWorker w)
    {
        return Find.TickManager.TicksGame + w.GetHashOffset();
    }

    public static bool IsHashIntervalTick(this WeatherWorker w, int interval)
    {
        return w.HashOffsetTicks() % interval == 0;
    }

    private static int GetHashOffset(this WeatherWorker ww)
    {
        if (hashCache.TryGetValue(ww.ToString(), out var val))
        {
            return val;
        }

        val = ww.GetHashCode().HashOffset();
        hashCache.Add(ww.ToString(), val);

        return val;
    }
}