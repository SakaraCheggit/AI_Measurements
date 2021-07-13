using AIChara;
using KKAPI;
using KKAPI.Chara;
using KKAPI.Maker;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Measurements
{
    public class MeasurementsController : CharaCustomFunctionController
    {
        public bool UseMetricUnits { get; internal set; }
        public int Region { get; internal set; }

        private static readonly Dictionary<Bones, string> s_measurementBones = Enum.GetValues(typeof(Bones))
            .Cast<Bones>()
            .ToDictionary(bone => bone, bone => Enum.GetName(typeof(Bones), bone));

        private readonly FindAssist _boneSearcher = new FindAssist();
        private bool _initialized;

        private static readonly List<CalculatorBase> s_calculators = new List<CalculatorBase>
        {
            new Height.Calculator(),
            new Bust.Calculator(),
            new Band.Calculator(),
            new Waist.Calculator(),
            new Hips.Calculator(),
            new Dick.Calculator(),
        };

        protected override void OnCardBeingSaved(GameMode currentGameMode) { }

        private void Initialize()
        {
            if (!_initialized)
            {
                _boneSearcher.Initialize(ChaControl.transform);
                _initialized = true;
            }
        }

        protected override void OnReload(GameMode currentGameMode)
        {
            if (MakerAPI.InsideAndLoaded)
            {
                UpdateTexts();
            }
            base.OnReload(currentGameMode);
        }

        internal Dictionary<Bones, Vector3> GetBoneVertices()
        {
            var boneVerts = new Dictionary<Bones, Vector3>();
            foreach (var bone in s_measurementBones)
            {
                var searchResult = _boneSearcher.GetObjectFromName(bone.Value);
                if (searchResult != null)
                    boneVerts[bone.Key] = searchResult.transform.position;
            }
            return boneVerts;
        }

        internal MeasurementsData GetMeasurements(Dictionary<Bones, Vector3> boneVerts)
        {
            return new MeasurementsData
            {
            };
        }

        internal void UpdateTexts()
        {
            if (!_initialized) Initialize();

            var boneVerts = GetBoneVertices();
            var data = GetMeasurements(boneVerts);
            foreach (var calculator in s_calculators)
                calculator.SetValue(ref data, _boneSearcher);
            foreach (var gui in MeasurementsGui.TextGuis)
                gui.REFACTOR_Update(data, this);

            if (MeasurementsPlugin.Configuration.DebugValues.Value)
            {
            }
        }
    }
}
