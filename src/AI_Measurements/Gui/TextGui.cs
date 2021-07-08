using KKAPI.Maker;
using KKAPI.Maker.UI;

namespace AI_Measurements.Gui
{
    internal abstract class TextGui : IGui<MakerText>
    {
        protected const int GENDER_FEMALE = 1;
        protected const int GENDER_MALE = 0;

        protected static readonly float FreedomRatio = 1 / 2.54f;

        private MakerText _control;
        private string _initialText;

        public abstract void Initialize(
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e);

        public abstract void Update(MeasurementsData data, MeasurementsController controller);

        protected void InitializeInternal(
            string initialText,
            MakerCategory category,
            MeasurementsPlugin plugin,
            RegisterSubCategoriesEvent e)
        {
            _initialText = initialText;
            _control = new MakerText(initialText + ":", category, plugin);
            e.AddControl(_control);
        }

        protected void SetText(string text)
        {
            if (!(_control is null))
            {
                _control.Text = $"{_initialText}: {text}";
            }
        }

        protected abstract bool ShouldBeVisible();

        internal bool IsVisible() => _control.Visible.Value;

        internal void SetVisibility()
        {
            var shouldBeVisible = ShouldBeVisible();
            var isVisible = IsVisible();
            if ((shouldBeVisible && !isVisible) || (!shouldBeVisible && isVisible))
                _control.Visible.OnNext(!isVisible);
        }
    }
}
