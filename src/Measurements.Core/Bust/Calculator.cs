using System;
using System.Collections.Generic;
using UnityEngine;

namespace Measurements.Bust
{
    internal class Calculator : CalculatorBase
    {
        protected override float GetValue(Dictionary<string, Vector3> boneVerts)
        {
            var leftSideBoob = GetLeftSideBoob(
                titCenter: boneVerts[nameof(Bones.cf_J_Mune02_L)],
                nipple: boneVerts[nameof(Bones.cf_J_Mune_Nip01_L)],
                targetY: (boneVerts[nameof(Bones.N_Back_L)].y + boneVerts[nameof(Bones.cf_J_Mune02_L)].y) / 2
            );
            var rightSideBoob = GetRightSideBoob(
                titCenter: boneVerts[nameof(Bones.cf_J_Mune02_R)],
                nipple: boneVerts[nameof(Bones.cf_J_Mune_Nip01_R)],
                targetY: (boneVerts[nameof(Bones.N_Back_R)].y + boneVerts[nameof(Bones.cf_J_Mune02_R)].y) / 2
            );

            var rightTit = new TitData
            {
                Nipple = boneVerts[nameof(Bones.cf_J_Mune_Nip01_R)],
                SideBoob = rightSideBoob,
                Lat = boneVerts[nameof(Bones.N_Back_R)],
            };
            var leftTit = new TitData
            {
                Nipple = boneVerts[nameof(Bones.cf_J_Mune_Nip01_L)],
                SideBoob = leftSideBoob,
                Lat = boneVerts[nameof(Bones.N_Back_L)],
            };

            return (
                GetDistanceInCm(rightTit.Nipple, rightTit.SideBoob) +
                GetDistanceInCm(rightTit.SideBoob, rightTit.Lat) +
                GetDistanceInCm(rightTit.Lat, leftTit.Lat) +
                GetDistanceInCm(leftTit.Lat, leftTit.SideBoob) +
                GetDistanceInCm(leftTit.SideBoob, leftTit.Nipple) +
                GetDistanceInCm(leftTit.Nipple, rightTit.Nipple)
            );
        }

        private static readonly float s_rotationForSideBoob = (float)Math.PI / 2;

        protected override string[] BoneNames => Enum.GetNames(typeof(Bones));

        private Vector3 GetRightSideBoob(Vector3 titCenter, Vector3 nipple, float targetY) =>
            GetSideBoob(titCenter, nipple, targetY, s_rotationForSideBoob);
        private Vector3 GetLeftSideBoob(Vector3 titCenter, Vector3 nipple, float targetY) =>
            GetSideBoob(titCenter, nipple, targetY, -1 * s_rotationForSideBoob);

        private Vector3 GetSideBoob(Vector3 titCenter, Vector3 nipple, float targetY, double thetaXZAdjustment)
        {
            var rhoXYZ = Vector3.Distance(titCenter, nipple);
            var rhoXZ = Math.Sqrt(Math.Pow(rhoXYZ, 2) - Math.Pow(targetY - titCenter.y, 2));
            var thetaXZ = Math.Atan((nipple.x - titCenter.x) / (nipple.z - titCenter.z)) + thetaXZAdjustment;
            return new Vector3(
                x: titCenter.x + (float)(rhoXZ * Math.Sin(thetaXZ)),
                y: targetY,
                z: titCenter.z + (float)(rhoXZ * Math.Cos(thetaXZ))
            );
        }

        protected override void SetValueInternal(ref MeasurementsData data, float value)
        {
            data.Bust = value;
            DebugLog(nameof(MeasurementsData.Bust), data.Bust);
        }
    }
}
