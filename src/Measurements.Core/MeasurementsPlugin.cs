using BepInEx;
using BepInEx.Logging;
using KKAPI.Chara;
using System;
using UnityEngine;

namespace Measurements
{
    [BepInPlugin(GUID, Name, Version)]
    [HelpURL("https://github.com/SakaraCheggit/AI_Measurements")]
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
            Configuration.Initialize(Config);
            CharacterApi.RegisterExtraBehaviour<MeasurementsController>(GUID);
            Hooks.InitHooks();
            UI.Initialize(this);
        }
    }
}
