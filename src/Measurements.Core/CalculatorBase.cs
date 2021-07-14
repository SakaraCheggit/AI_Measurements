using System;
using System.Collections.Generic;
using UnityEngine;

namespace Measurements
{
    internal abstract class CalculatorBase
    {
        protected abstract string[] BoneNames { get; }

        protected Dictionary<string, Func<FindAssist, Vector3>> BoneExceptions { get; } = new Dictionary<string, Func<FindAssist, Vector3>>();

        protected abstract float GetValue(Dictionary<string, Vector3> boneVerts);
        protected abstract void SetValueInternal(ref MeasurementsData data, float value);

        public void SetValue(ref MeasurementsData data, FindAssist boneSearcher)
        {
            var boneVerts = GetBoneVertices(boneSearcher);
            var value = GetValue(boneVerts);
            SetValueInternal(ref data, value);
        }

        internal Dictionary<string, Vector3> GetBoneVertices(FindAssist boneSearcher)
        {
            var boneVerts = new Dictionary<string, Vector3>();
            foreach (var boneName in BoneNames)
            {
                if (BoneExceptions.ContainsKey(boneName))
                {
                    boneVerts[boneName] = BoneExceptions[boneName](boneSearcher);
                }
                else
                {
                    var searchResult = boneSearcher.GetObjectFromName(boneName);
                    if (searchResult != null)
                        boneVerts[boneName] = searchResult.transform.position;
                }
            }
            return boneVerts;
        }

        protected float GetDistanceInCm(Vector3 pointA, Vector3 pointB) =>
            // pulled 10.5 value from HeightBarX plugin
            Vector3.Distance(pointA, pointB) * 10.5f;

        protected float GetEllipseCircumference(float axis1, float axis2) =>
            (float)(2 * Math.PI * Math.Sqrt(
                (Math.Pow(axis1, 2) + Math.Pow(axis2, 2)) / 2
            ));

        protected void DebugLog(string name, float value)
        {
            if (MeasurementsPlugin.Configuration.DebugValues.Value)
                MeasurementsPlugin.Logger.LogInfo($"{name} = {value}");
        }
    }
}
