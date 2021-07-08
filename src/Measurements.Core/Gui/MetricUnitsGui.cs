using KKAPI.Maker;
using KKAPI.Maker.UI;

namespace Measurements.Gui
{
    internal class MetricUnitsGui : ConfigGui<MakerToggle, bool>
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e)
        {
            var control = new MakerToggle(category, "Use metric units", MeasurementsPlugin.UseMetricUnits.Value, plugin);
            InitializeInternal(control, e,
                ctrlr => ctrlr.UseMetricUnits,
                (ctrlr, value) => ctrlr.UseMetricUnits = value);
        }
    }
}
