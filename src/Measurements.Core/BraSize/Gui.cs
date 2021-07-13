using KKAPI.Maker;
using Measurements.Gui;
using System;

namespace Measurements.BraSize
{
    internal class Gui : TextGui
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e
        ) => InitializeInternal("Bra Size", category, plugin, e);

        protected override void UpdateInternal(MeasurementsData data, MeasurementsController controller)
        {
            if (controller.Region >= 0)
            {
                var region = (Region)Enum.Parse(typeof(Region), MeasurementsPlugin.Regions[controller.Region]);
                var bandSize = ((int)((data.Band * FreedomRatio) / 2) + 1) * 2;
                var difference = (data.Bust * FreedomRatio) - bandSize;
                var cupSize = CupSizeCalculator.GetCupSize(difference, region);
                SetText(controller.UseMetricUnits
                    ? $"{Math.Round((bandSize / FreedomRatio) / 5) * 5:N0}{cupSize}"
                    : $"{bandSize:N0}{cupSize}");
            }
        }

        protected override bool ShouldBeVisible()
            => MakerAPI.GetMakerSex() == GENDER_FEMALE;
    }
}
