using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneViewCameraUtility
    {
        /// <summary>
        /// Applies the new rotation to the scene view camera
        /// </summary>
        /// <param name="sv">the scene view to apply the rotation to</param>
        /// <param name="targetRotation">the target rotation</param>
        /// <param name="notificationMessage">message to show</param>
        /// <param name="is2DMode">if the camera is in 2D mode or not</param>
        public static void ApplyState(SceneView sv, bool is2DMode, float targetRotation, string notificationMessage = "")
        {
            if (sv == null) return;

            sv.in2DMode = false;
            sv.isRotationLocked = is2DMode;
            sv.orthographic = is2DMode;

            sv.TryGetOverlay("Orientation", out var overlay);
            if (overlay != null) overlay.displayed = !is2DMode;

            Vector3 forward = Vector3.forward;
            Vector3 up = Quaternion.AngleAxis(targetRotation, forward) * Vector3.up;

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
    }
}