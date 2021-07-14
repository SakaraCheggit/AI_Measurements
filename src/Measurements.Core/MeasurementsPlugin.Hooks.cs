using AIChara;
using HarmonyLib;
using KKAPI.Maker;
using System.Linq;

namespace Measurements
{
    public partial class MeasurementsPlugin
    {
        private static MeasurementsController GetController(ChaControl chaControl) =>
            chaControl?.gameObject.GetComponent<MeasurementsController>();

        private static partial class Hooks
        {
            public static void InitHooks()
            {
                Harmony.CreateAndPatchAll(typeof(Hooks));
            }

            [HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.SetShapeBodyValue))]
            public static void ChaControl_SetShapeBodyValue(ChaControl __instance, int index, float value)
            {
                if (!MakerAPI.InsideAndLoaded) return;
                var isMeasuredBodyShapes = s_measuredBodyShapes.Any(shapeIdx => shapeIdx == index);
                if (!isMeasuredBodyShapes) return;

                GetController(__instance).UpdateTexts();
            }

            [HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.AnimPlay))]
            public static void ChaControl_AnimPlay(ChaControl __instance)
            {
                if (!MakerAPI.InsideAndLoaded) return;

                GetController(__instance).UpdateTexts();
            }

            private static readonly int[] s_measuredBodyShapes = new ChaFileDefine.BodyShapeIdx[]
            {
                ChaFileDefine.BodyShapeIdx.Height,
                ChaFileDefine.BodyShapeIdx.BustSize,
                ChaFileDefine.BodyShapeIdx.BustY,
                ChaFileDefine.BodyShapeIdx.BustRotX,
                ChaFileDefine.BodyShapeIdx.BustX,
                ChaFileDefine.BodyShapeIdx.BustRotY,
                ChaFileDefine.BodyShapeIdx.BustSharp,
                ChaFileDefine.BodyShapeIdx.AreolaBulge,
                ChaFileDefine.BodyShapeIdx.BodyShoulderW,
                ChaFileDefine.BodyShapeIdx.BodyShoulderZ,
                ChaFileDefine.BodyShapeIdx.BodyUpW,
                ChaFileDefine.BodyShapeIdx.BodyUpZ,
                ChaFileDefine.BodyShapeIdx.WaistUpW,
                ChaFileDefine.BodyShapeIdx.WaistUpZ,
                ChaFileDefine.BodyShapeIdx.WaistLowW,
                ChaFileDefine.BodyShapeIdx.WaistLowZ,
                ChaFileDefine.BodyShapeIdx.ThighUp,
            }.Select(idx => (int)idx).ToArray();
        }
    }
}
