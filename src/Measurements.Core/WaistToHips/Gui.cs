using KKAPI.Maker;
using Measurements.Gui;

namespace Measurements.WaistToHips
{
    internal class Gui : TextGui
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e
        ) => InitializeInternal("Waist-to-Hips", category, plugin, e);

        protected override void UpdateInternal(MeasurementsData data, MeasurementsController controller)
        {
            SetText($"{data.Waist / data.Hips:N3}");
        }

        protected override bool ShouldBeVisible() =>
            MakerAPI.GetMakerSex() == GENDER_FEMALE;
    }
}
