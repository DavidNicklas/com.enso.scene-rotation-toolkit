using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public class SRT_OrbitController
    {
        private int orbitControlId;
        private float yawSign = 1f;
        private const float SENSITIVITY = 0.003f * Mathf.Rad2Deg;

        public bool IsRelevant(Event e)
        {
            return e.alt && !e.shift && e.button == 0 &&
                   (e.type == EventType.Layout ||
                    e.type == EventType.MouseDown ||
                    e.type == EventType.MouseDrag ||
                    e.type == EventType.MouseUp ||
                    e.type == EventType.MouseLeaveWindow ||
                    e.type == EventType.Ignore);
        }

        public void Handle(SceneView sv, Event e)
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

        private bool EndOrbitOutOfSceneView(Event e)
        {
            return GUIUtility.hotControl == orbitControlId &&
                   (!e.alt || e.type == EventType.MouseLeaveWindow || e.type == EventType.Ignore);
        }

        private bool IsStart(Event e) => e.type == EventType.MouseDown && e.alt && !e.shift && e.button == 0;
        private bool IsDrag(Event e)  => e.type == EventType.MouseDrag && e.alt && !e.shift && e.button == 0;
        private bool IsEnd(Event e)   => e.type == EventType.MouseUp   && e.alt && !e.shift && e.button == 0;

        private void PerformOrbit(SceneView sv, Event e)
        {
            Quaternion rot = sv.rotation;

            Vector3 sceneUp = Quaternion.AngleAxis(SRT_SceneViewState.SceneZRotation, Vector3.forward) * Vector3.up;

            // Pitch around camera-local right
            rot = Quaternion.AngleAxis(e.delta.y * SENSITIVITY, rot * Vector3.right) * rot;

            // Yaw around sceneUp with sign flip
            rot = Quaternion.AngleAxis(yawSign * e.delta.x * SENSITIVITY, sceneUp) * rot;

            sv.rotation = rot;
        }

        private void UpdateYawSign(SceneView sv)
        {
            Vector3 sceneUp = Quaternion.AngleAxis(SRT_SceneViewState.SceneZRotation, Vector3.forward) * Vector3.up;

            yawSign = Mathf.Sign(Vector3.Dot(sv.rotation * Vector3.up, sceneUp));
            if (Mathf.Approximately(yawSign, 0f)) yawSign = 1f;
        }

        private void ForceEnd(Event e)
        {
            GUIUtility.hotControl = 0;
            ToolUtility.RestorePreviousToolState();
            EditorGUIUtility.SetWantsMouseJumping(0);
            e.Use();
        }
    }
}