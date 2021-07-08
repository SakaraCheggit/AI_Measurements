using BepInEx;
using KKAPI;

namespace Measurements
{
    [BepInProcess("HoneySelect2")]
    [BepInProcess("HoneySelect2VR")]
    [BepInDependency(KoikatuAPI.GUID, "1.20")]
    public partial class MeasurementsPlugin
    {
    }
}
