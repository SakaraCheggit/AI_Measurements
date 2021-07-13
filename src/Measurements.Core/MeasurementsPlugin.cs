using BepInEx;
using BepInEx.Logging;
using KKAPI.Chara;
using System;

namespace Measurements
{
    [BepInPlugin(GUID, Name, Version)]
    public partial class MeasurementsPlugin : BaseUnityPlugin
    {
        public const string GUID = "sakacheggs.measurements";
        public const string Name = "Chara Measurements";
        public const string Version = "1.1.0";

        internal static readonly string[] Regions = Enum.GetNames(typeof(Region));

        internal static new ManualLogSource Logger { get; private set; }

        public void Start()
        {
            Logger = base.Logger;
            MeasurementsPlugin.Configuration.Initialize(Config);
            CharacterApi.RegisterExtraBehaviour<MeasurementsController>("AI_Measurements");
            MeasurementsPlugin.UI.Initialize(this);
        }
    }
}
