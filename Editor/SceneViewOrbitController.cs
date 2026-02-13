using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneViewOrbitController
    {
        private static int orbitControlId;
        private static float yawSign = 1f;
        private const float SENSITIVITY = 0.003f * Mathf.Rad2Deg;

        public static bool IsRelevant(Event e)
        {
            return e.alt && !e.shift && e.button == 0 &&
                   (e.type == EventType.Layout ||
                    e.type == EventType.MouseDown ||
                    e.type == EventType.MouseDrag ||
                    e.type == EventType.MouseUp ||
                    e.type == EventType.MouseLeaveWindow ||
                    e.type == EventType.Ignore);
        }

        public static void Handle(SceneView sv, Event e)
        {
            if (orbitControlId == 0)
                orbitControlId = GUIUtility.GetControlID(FocusType.Passive);

            // During layout, declare "if orbit happens, route events to this class"
            if (e.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(orbitControlId);
                return;
            }

            if (EndOrbitOutOfSceneView(e))
            {
                ForceEnd(e);
                return;
            }

            // If another control is actively dragging, and it's not us -> don't fight it
            if (GUIUtility.hotControl != 0 && GUIUtility.hotControl != orbitControlId) return;

            if (IsStart(e))
            {
                GUIUtility.hotControl = orbitControlId;
                UpdateYawSign(sv);
                ToolUtility.StoreCurrentToolState();
                EditorGUIUtility.SetWantsMouseJumping(1);
                e.Use();
                return;
            }

            if (IsDrag(e) && GUIUtility.hotControl == orbitControlId)
            {
                PerformOrbit(sv, e);
                ToolUtility.SetTool(Tool.View, ViewTool.Orbit);
                e.Use();
                sv.Repaint();
                return;
            }

            if (IsEnd(e))
            {
                ForceEnd(e);
            }
        }

        private static bool EndOrbitOutOfSceneView(Event e)
        {
            return GUIUtility.hotControl == orbitControlId &&
                   (!e.alt || e.type == EventType.MouseLeaveWindow || e.type == EventType.Ignore);
        }

        private static bool IsStart(Event e) => e.type == EventType.MouseDown && e.alt && !e.shift && e.button == 0;
        private static bool IsDrag(Event e)  => e.type == EventType.MouseDrag && e.alt && !e.shift && e.button == 0;
        private static bool IsEnd(Event e)   => e.type == EventType.MouseUp   && e.alt && !e.shift && e.button == 0;

        private static void PerformOrbit(SceneView sv, Event e)
        {
            Quaternion rot = sv.rotation;

            Vector3 sceneUp = Quaternion.AngleAxis(SceneViewState.SceneZRotation, Vector3.forward) * Vector3.up;

            // Pitch around camera-local right
            rot = Quaternion.AngleAxis(e.delta.y * SENSITIVITY, rot * Vector3.right) * rot;

            // Yaw around sceneUp with sign flip
            rot = Quaternion.AngleAxis(yawSign * e.delta.x * SENSITIVITY, sceneUp) * rot;

            sv.rotation = rot;
        }

        private static void UpdateYawSign(SceneView sv)
        {
            Vector3 sceneUp = Quaternion.AngleAxis(SceneViewState.SceneZRotation, Vector3.forward) * Vector3.up;

            yawSign = Mathf.Sign(Vector3.Dot(sv.rotation * Vector3.up, sceneUp));
            if (Mathf.Approximately(yawSign, 0f)) yawSign = 1f;
        }

        private static void ForceEnd(Event e)
        {
            GUIUtility.hotControl = 0;
            ToolUtility.RestorePreviousToolState();
            EditorGUIUtility.SetWantsMouseJumping(0);
            e.Use();
        }
    }
}