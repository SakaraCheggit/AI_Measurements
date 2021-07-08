﻿using KKAPI.Maker;

namespace AI_Measurements.Gui
{
    internal class HeightGui : TextGui
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e
        ) => InitializeInternal("Height", category, plugin, e);

        public override void Update(MeasurementsData data, MeasurementsController controller)
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
