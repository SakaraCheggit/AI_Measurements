using BepInEx.Configuration;
using KKAPI.Utilities;
using System;

namespace Measurements
{
    public partial class MeasurementsPlugin
    {
        internal static class Configuration
        {
            public static ConfigEntry<bool> UseMetricUnits { get; private set; }
            public static ConfigEntry<string> Region { get; private set; }

            public static ConfigEntry<bool> DebugValues { get; private set; }

            public static void Initialize(ConfigFile configFile)
            {
                DebugValues = configFile.Bind("Debug", "Enable logging of measurements (Debug mode)", false,
                    new ConfigDescription("Will log all measurements to the console.", null,
                        new ConfigurationManagerAttributes { IsAdvanced = true }));

                UseMetricUnits = configFile.Bind("General", "Use metric measurements", false,
                    new ConfigDescription("True to show values in centimeters. False to show values in inches."));

                var regions = Enum.GetNames(typeof(Region));
                Region = configFile.Bind("General", "Region", regions[0],
                    new ConfigDescription(
                        "Cup sizes differ by region because bra makers are dumb. Select the one you like most.",
                        new AcceptableValueList<string>(regions)));
            }
        }
    }
}
