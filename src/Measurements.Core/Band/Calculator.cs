using KKAPI.Maker;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Measurements.Band
{
    internal class Calculator : CalculatorBase
    {
        public Calculator()
        {
            BoneExceptions[nameof(Bones.cf_J_Spine02_s_Scale)] =
                (searcher) => searcher.GetObjectFromName(nameof(Bones.cf_J_Spine02_s)).transform.localScale;
        }

        protected override string[] BoneNames => Enum.GetNames(typeof(Bones));

        protected override float GetValue(Dictionary<string, Vector3> boneVerts)
        {
            const float BAND_RATIO_FRONT = 0.8095239f;
            const float BAND_RATIO_BACK = -0.8571428665f;
            const float BAND_RATIO_SIDE = 1.09523809f;

            var spine02 = boneVerts[nameof(Bones.cf_J_Spine02_s)];
            var spine03 = boneVerts[nameof(Bones.cf_J_Spine03_s)];
            var bandScale = boneVerts[nameof(Bones.cf_J_Spine02_s_Scale)];

            var front = new Vector3((spine02.x + spine03.x) / 2, spine02.y, bandScale.z * BAND_RATIO_FRONT);
            var back = new Vector3(front.x, (front.y + spine03.y) / 2, bandScale.z * BAND_RATIO_BACK);
            var right = new Vector3(bandScale.x * BAND_RATIO_SIDE, (front.y + back.y) / 2, (front.y + back.y) / 2);
            var left = new Vector3(-1 * right.x, right.y, right.z);

            var axis1 = GetDistanceInCm(front, back) / 2;
            var axis2 = GetDistanceInCm(left, right) / 2;
            var value = GetEllipseCircumference(axis1, axis2);
            // adjust band by 20% of card height less the default height (50). otherwise,
            // it measures too small for tall girls and too large for short girls
            return value * (1f + (0.2f * (MakerAPI.GetCharacterControl().fileBody.shapeValueBody[0] - 0.5f)));
        }

        protected override void SetValueInternal(ref MeasurementsData data, float value)
        {
            data.Band = value;
            DebugLog(nameof(MeasurementsData.Band), data.Band);
        }
    }
}
