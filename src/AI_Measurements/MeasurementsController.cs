using AIChara;
using KKAPI;
using KKAPI.Chara;
using KKAPI.Maker;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI_Measurements
{
    public class MeasurementsController : CharaCustomFunctionController
    {
        public bool UseMetricUnits { get; internal set; }
        public int Region { get; internal set; }

        private static readonly Dictionary<Bones, string> s_measurementBones = Enum.GetValues(typeof(Bones))
            .Cast<Bones>()
            .ToDictionary(bone => bone, bone => Enum.GetName(typeof(Bones), bone));

        private readonly FindAssist _boneSearcher = new FindAssist();
        private bool _initialized;

        protected override void OnCardBeingSaved(GameMode currentGameMode) { }

        public void Initialize()
        {
            if (!_initialized)
            {
                _boneSearcher.Initialize(ChaControl.transform);
                _initialized = true;
            }
        }

        protected override void OnReload(GameMode currentGameMode)
        {
            if (MakerAPI.InsideAndLoaded)
            {
                UpdateTexts();
            }
            base.OnReload(currentGameMode);
        }

        internal Dictionary<Bones, Vector3> GetBoneVertices()
        {
            var boneVerts = new Dictionary<Bones, Vector3>();
            foreach (var bone in s_measurementBones)
            {
                if (bone.Key == Bones.cf_J_Spine02_s_Scale)
                {
                    HandleSpine02Scale();
                    continue;
                }

                var searchResult = _boneSearcher.GetObjectFromName(bone.Value);
                if (searchResult != null)
                    boneVerts[bone.Key] = searchResult.transform.position;
            }
            return boneVerts;

            void HandleSpine02Scale()
            {
                var chestScale = _boneSearcher.GetObjectFromName(nameof(Bones.cf_J_Spine02_s));
                if (chestScale != null)
                    boneVerts[Bones.cf_J_Spine02_s_Scale] = chestScale.transform.localScale;
            }
        }

        internal MeasurementsData GetMeasurements(Dictionary<Bones, Vector3> boneVerts)
        {
            return new MeasurementsData
            {
                Band = GetBand(),
                Bust = GetBust(),
                Dick = GetDick(),
                Height = GetHeight(),
                Hips = GetHips(),
                Waist = GetWaist(),
            };

            float GetHeight()
            {
                return MeasurementsCalculator.CalculateHeight(boneVerts[Bones.N_Head_top]);
            }

            float GetBust()
            {
                var leftSideBoob = MeasurementsCalculator.GetLeftSideBoob(
                    titCenter: boneVerts[Bones.cf_J_Mune02_L],
                    nipple: boneVerts[Bones.cf_J_Mune_Nip01_L],
                    targetY: (boneVerts[Bones.N_Back_L].y + boneVerts[Bones.cf_J_Mune02_L].y) / 2
                );
                var rightSideBoob = MeasurementsCalculator.GetLeftSideBoob(
                    titCenter: boneVerts[Bones.cf_J_Mune02_R],
                    nipple: boneVerts[Bones.cf_J_Mune_Nip01_R],
                    targetY: (boneVerts[Bones.N_Back_R].y + boneVerts[Bones.cf_J_Mune02_R].y) / 2
                );
                return MeasurementsCalculator.CalculateBust(
                    rightTit: new TitMeasurement
                    {
                        Nipple = boneVerts[Bones.cf_J_Mune_Nip01_R],
                        SideBoob = rightSideBoob,
                        Lat = boneVerts[Bones.N_Back_R],
                    },
                    leftTit: new TitMeasurement
                    {
                        Nipple = boneVerts[Bones.cf_J_Mune_Nip01_L],
                        SideBoob = leftSideBoob,
                        Lat = boneVerts[Bones.N_Back_L],
                    });
            }

            float GetBand()
            {
                const float BAND_RATIO_FRONT = 0.8095239f;
                const float BAND_RATIO_BACK = -0.8571428665f;
                const float BAND_RATIO_SIDE = 1.09523809f;

                var spine02 = boneVerts[Bones.cf_J_Spine02_s];
                var spine03 = boneVerts[Bones.cf_J_Spine03_s];
                var bandScale = boneVerts[Bones.cf_J_Spine02_s_Scale];

                var front = new Vector3((spine02.x + spine03.x) / 2, spine02.y, bandScale.z * BAND_RATIO_FRONT);
                var back = new Vector3(front.x, (front.y + spine03.y) / 2, bandScale.z * BAND_RATIO_BACK);
                var right = new Vector3(bandScale.x * BAND_RATIO_SIDE, (front.y + back.y) / 2, (front.y + back.y) / 2);
                var left = new Vector3(-1 * right.x, right.y, right.z);

                return MeasurementsCalculator.CalculateBand(front, back, left, right);
            }

            float GetWaist() => MeasurementsCalculator.CalculateWaist(
                boneVerts[Bones.N_Waist_L], boneVerts[Bones.N_Waist_R],
                boneVerts[Bones.N_Waist_f], boneVerts[Bones.N_Waist_b]);

            float GetHips() => MeasurementsCalculator.CalculateHips(
                // TODO find way to adjust Ass vector for butt size as the Legsk_0(3|5) bones don't adjust when ass size changes
                leftHip: new HipMeasurement
                {
                    Front = boneVerts[Bones.cf_J_Legsk_07_00],
                    Side = boneVerts[Bones.cf_J_Legsk_06_00],
                    Ass = boneVerts[Bones.cf_J_Legsk_05_00],
                },
                rightHip: new HipMeasurement
                {
                    Front = boneVerts[Bones.cf_J_Legsk_01_00],
                    Side = boneVerts[Bones.cf_J_Legsk_02_00],
                    Ass = boneVerts[Bones.cf_J_Legsk_03_00],
                });

            float GetDick()
            {
                return MeasurementsCalculator.CalculateDick(
                    boneVerts[Bones.cm_J_dan100_00],
                    boneVerts[Bones.cm_J_dan109_00]);
            }
        }

        internal void UpdateTexts()
        {
            if (!_initialized) Initialize();

            var boneVerts = GetBoneVertices();
            var data = GetMeasurements(boneVerts);
            foreach (var gui in MeasurementsGui.TextGuis)
            {
                gui.SetVisibility();
                if (gui.IsVisible())
                    gui.Update(data, this);
            }

            if (MeasurementsPlugin.DebugValues.Value)
            {
                MeasurementsPlugin.Logger.LogInfo($"Height = {data.Height}");
                MeasurementsPlugin.Logger.LogInfo($"Bust = {data.Bust}");
                MeasurementsPlugin.Logger.LogInfo($"Band = {data.Band}");
                MeasurementsPlugin.Logger.LogInfo($"Waist = {data.Waist}");
                MeasurementsPlugin.Logger.LogInfo($"Hips = {data.Hips}");
                MeasurementsPlugin.Logger.LogInfo($"Dick = {data.Dick}");
            }
        }
    }
}
