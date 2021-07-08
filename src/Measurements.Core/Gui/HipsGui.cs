using KKAPI.Maker;

namespace Measurements.Gui
{
    internal class HipsGui : TextGui
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e
        ) => InitializeInternal("Hips", category, plugin, e);

        public override void Update(MeasurementsData data, MeasurementsController controller)
        {
            SetText(controller.UseMetricUnits
                ? $"{data.Hips:N0} cm"
                : $"{data.Hips * FreedomRatio:N0}\"");
        }

        protected override bool ShouldBeVisible()
            => MakerAPI.GetMakerSex() == GENDER_FEMALE;
    }
}
