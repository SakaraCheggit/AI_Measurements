using System;
using System.Collections.Generic;
using UnityEngine;

namespace Measurements.Dick
{
    internal class Calculator : CalculatorBase
    {
        protected override string[] BoneNames => Enum.GetNames(typeof(Bones));

        protected override float GetValue(Dictionary<string, Vector3> boneVerts)
        {
            var dickBase = boneVerts[nameof(Bones.cm_J_dan100_00)];
            var dickTip = boneVerts[nameof(Bones.cm_J_dan109_00)];

            return GetDistanceInCm(dickBase, dickTip);
        }

        protected override void SetValueInternal(ref MeasurementsData data, float value)
        {
            data.Dick = value;
            DebugLog(nameof(MeasurementsData.Band), data.Band);
        }
    }
}
