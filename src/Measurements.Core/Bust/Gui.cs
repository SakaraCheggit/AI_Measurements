using KKAPI.Maker;
using Measurements.Gui;

namespace Measurements.Bust
{
    internal class Gui : TextGui
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e
        ) => InitializeInternal("Bust", category, plugin, e);

        protected override void UpdateInternal(MeasurementsData data, MeasurementsController controller)
        {
            SetText(controller.UseMetricUnits
                ? $"{data.Bust:N0} cm"
                : $"{data.Bust * FreedomRatio:N0}\"");
        }

        protected override bool ShouldBeVisible()
            => MakerAPI.GetMakerSex() == GENDER_FEMALE;
    }
}
