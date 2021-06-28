using KKAPI.Maker;

namespace AI_Measurements.Gui
{
    internal class WaistToHipsGui : TextGui
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e
        ) => InitializeInternal("Waist-to-Hips", category, plugin, e);

        public override void Update(MeasurementsData data, MeasurementsController controller)
        {
            SetText($"{data.Waist / data.Hips:N3}");
        }

        protected override bool ShouldBeVisible() =>
            MakerAPI.GetMakerSex() == GENDER_FEMALE;
    }
}
