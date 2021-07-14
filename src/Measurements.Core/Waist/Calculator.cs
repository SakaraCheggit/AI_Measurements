using System;
using System.Collections.Generic;
using UnityEngine;

namespace Measurements.Waist
{
    internal class Calculator : CalculatorBase
    {
        protected override string[] BoneNames => Enum.GetNames(typeof(Bones));

        protected override float GetValue(Dictionary<string, Vector3> boneVerts)
        {
            var waistLeft = boneVerts[nameof(Bones.N_Waist_L)];
            var waistRight = boneVerts[nameof(Bones.N_Waist_R)];
            var waistFront = boneVerts[nameof(Bones.N_Waist_f)];
            var waistBack = boneVerts[nameof(Bones.N_Waist_b)];

            var axis1 = GetDistanceInCm(waistFront, waistBack) / 2;
            var axis2 = GetDistanceInCm(waistLeft, waistRight) / 2;
            return GetEllipseCircumference(axis1, axis2);
        }

        protected override void SetValueInternal(ref MeasurementsData data, float value)
        {
            data.Waist = value;
            DebugLog(nameof(MeasurementsData.Waist), data.Waist);
        }
    }
}
