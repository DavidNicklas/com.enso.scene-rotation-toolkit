using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneViewOrbitController
    {
        private static int orbitControlId;
        private static float yawSign = 1f;

        public static void Handle(SceneView sv, Event e)
        {
            if (sv.in2DMode) return;

            // Reserve Alt+Shift+LMB for RectTool / handle modifications
            if (e.alt && e.shift && e.button == 0) return;

            // Create a stable control id
            if (orbitControlId == 0) orbitControlId = GUIUtility.GetControlID(FocusType.Passive);

            bool wantsOrbit = WantsOrbit(e);

            // During layout, declare "if orbit happens, route events to us"
            if (e.type == EventType.Layout)
            {
                if (wantsOrbit) HandleUtility.AddDefaultControl(orbitControlId);
                return;
            }

            // If another control is actively dragging and it's not us -> don't fight it
            if (GUIUtility.hotControl != 0 && GUIUtility.hotControl != orbitControlId) return;

            if (IsOrbitStart(e))
            {
                GUIUtility.hotControl = orbitControlId;
                UpdateYawSign(sv);
                ToolUtility.StoreCurrentToolState();
                EditorGUIUtility.SetWantsMouseJumping(1);
                e.Use();
                return;
            }

            if (IsOrbitDrag(e) && GUIUtility.hotControl == orbitControlId)
            {
                Orbit(sv, e);
                ToolUtility.SetTool(Tool.View, ViewTool.Orbit);
                e.Use();
                sv.Repaint();
                return;
            }

            if (IsOrbitEnd(e) && GUIUtility.hotControl == orbitControlId)
            {
                GUIUtility.hotControl = 0;
                ToolUtility.RestorePreviousToolState();
                EditorGUIUtility.SetWantsMouseJumping(0);
                e.Use();
            }
        }

        private static bool WantsOrbit(Event e)
        {
            if (e.alt && !e.shift && e.button == 0) return true;
            if (e.button == 1) return true;
            return false;
        }

        private static bool IsOrbitStart(Event e)
        {
            if (e.type != EventType.MouseDown) return false;
            return (e.alt && !e.shift && e.button == 0) || e.button == 1;
        }

        private static bool IsOrbitDrag(Event e)
        {
            if (e.type != EventType.MouseDrag) return false;
            return (e.alt && !e.shift && e.button == 0) || e.button == 1;
        }

        private static bool IsOrbitEnd(Event e)
        {
            if (e.type != EventType.MouseUp) return false;
            return (e.alt && !e.shift && e.button == 0) || e.button == 1;
        }

        private static void Orbit(SceneView sv, Event e)
        {
            float scaling = 0.003f * Mathf.Rad2Deg;

            Quaternion sceneRotation = sv.rotation;

            Vector3 sceneUp = Quaternion.AngleAxis(SceneViewState.SceneZRotation, Vector3.forward) * Vector3.up;

            // Pitch around camera-local right
            sceneRotation = Quaternion.AngleAxis(e.delta.y * scaling, sceneRotation * Vector3.right) * sceneRotation;

            // Yaw around sceneUp with sign flip
            sceneRotation = Quaternion.AngleAxis(yawSign * e.delta.x * scaling, sceneUp) * sceneRotation;

            sv.rotation = sceneRotation;
        }

        private static void UpdateYawSign(SceneView sv)
        {
            Vector3 sceneUp = Quaternion.AngleAxis(SceneViewState.SceneZRotation, Vector3.forward) * Vector3.up;

            yawSign = Mathf.Sign(Vector3.Dot(sv.rotation * Vector3.up, sceneUp));
            if (Mathf.Approximately(yawSign, 0f)) yawSign = 1f;
        }
    }
}
