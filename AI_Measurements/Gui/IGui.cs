using KKAPI.Maker;
using KKAPI.Maker.UI;

namespace AI_Measurements.Gui
{
    internal interface IGui<TControl> where TControl : BaseGuiEntry
    {
        void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e);
    }
}
