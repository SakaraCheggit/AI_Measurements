using System;
using UnityEngine;

namespace Measurements
{
    internal static class MeasurementsCalculator
    {
        public static float CalculateWaist(Vector3 front, Vector3 back, Vector3 left, Vector3 right) =>
            GetEllipseCircumference(
                GetDistanceInCm(front, back) / 2,
                GetDistanceInCm(left, right) / 2);

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
