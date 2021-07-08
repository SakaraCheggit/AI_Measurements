using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KKAPI.Chara;
using KKAPI.Utilities;
using System;

namespace Measurements
{
    [BepInPlugin(GUID, Name, Version)]
    public partial class MeasurementsPlugin : BaseUnityPlugin
    {
        public const string GUID = "sakacheggs.measurements";
        public const string Name = "Chara Measurements";
        public const string Version = "1.1.0";

        internal static ConfigEntry<bool> UseMetricUnits { get; private set; }
        internal static ConfigEntry<string> Region { get; private set; }

        internal static ConfigEntry<bool> DebugValues { get; private set; }

        internal static readonly string[] Regions = Enum.GetNames(typeof(Region));

        internal static new ManualLogSource Logger { get; private set; }

        public void Start()
        {
            Logger = base.Logger;
            PluginConfig();
            CharacterApi.RegisterExtraBehaviour<MeasurementsController>("AI_Measurements");
            MeasurementsGui.InitMaker(this);
        }

        private void PluginConfig()
        {
            DebugValues = Config.Bind("Debug", "Enable logging of measurements (Debug mode)", false,
                new ConfigDescription("Will log all measurements to the console.", null,
                    new ConfigurationManagerAttributes { IsAdvanced = true }));

            UseMetricUnits = Config.Bind("General", "Use metric measurements", false,
                new ConfigDescription("True to show values in centimeters. False to show values in inches."));

            var regions = Enum.GetNames(typeof(Region));
            Region = Config.Bind("General", "Region", regions[0],
                new ConfigDescription(
                    "Cup sizes differ by region because bra makers are dumb. Select the one you like most.",
                    new AcceptableValueList<string>(regions)));
        }
    }
}
