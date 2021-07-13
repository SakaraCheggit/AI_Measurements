using System;
using System.Collections.Generic;
using UnityEngine;

namespace Measurements.Height
{
    internal class Calculator : CalculatorBase
    {
        protected override string[] BoneNames => Enum.GetNames(typeof(Bones));

        protected override float GetValue(Dictionary<string, Vector3> boneVerts)
        {
            var topOfHead = boneVerts[nameof(Bones.N_Head_top)];
            return GetDistanceInCm(topOfHead, new Vector3(0, 0, 0));
        }

        protected override void SetValueInternal(ref MeasurementsData data, float value)
        {
            data.Height = value;
            DebugLog(nameof(MeasurementsData.Band), data.Band);
        }
    }
}
