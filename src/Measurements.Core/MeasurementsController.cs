using AIChara;
using KKAPI;
using KKAPI.Chara;
using KKAPI.Maker;
using System.Collections.Generic;

namespace Measurements
{
    public class MeasurementsController : CharaCustomFunctionController
    {
        public bool UseMetricUnits { get; internal set; }
        public int Region { get; internal set; }

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

        internal void UpdateTexts()
        {
            if (!_initialized) Initialize();

            var data = new MeasurementsData();
            foreach (var calculator in s_calculators)
                calculator.SetValue(ref data, _boneSearcher);
            foreach (var gui in MeasurementsPlugin.UI.MeasurementGuis)
                gui.Update(data, this);
        }
    }
}
