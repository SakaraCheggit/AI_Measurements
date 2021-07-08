using KKAPI.Maker;

namespace AI_Measurements.Gui
{
    internal class WaistGui : TextGui
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e
        ) => InitializeInternal("Waist", category, plugin, e);

        public override void Update(MeasurementsData data, MeasurementsController controller)
        {
            SetText(controller.UseMetricUnits
                ? $"{data.Waist:N0} cm"
                : $"{data.Waist * FreedomRatio:N0}\"");
        }

        protected override bool ShouldBeVisible() => true;
    }
}
