using KKAPI.Chara;
using KKAPI.Maker;
using KKAPI.Maker.UI;
using System;
using System.Collections.Generic;

namespace Measurements.Gui
{
    internal abstract class ConfigGui<TControl, TValue> : IGui<BaseEditableGuiEntry<TValue>>
        where TControl : BaseEditableGuiEntry<TValue>
    {
        public abstract void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e);

        protected void InitializeInternal(
            TControl control,
            RegisterSubCategoriesEvent e,
            Func<MeasurementsController, TValue> getValue,
            Action<MeasurementsController, TValue> setValue)
        {
            e.AddControl(control);
            control.BindToFunctionController(
                (MeasurementsController ctrlr) => getValue(ctrlr),
                (MeasurementsController ctrlr, TValue value) =>
                {
                    var oldValue = getValue(ctrlr);
                    var valueChanged =
                        !EqualityComparer<TValue>.Default.Equals(oldValue, value);
                    if (valueChanged)
                    {
                        setValue(ctrlr, value);
                        OnMakerSettingsChanged(ctrlr);
                    }
                });
        }

        public static void OnMakerSettingsChanged(MeasurementsController controller)
        {
            controller.UpdateTexts();
        }
    }
}
