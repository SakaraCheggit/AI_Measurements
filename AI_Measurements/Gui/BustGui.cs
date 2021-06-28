using KKAPI.Maker;

namespace AI_Measurements.Gui
{
    internal class BustGui : TextGui
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e
        ) => InitializeInternal("Bust", category, plugin, e);

        public override void Update(MeasurementsData data, MeasurementsController controller)
        {
            SetText(controller.UseMetricUnits
                ? $"{data.Bust:N0} cm"
                : $"{data.Bust * FreedomRatio:N0}\"");
        }

        protected override bool ShouldBeVisible()
            => MakerAPI.GetMakerSex() == GENDER_FEMALE;
    }
}
