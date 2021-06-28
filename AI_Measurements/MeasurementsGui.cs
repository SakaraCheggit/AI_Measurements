﻿using KKAPI.Maker;
using KKAPI.Maker.UI;
using KKAPI.Studio;
using System.Collections.Generic;

namespace AI_Measurements
{
    public static class MeasurementsGui
    {
        private static MeasurementsPlugin _pluginInstance;

        internal static List<Gui.TextGui> TextGuis { get; private set; } = new List<Gui.TextGui>
        {
            new Gui.HeightGui(),
            new Gui.BraSizeGui(),
            new Gui.BustGui(),
            new Gui.WaistGui(),
            new Gui.HipsGui(),
            new Gui.WaistToHipsGui(),
            new Gui.DickGui(),
        };

        internal static void InitMaker(MeasurementsPlugin measurementsPlugin)
        {
            _pluginInstance = measurementsPlugin;
            if (!StudioAPI.InsideStudio)
            {
                MakerAPI.RegisterCustomSubCategories += MakerAPI_RegisterCustomSubCategories;
                MakerAPI.MakerFinishedLoading += (_, e) => { UpdateData(); };
            }
        }
        private static void MakerAPI_RegisterCustomSubCategories(object _, RegisterSubCategoriesEvent e)
        {
            var category = new MakerCategory(MakerConstants.Body.CategoryName, "Measurements");
            e.AddSubCategory(category);
            AddMeasurementControls(category, e);
            e.AddControl(new MakerSeparator(category, _pluginInstance));
            var refreshButton = e.AddControl(new MakerButton("Refresh", category, _pluginInstance));
            refreshButton.OnClick.AddListener(UpdateData);
            e.AddControl(new MakerSeparator(category, _pluginInstance));
            AddConfigControls(category, e);
        }

        private static void AddMeasurementControls(MakerCategory category, RegisterSubCategoriesEvent e)
        {
            foreach (var gui in TextGuis)
            {
                gui.Initialize(category, _pluginInstance, e);
            }
        }

        private static void AddConfigControls(MakerCategory category, RegisterSubCategoriesEvent e)
        {
            var metricUnitsGui = new Gui.MetricUnitsGui();
            metricUnitsGui.Initialize(category, _pluginInstance, e);
            var regionGui = new Gui.RegionGui();
            regionGui.Initialize(category, _pluginInstance, e);
        }

        private static void UpdateData()
        {
            var controller = MakerAPI.GetCharacterControl().gameObject.GetComponent<MeasurementsController>();
            controller.UpdateTexts();
        }
    }
}
