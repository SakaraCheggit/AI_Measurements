using KKAPI.Maker;
using System;
using UnityEngine;

namespace Measurements
{
    internal static class MeasurementsCalculator
    {
        public static float CalculateHeight(Vector3 topOfHead)
            => GetDistanceInCm(topOfHead, new Vector3(0, 0, 0));

        public static float CalculateBand(Vector3 front, Vector3 back, Vector3 left, Vector3 right) =>
            GetEllipseCircumference(
                GetDistanceInCm(front, back) / 2,
                GetDistanceInCm(left, right) / 2)
            // adjust band by 20% of card height less the default height (50). otherwise,
            // it measures too small for tall girls and too large for short girls
            * (1f + (0.2f * (MakerAPI.GetCharacterControl().fileBody.shapeValueBody[0] - 0.5f)));

        public static float CalculateBust(TitMeasurement rightTit, TitMeasurement leftTit) => (
            GetDistanceInCm(rightTit.Nipple, rightTit.SideBoob) +
            GetDistanceInCm(rightTit.SideBoob, rightTit.Lat) +
            GetDistanceInCm(rightTit.Lat, leftTit.Lat) +
            GetDistanceInCm(leftTit.Lat, leftTit.SideBoob) +
            GetDistanceInCm(leftTit.SideBoob, leftTit.Nipple) +
            GetDistanceInCm(leftTit.Nipple, rightTit.Nipple)
        );

        public static float CalculateWaist(Vector3 front, Vector3 back, Vector3 left, Vector3 right) =>
            GetEllipseCircumference(
                GetDistanceInCm(front, back) / 2,
                GetDistanceInCm(left, right) / 2);

        public static float CalculateHips(HipMeasurement leftHip, HipMeasurement rightHip) =>
            GetDistanceInCm(leftHip.Front, leftHip.Side) +
            GetDistanceInCm(leftHip.Side, leftHip.Ass) +
            GetDistanceInCm(leftHip.Ass, rightHip.Ass) +
            GetDistanceInCm(rightHip.Ass, rightHip.Side) +
            GetDistanceInCm(rightHip.Side, rightHip.Front) +
            GetDistanceInCm(rightHip.Front, leftHip.Front);

        public static float CalculateDick(Vector3 dickBase, Vector3 dickTip) =>
            GetDistanceInCm(dickBase, dickTip);

        public static Vector3 GetRightSideBoob(Vector3 middleBoob, Vector3 nipple) =>
            GetSideBoob(middleBoob, nipple, phi => phi + (Math.PI / 2), theta => theta);

        public static Vector3 GetLeftSideBoob(Vector3 centerOfBoob, Vector3 nipple) =>
            GetSideBoob(centerOfBoob, nipple, phi => -1 * (phi + (Math.PI / 2)), theta => -1 * theta);

        private static Vector3 GetSideBoob(Vector3 centerOfBoob, Vector3 nipple, Func<double, double> adjustPhi, Func<double, double> adjustTheta)
        {
            var p = GetDistance(centerOfBoob, nipple);
            var phi_0 = Math.Acos((nipple.z - centerOfBoob.z) / p);
            var theta_0 = Math.Asin((nipple.y - centerOfBoob.y) / (p * Math.Sin(phi_0)));
            var phi = adjustPhi(phi_0);
            var theta = adjustTheta(theta_0);
            return new Vector3(
                x: centerOfBoob.x + p * (float)(Math.Sin(phi) * Math.Cos(theta)),
                y: centerOfBoob.y + p * (float)(Math.Sin(phi) * Math.Sin(theta)),
                z: centerOfBoob.z + p * (float)Math.Cos(phi));
        }

        private static readonly double s_rotationForSideBoob = Math.PI / 2;

        public static Vector3 GetRightSideBoob(Vector3 titCenter, Vector3 nipple, float targetY) =>
            GetSideBoob(titCenter, nipple, targetY, s_rotationForSideBoob);
        public static Vector3 GetLeftSideBoob(Vector3 titCenter, Vector3 nipple, float targetY) =>
            GetSideBoob(titCenter, nipple, targetY, -1 * s_rotationForSideBoob);

        private static Vector3 GetSideBoob(Vector3 titCenter, Vector3 nipple, float targetY, double thetaXZAdjustment)
        {
            var rhoXYZ = GetDistance(titCenter, nipple);
            var rhoXZ = Math.Sqrt(Math.Pow(rhoXYZ, 2) - Math.Pow(targetY - titCenter.y, 2));
            var thetaXZ = Math.Atan((nipple.x - titCenter.x) / (nipple.z - titCenter.z)) + thetaXZAdjustment;
            return new Vector3(
                x: titCenter.x + (float)(rhoXZ * Math.Sin(thetaXZ)),
                y: targetY,
                z: titCenter.z + (float)(rhoXZ * Math.Cos(thetaXZ))
            );
        }

        private static float GetDistanceInCm(Vector3 pointA, Vector3 pointB) =>
            // pulled 10.5 value from HeightBarX plugin
            GetDistance(pointA, pointB) * 10.5f;

        private static float GetEllipseCircumference(float radiusToFront, float radiusToSide) =>
            (float)(2 * Math.PI * Math.Sqrt(
                (Math.Pow(radiusToFront, 2) + Math.Pow(radiusToSide, 2)) / 2
            ));

        public static float GetDistance(Vector3 pointA, Vector3 pointB) =>
            (float)Math.Sqrt(
                Math.Pow(pointA.x - pointB.x, 2) +
                Math.Pow(pointA.y - pointB.y, 2) +
                Math.Pow(pointA.z - pointB.z, 2)
            );
    }
}
