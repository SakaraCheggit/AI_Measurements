using AIChara;
using KKAPI;
using KKAPI.Chara;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Measurements
{
    public class MeasurementsController : CharaCustomFunctionController
    {
        public bool UseMetricUnits { get; internal set; }
        public int Region { get; internal set; }

        private readonly FindAssist _boneSearcher = new FindAssist();
        private bool _initialized;
        private bool _waiting = false;

        private static readonly List<CalculatorBase> s_calculators = new List<CalculatorBase>
        {
            new Height.Calculator(),
            new Bust.Calculator(),
            new Band.Calculator(),
            new Waist.Calculator(),
            new Hips.Calculator(),
            new Dick.Calculator(),
        };
        private static Dictionary<string, Vector3> s_prevVerts;

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
            UpdateTexts();
            base.OnReload(currentGameMode);
        }

        internal void UpdateTexts()
        {
            if (!_initialized) Initialize();

            if (!_waiting)
            {
                _waiting = true;
                StartCoroutine(CoUpdateTexts());
            }
        }

        private IEnumerator CoUpdateTexts()
        {
            while (IsDelayRequired())
                yield return new WaitForSeconds(0.1f);

            var data = new MeasurementsData();
            if (MeasurementsPlugin.Configuration.DebugValues.Value)
                MeasurementsPlugin.Logger.LogInfo($">>> Measurement Values <<<");
            foreach (var calculator in s_calculators)
                calculator.SetValue(ref data, _boneSearcher);
            foreach (var gui in MeasurementsPlugin.UI.MeasurementGuis)
                gui.Update(data, this);

            yield return new WaitForSeconds(0.1f);
            _waiting = false;
        }

        private bool IsDelayRequired()
        {
            var currentVerts = s_calculators
                .SelectMany(calculator => calculator.GetBoneVertices(_boneSearcher))
                .ToDictionary(bone => bone.Key, bone => bone.Value);
            var partsAreJiggling = currentVerts.Keys
                .Any(key => Vector3.Distance(s_prevVerts?[key] ?? Vector3.zero, currentVerts[key]) > 0.001f);
            if (partsAreJiggling) s_prevVerts = currentVerts;
            return partsAreJiggling;
        }
    }
}
