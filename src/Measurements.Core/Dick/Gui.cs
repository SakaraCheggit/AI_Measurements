using KKAPI.Maker;
using Measurements.Gui;

namespace Measurements.Dick
{
    internal class Gui : TextGui
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e
        )
        {
            var initialText = MakerAPI.GetMakerSex() == GENDER_MALE ? "Dick" : "Dick (futa)";
            InitializeInternal(initialText, category, plugin, e);
        }

        protected override void UpdateInternal(MeasurementsData data, MeasurementsController controller)
        {
            if (controller.UseMetricUnits) SetText($"{data.Dick:N1} cm");
            else SetText($"{data.Dick * FreedomRatio:N1}\"");
        }

        protected override bool ShouldBeVisible()
        {
            var isMale = MakerAPI.GetMakerSex() == GENDER_MALE;
            var isFuta = MakerAPI.GetCharacterControl().chaFile.parameter.futanari;
            return isMale || isFuta;
        }
    }
}
