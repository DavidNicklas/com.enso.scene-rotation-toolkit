using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneViewOrbitController
    {
        private static float yawSign = 1f;

        public static void Handle(SceneView sv, Event e)
        {
            if (sv.in2DMode) return;

            if (IsOrbitStart(e))
            {
                UpdateYawSign(sv);
                return;
            }

            if (!IsOrbitDrag(e)) return;

            Orbit(sv, e);
            e.Use();
        }

        private static void Orbit(SceneView sv, Event e)
        {
            // Same style of scaling used in UnityCsReference / common editor scripts
            float scaling = 0.003f * Mathf.Rad2Deg;

            Quaternion sceneRotation = sv.rotation;

            Vector3 sceneUp = Quaternion.AngleAxis(SceneViewState.SceneZRotation, Vector3.forward) * Vector3.up;

            // Pitch around camera-local right axis
            sceneRotation = Quaternion.AngleAxis(e.delta.y * scaling, sceneRotation * Vector3.right) * sceneRotation;

            // Yaw around (possibly flipped) up axis
            sceneRotation = Quaternion.AngleAxis(yawSign * e.delta.x * scaling, sceneUp) * sceneRotation;

            sv.rotation = sceneRotation;
        }

        private static void UpdateYawSign(SceneView sv)
        {
            Vector3 sceneUp = Quaternion.AngleAxis(SceneViewState.SceneZRotation, Vector3.forward) * Vector3.up;

            // When the camera is upside down relative to sceneUp, Unity flips horizontal orbit direction
            // to avoid mirrored left/right feel.
            yawSign = Mathf.Sign(Vector3.Dot(sv.rotation * Vector3.up, sceneUp));
            if (Mathf.Approximately(yawSign, 0f)) yawSign = 1f;
        }

        private static bool IsOrbitStart(Event e)
        {
            if (e.type != EventType.MouseDown) return false;
            return (e.alt && e.button == 0) || e.button == 1;
        }

        private static bool IsOrbitDrag(Event e)
        {
            if (e.type != EventType.MouseDrag) return false;
            return (e.alt && e.button == 0) || e.button == 1;
        }
    }
}
