using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneViewOrbitController
    {
        public static void Handle(SceneView sv, Event e)
        {
            if (sv.in2DMode) return;

            if (!IsOrbitEvent(e)) return;

            Orbit(sv, e);
            e.Use();
        }

        private static void Orbit(SceneView sv, Event e)
        {
            float sensitivity = 0.2f;

            Quaternion rot = sv.rotation;

            Vector3 sceneUp = Quaternion.AngleAxis(SceneViewState.SceneZRotation, Vector3.forward) * Vector3.up;
            Vector3 sceneRight = Vector3.Cross(sceneUp, Vector3.forward).normalized;

            float dx = e.delta.x * sensitivity;
            float dy = e.delta.y * sensitivity;

            Quaternion yaw   = Quaternion.AngleAxis(dx, sceneUp);
            Quaternion pitch = Quaternion.AngleAxis(dy, sceneRight);

            Quaternion newRot = yaw * pitch * rot;
            Vector3 forward = newRot * Vector3.forward;

            sv.rotation = Quaternion.LookRotation(forward, sceneUp);
        }

        private static bool IsOrbitEvent(Event e)
        {
            if (e.type != EventType.MouseDrag)
                return false;

            // Alt + LMB or RMB drag = orbit in SceneView
            return (e.alt && e.button == 0) || e.button == 1;
        }
    }
}