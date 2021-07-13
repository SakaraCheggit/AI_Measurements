using KKAPI.Maker;
using Measurements.Gui;

namespace Measurements.Waist
{
    internal class Gui : TextGui
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e
        ) => InitializeInternal("Waist", category, plugin, e);

        protected override void UpdateInternal(MeasurementsData data, MeasurementsController controller)
        {
            SetText(controller.UseMetricUnits
                ? $"{data.Waist:N0} cm"
                : $"{data.Waist * FreedomRatio:N0}\"");
        }

        protected override bool ShouldBeVisible() => true;
    }
}
