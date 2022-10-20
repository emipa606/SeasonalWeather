namespace SeasonalWeather.Utils;
#if DEBUG
    [StaticConstructorOnStartup]
    internal static class WeatherDebugActionFix
    {
        private static readonly BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Instance;
        private static readonly string varName = "localWeather";

        private static readonly Type anonType = typeof(Dialog_DebugActionsMenu).GetNestedTypes(BindingFlags.NonPublic)
            .First(t => t.HasAttribute<CompilerGeneratedAttribute>() && t.GetField(varName, bf) != null);

        private static readonly MethodInfo anonMethod = anonType.GetMethods(bf).First(); // assuming first for now...

        static WeatherDebugActionFix()
        {
            var harmony = new Harmony("rimworld.whyisthat.weatherdebug");
            Log.Message("anonMethod: " + anonMethod.Name);
            harmony.Patch(anonMethod, null,
                new HarmonyMethod(typeof(WeatherDebugActionFix).GetMethod(nameof(Postfix))));
        }

        public static void Postfix()
        {
            Log.Message("Setting curWeatherDuration");
            Traverse.Create(Find.CurrentMap.weatherDecider).Field("curWeatherDuration")
                .SetValue(Find.CurrentMap.weatherManager.curWeather.durationRange.RandomInRange);
        }
    }
#endif