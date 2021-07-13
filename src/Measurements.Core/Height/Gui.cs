using KKAPI.Maker;
using Measurements.Gui;

namespace Measurements.Height
{
    internal class Gui : TextGui
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e
        ) => InitializeInternal("Height", category, plugin, e);

        protected override void UpdateInternal(MeasurementsData data, MeasurementsController controller)
        {
            if (controller.UseMetricUnits) SetText($"{data.Height:N1} cm");
            else
            {
                var height = data.Height * FreedomRatio;
                var feet = (int)(height / 12);
                var inches = height % 12;
                SetText($"{feet}' {inches:N0}\" ({height:N1} in)");
            }
        }

        protected override bool ShouldBeVisible() => true;
    }
}
