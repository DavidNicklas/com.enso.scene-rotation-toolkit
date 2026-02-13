using System;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public enum RotationState { South, North, East, West }

    public readonly struct SceneViewStateSnapshot
    {
        public readonly float sceneZRotation;
        public readonly bool is2DMode;
        public readonly bool enableTool;

        public SceneViewStateSnapshot(float sceneZRotation, bool is2DMode, bool enableTool)
        {
            this.sceneZRotation = sceneZRotation;
            this.is2DMode = is2DMode;
            this.enableTool = enableTool;
        }
    }

    public static class SRT_SceneViewState
    {
        public const float SOUTH_Z_ROTATION_ANGLE = 0f;
        private const float EAST_Z_ROTATION_ANGLE  = 90f;
        private const float NORTH_Z_ROTATION_ANGLE = 180f;
        private const float WEST_Z_ROTATION_ANGLE  = 270f;

        public static float SceneZRotation { get; private set; } = SOUTH_Z_ROTATION_ANGLE;
        public static bool Is2DMode { get; private set; }
        public static bool EnableTool { get; private set; }

        public static event Action onChanged;

        public static SceneViewStateSnapshot PreviousState { get; private set; }

        public static void SetRotation(RotationState rotation)
        {
            float value = GetRotationValue(rotation);
            if (Mathf.Approximately(SceneZRotation, value)) return;

            PreviousState = CreateSnapshot();
            SceneZRotation = value;
            RaiseChanged();
        }

        public static void Toggle2DMode()
        {
            PreviousState = CreateSnapshot();
            Is2DMode = !Is2DMode;
            RaiseChanged();
        }

        public static void ToggleTool()
        {
            PreviousState = CreateSnapshot();
            EnableTool = !EnableTool;

            if (!EnableTool)
            {
                SceneZRotation = SOUTH_Z_ROTATION_ANGLE;
                Is2DMode = false;
            }

            RaiseChanged();
        }

        public static void SetState(float rotationValue, bool is2DMode, bool enable)
        {
            bool changed =
                !Mathf.Approximately(SceneZRotation, rotationValue) ||
                Is2DMode != is2DMode ||
                EnableTool != enable;

            if (!changed) return;

            PreviousState = CreateSnapshot();

            SceneZRotation = rotationValue;
            Is2DMode = is2DMode;
            EnableTool = enable;

            if (!EnableTool)
            {
                SceneZRotation = SOUTH_Z_ROTATION_ANGLE;
                Is2DMode = false;
            }

            RaiseChanged();
        }

        public static float GetRotationValue(RotationState stateEnum)
        {
            return stateEnum switch
            {
                RotationState.South => SOUTH_Z_ROTATION_ANGLE,
                RotationState.East  => EAST_Z_ROTATION_ANGLE,
                RotationState.North => NORTH_Z_ROTATION_ANGLE,
                RotationState.West  => WEST_Z_ROTATION_ANGLE,
                _ => SOUTH_Z_ROTATION_ANGLE
            };
        }

        public static RotationState GetRotationState()
        {
            float z = SceneZRotation;

            if (Mathf.Approximately(z, SOUTH_Z_ROTATION_ANGLE)) return RotationState.South;
            if (Mathf.Approximately(z, EAST_Z_ROTATION_ANGLE))  return RotationState.East;
            if (Mathf.Approximately(z, NORTH_Z_ROTATION_ANGLE)) return RotationState.North;
            if (Mathf.Approximately(z, WEST_Z_ROTATION_ANGLE))  return RotationState.West;
            return RotationState.South;
        }

        private static SceneViewStateSnapshot CreateSnapshot()
        {
            return new SceneViewStateSnapshot(SceneZRotation, Is2DMode, EnableTool);
        }

        private static void RaiseChanged()
        {
            onChanged?.Invoke();
        }
    }
}