using KKAPI.Maker;
using KKAPI.Maker.UI;
using System;

namespace Measurements.Gui
{
    internal class RegionGui : ConfigGui<MakerDropdown, int>
    {
        public override void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e)
        {
            var configuredRegion = Array.IndexOf(
                MeasurementsPlugin.Regions,
                MeasurementsPlugin.Region.Value);
            var control = new MakerDropdown("Region", MeasurementsPlugin.Regions, category, configuredRegion, plugin);
            InitializeInternal(control, e,
                ctrlr => ctrlr.Region,
                (ctrlr, value) => ctrlr.Region = value);
        }
    }
}
