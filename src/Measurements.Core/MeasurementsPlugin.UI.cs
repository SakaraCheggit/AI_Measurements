using KKAPI;
using KKAPI.Maker;
using KKAPI.Maker.UI;
using System.Collections.Generic;

namespace Measurements
{
    public partial class MeasurementsPlugin
    {
        internal static class UI
        {
            internal static List<Gui.TextGui> MeasurementGuis { get; } = new List<Gui.TextGui>
            {
                new Height.Gui(),
                new BraSize.Gui(),
                new Bust.Gui(),
                new Waist.Gui(),
                new Hips.Gui(),
                new WaistToHips.Gui(),
                new Dick.Gui(),
            };

            internal static void Initialize(MeasurementsPlugin plugin)
            {
                MakerAPI.RegisterCustomSubCategories += (_, e) =>
                {
                    if (KoikatuAPI.GetCurrentGameMode() == GameMode.Maker)
                    {
                        RegisterCustomSubCategories(plugin, e);
                    }
                };
                MakerAPI.MakerFinishedLoading += (_, e) => { UpdateData(); };
            }

            private static void RegisterCustomSubCategories(MeasurementsPlugin plugin, RegisterSubCategoriesEvent e)
            {
                var category = new MakerCategory(MakerConstants.Body.CategoryName, "Measurements");
                e.AddSubCategory(category);
                AddMeasurementControls(plugin, category, e);
                e.AddControl(new MakerSeparator(category, plugin));
                var refreshButton = e.AddControl(new MakerButton("Refresh", category, plugin));
                refreshButton.OnClick.AddListener(UpdateData);
                e.AddControl(new MakerSeparator(category, plugin));
                AddConfigControls(plugin, category, e);
            }

            private static void AddMeasurementControls(MeasurementsPlugin plugin, MakerCategory category, RegisterSubCategoriesEvent e)
            {
                foreach (var gui in MeasurementGuis)
                {
                    gui.Initialize(category, plugin, e);
                }
            }

            private static void AddConfigControls(MeasurementsPlugin plugin, MakerCategory category, RegisterSubCategoriesEvent e)
            {
                var metricUnitsGui = new Gui.MetricUnitsGui();
                metricUnitsGui.Initialize(category, plugin, e);
                var regionGui = new Gui.RegionGui();
                regionGui.Initialize(category, plugin, e);
            }

            private static void UpdateData()
            {
                var controller = MakerAPI.GetCharacterControl().gameObject.GetComponent<MeasurementsController>();
                controller.UpdateTexts();
            }
        }
    }
}
