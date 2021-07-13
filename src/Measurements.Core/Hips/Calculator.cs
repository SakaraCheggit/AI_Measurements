using System;
using System.Collections.Generic;
using UnityEngine;

namespace Measurements.Hips
{
    internal class Calculator : CalculatorBase
    {
        protected override string[] BoneNames => Enum.GetNames(typeof(Bones));

        protected override float GetValue(Dictionary<string, Vector3> boneVerts)
        {
            // TODO find way to adjust Ass vector for butt size as the Legsk_0(3|5) bones don't adjust when ass size changes
            var leftHip = new HipData
            {
                Front = boneVerts[nameof(Bones.cf_J_Legsk_07_00)],
                Side = boneVerts[nameof(Bones.cf_J_Legsk_06_00)],
                Ass = boneVerts[nameof(Bones.cf_J_Legsk_05_00)],
            };
            var rightHip = new HipData
            {
                Front = boneVerts[nameof(Bones.cf_J_Legsk_01_00)],
                Side = boneVerts[nameof(Bones.cf_J_Legsk_02_00)],
                Ass = boneVerts[nameof(Bones.cf_J_Legsk_03_00)],
            };
            return (
                GetDistanceInCm(leftHip.Front, leftHip.Side) +
                GetDistanceInCm(leftHip.Side, leftHip.Ass) +
                GetDistanceInCm(leftHip.Ass, rightHip.Ass) +
                GetDistanceInCm(rightHip.Ass, rightHip.Side) +
                GetDistanceInCm(rightHip.Side, rightHip.Front) +
                GetDistanceInCm(rightHip.Front, leftHip.Front)
            );
        }

        protected override void SetValueInternal(ref MeasurementsData data, float value)
        {
            data.Hips = value;
            DebugLog(nameof(MeasurementsData.Band), data.Band);
        }
    }
}
