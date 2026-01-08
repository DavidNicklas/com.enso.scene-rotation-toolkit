using System;
using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneRotationPrefs
    {
        public const string Z_ROTATION = "SceneRotationToolkit.ZRotation";
        public const string FAKE_2D = "SceneRotationToolkit.Fake2D";
    }

    public static class SceneViewState
    {
        /// <summary>
        /// Stores the base Z rotation of the scene view camera
        /// </summary>
        public static float SceneZRotation { get; private set; }
        public static bool Fake2DMode { get; private set; }

        public static event Action onChanged;

        static SceneViewState()
        {
            Load();
            EditorApplication.delayCall += ApplyToLastSceneView;
        }

        private static void ApplyToLastSceneView()
        {
            Apply(SceneView.lastActiveSceneView);
        }

        public static void SetRotation(float zRotation)
        {
            if (Mathf.Approximately(SceneZRotation, zRotation))
            {
                return;
            }

            SceneZRotation = zRotation;
            Save();
            Apply(SceneView.lastActiveSceneView, $"Rotate to: {SceneZRotation}°");
            onChanged?.Invoke();
        }

        public static void ToggleFake2DMode()
        {
            Fake2DMode = !Fake2DMode;
            Save();
            Apply(SceneView.lastActiveSceneView, $"2D Mode: {Fake2DMode}");
            onChanged?.Invoke();
        }

        /// <summary>
        /// Applies the new rotation to the scene view camera
        /// </summary>
        /// <param name="sv">the scene view to apply the rotation to</param>
        /// <param name="notificationMessage">message to show</param>
        private static void Apply(SceneView sv, string notificationMessage = "")
        {
            if (sv == null) return;

            sv.in2DMode = false;
            sv.orthographic = Fake2DMode;

            Vector3 forward = Vector3.forward;
            Vector3 up = Quaternion.AngleAxis(SceneZRotation, forward) * Vector3.up;

            sv.LookAt(
                sv.pivot,
                Quaternion.LookRotation(forward, up),
                sv.size,
                sv.orthographic,
                false
            );

            if (!string.IsNullOrEmpty(notificationMessage))
            {
                sv.ShowNotification(new GUIContent(notificationMessage), 1.5f);
            }
            sv.Repaint();
        }

        private static void Load()
        {
            Fake2DMode = EditorPrefs.GetBool(SceneRotationPrefs.FAKE_2D, false);
            SceneZRotation = EditorPrefs.GetFloat(SceneRotationPrefs.Z_ROTATION, 0f);
        }

        private static void Save()
        {
            EditorPrefs.SetBool(SceneRotationPrefs.FAKE_2D, Fake2DMode);
            EditorPrefs.SetFloat(SceneRotationPrefs.Z_ROTATION, SceneZRotation);
        }
    }
}