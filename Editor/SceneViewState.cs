using System;
using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public enum RotationState { South, North, East, West }

    public static class SceneViewState
    {
        /// <summary>
        /// Stores the base Z rotation of the scene view camera
        /// </summary>
        public static float SceneZRotation { get; private set; }
        public static bool Is2DMode { get; private set; }
        public static bool EnableTool { get; private set; }

        public static event Action onChanged;

        private const float SOUTH_Z_ROTATION_ANGLE = 0f;
        private const float NORTH_Z_ROTATION_ANGLE = 180f;
        private const float EAST_Z_ROTATION_ANGLE = 90f;
        private const float WEST_Z_ROTATION_ANGLE = 270f;

        private static class SceneRotationPrefs
        {
            public const string Z_ROTATION = "SceneRotationToolkit.ZRotation";
            public const string IS_2D_MODE = "SceneRotationToolkit.Is2DMode";
            public const string ENABLE = "SceneRotationToolkit.Enable";
        }

        static SceneViewState()
        {
            Load();

            if (EnableTool) SceneViewController.Toggle(EnableTool);

            EditorApplication.delayCall += ApplyToLastSceneView;
        }

        private static void ApplyToLastSceneView()
        {
            SceneViewCameraUtility.ApplyState(SceneView.lastActiveSceneView, Is2DMode, SceneZRotation);
        }

        public static void SetRotation(RotationState stateEnum)
        {
            if (Mathf.Approximately(SceneZRotation, GetRotationValue(stateEnum))) return;

            SceneZRotation = GetRotationValue(stateEnum);
            Save();
            SceneViewCameraUtility.ApplyState(SceneView.lastActiveSceneView, Is2DMode, SceneZRotation, $"Rotate to: {stateEnum.ToString()}");
            onChanged?.Invoke();
        }

        public static void Toggle2DMode()
        {
            Is2DMode = !Is2DMode;
            Save();
            SceneViewCameraUtility.ApplyState(SceneView.lastActiveSceneView, Is2DMode, SceneZRotation, $"2D Mode: {Is2DMode}");
            onChanged?.Invoke();
        }

        public static void ToggleTool()
        {
            EnableTool = !EnableTool;
            ResetToDefaults();
            Save();

            SceneViewController.Toggle(EnableTool);

            onChanged?.Invoke();
        }

        private static void ResetToDefaults()
        {
            SceneZRotation = SOUTH_Z_ROTATION_ANGLE;
            Is2DMode = false;
            SceneViewCameraUtility.ApplyState(SceneView.lastActiveSceneView, Is2DMode, SceneZRotation);
        }

        /// <summary>
        /// Returns the Z-Angle Value for a corresponding Rotation State
        /// </summary>
        /// <param name="stateEnum">the rotation of which you want to get the angle</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static float GetRotationValue(RotationState stateEnum)
        {
            return stateEnum switch
            {
                RotationState.South => SOUTH_Z_ROTATION_ANGLE,
                RotationState.North => NORTH_Z_ROTATION_ANGLE,
                RotationState.East => EAST_Z_ROTATION_ANGLE,
                RotationState.West => WEST_Z_ROTATION_ANGLE,
                _ => throw new ArgumentOutOfRangeException(nameof(stateEnum), stateEnum, null)
            };
        }

        /// <summary>
        /// Returns the current rotation state of the scene view.
        /// </summary>
        /// <returns></returns>
        public static RotationState GetState()
        {
            if (Mathf.Approximately(SceneZRotation, GetRotationValue(RotationState.South)))
            {
                return RotationState.South;
            }
            if (Mathf.Approximately(SceneZRotation, GetRotationValue(RotationState.North)))
            {
                return RotationState.North;
            }

            if (Mathf.Approximately(SceneZRotation, GetRotationValue(RotationState.East)))
            {
                return RotationState.East;
            }

            if (Mathf.Approximately(SceneZRotation, GetRotationValue(RotationState.West)))
            {
                return RotationState.West;
            }

            return RotationState.South;
        }

        private static void Load()
        {
            Is2DMode = EditorPrefs.GetBool(SceneRotationPrefs.IS_2D_MODE, false);
            SceneZRotation = EditorPrefs.GetFloat(SceneRotationPrefs.Z_ROTATION, 0f);
            EnableTool = EditorPrefs.GetBool(SceneRotationPrefs.ENABLE, EnableTool);
        }

        private static void Save()
        {
            EditorPrefs.SetBool(SceneRotationPrefs.IS_2D_MODE, Is2DMode);
            EditorPrefs.SetFloat(SceneRotationPrefs.Z_ROTATION, SceneZRotation);
            EditorPrefs.SetBool(SceneRotationPrefs.ENABLE, EnableTool);
        }
    }
}

