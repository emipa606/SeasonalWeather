using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SeasonalWeather.Utils;

/// <summary>
///     provides IsHashIntervalTick implementations with caching
/// </summary>
internal static class HashCache
{
    private static readonly Dictionary<string, int> hashCache = new();

    extension(DateNotifier dn)
    {
        public bool IsHashIntervalTick(int interval)
        {
            return dn.HashOffsetTicks() % interval == 0;
        }

        private int HashOffsetTicks()
        {
            return Find.TickManager.TicksGame + dn.GetHashOffset();
        }

        private int GetHashOffset()
        {
            if (hashCache.TryGetValue(dn.ToString(), out var val))
            {
                return val;
            }

            val = dn.GetHashCode().HashOffset();
            hashCache.Add(dn.ToString(), val);

            return val;
        }
    }
}