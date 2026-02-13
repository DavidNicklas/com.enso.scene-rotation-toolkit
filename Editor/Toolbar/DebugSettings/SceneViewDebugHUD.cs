using System;
using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneViewDebugHUD
    {
        private static bool enabled;

        private const float PIVOT_GIZMO_THICKNESS = 1.5f;

        public static void Toggle()
        {
            enabled = !enabled;

            if (enabled) SceneView.duringSceneGui += Draw;
            else SceneView.duringSceneGui -= Draw;
        }

        private static void Draw(SceneView sv)
        {
            if (sv == null) return;

            var cam = sv.camera;
            if (cam == null) return;

            if (SceneViewDebugSettings.ShowPivotGizmo) DrawPivotGizmo(sv);
            if (SceneViewDebugSettings.ShowRotationBadge) DrawRotationBadge(sv);
            if (SceneViewDebugSettings.ShowHud) DrawHUD(sv);
        }

        /// <summary>
        /// Draws the pivot of the camera with corresponding up and right vector and its custom axes
        /// </summary>
        /// <param name="sv"></param>
        private static void DrawPivotGizmo(SceneView sv)
        {
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;

            Vector3 p = sv.pivot;

            Handles.Label(p + Vector3.down, "Camera Pivot");

            // Cross at pivot
            float s = HandleUtility.GetHandleSize(p) * 0.25f;
            Handles.DrawLine(p + Vector3.right * s, p - Vector3.right * s, PIVOT_GIZMO_THICKNESS);
            Handles.DrawLine(p + Vector3.up * s, p - Vector3.up * s, PIVOT_GIZMO_THICKNESS);
            Handles.DrawLine(p + Vector3.forward * s, p - Vector3.forward * s, PIVOT_GIZMO_THICKNESS);

            // Local axes at pivot based on SceneView rotation
            Quaternion r = sv.rotation;
            float a = HandleUtility.GetHandleSize(p);// * 0.25f;
            Handles.ArrowHandleCap(0, p, Quaternion.LookRotation(r * Vector3.right), a, EventType.Repaint);
            Handles.ArrowHandleCap(0, p, Quaternion.LookRotation(r * Vector3.up), a, EventType.Repaint);
            Handles.ArrowHandleCap(0, p, Quaternion.LookRotation(r * Vector3.forward), a, EventType.Repaint);
        }

        /// <summary>
        /// Draws a small rectangle with a label and a color of the current rotation
        /// </summary>
        /// <param name="sv"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static void DrawRotationBadge(SceneView sv)
        {
            Handles.BeginGUI();
            var style = new GUIStyle(EditorStyles.miniLabel)
            {
                richText = false,
                alignment =  TextAnchor.MiddleCenter,
                wordWrap = true
            };

            var rotationState = SceneViewState.GetRotationState();

            // Background box dimension
            float width = Mathf.Min(100, sv.position.width - 20);
            var rect = new Rect((sv.position.width / 2) - (width / 2), 10, width, 0)
            {
                height = style.CalcHeight(new GUIContent(rotationState.ToString()), width) + 12
            };

            DrawOutline(rect, 2f, Color.black);

            var bgColor = rotationState switch
            {
                RotationState.South => new Color(1, 0, 0.2f, SceneViewDebugSettings.HUDOpacity),
                RotationState.North => new Color(0, 0.8f, 1, SceneViewDebugSettings.HUDOpacity),
                RotationState.East => new Color(0.3f, 0, 0.7f, SceneViewDebugSettings.HUDOpacity),
                RotationState.West => new Color(0.7f, 0.5f, 0, SceneViewDebugSettings.HUDOpacity),
                _ => throw new ArgumentOutOfRangeException()
            };
            EditorGUI.DrawRect(rect, bgColor);

            // Text
            var textRect = new Rect(rect.x + 6, rect.y + 6, rect.width - 12, rect.height - 12);
            GUI.Label(textRect, rotationState.ToString(), style);

            Handles.EndGUI();
        }

        private static void DrawOutline(Rect r, float thickness, Color color)
        {
            // Top
            EditorGUI.DrawRect(new Rect(r.x, r.y, r.width, thickness), color);
            // Bottom
            EditorGUI.DrawRect(new Rect(r.x, r.yMax - thickness, r.width, thickness), color);
            // Left
            EditorGUI.DrawRect(new Rect(r.x, r.y, thickness, r.height), color);
            // Right
            EditorGUI.DrawRect(new Rect(r.xMax - thickness, r.y, thickness, r.height), color);
        }

        /// <summary>
        /// Draws alls the information inside a container in the scene view
        /// </summary>
        /// <param name="sv"></param>
        private static void DrawHUD(SceneView sv)
        {
            var cam = sv.camera;
            var e = Event.current;

            Vector3 camPos = cam.transform.position;
            Vector3 camFwd = cam.transform.forward;
            Vector3 camUp = cam.transform.up;
            Vector3 camRight = cam.transform.right;

            float camDist = Vector3.Distance(camPos, sv.pivot);

            Vector3 rotEuler = sv.rotation.eulerAngles;

            string zoomLine = $"CamDist: {camDist:0.###}";

            string toolState =
                $"EnableTool: {SceneViewState.EnableTool}   Fake2D: {SceneViewState.Is2DMode}   SceneZ: {SceneViewState.SceneZRotation:0}°";

            string inputState =
                $"Mouse: {e.type} btn:{e.button} alt:{e.alt} shift:{e.shift} ctrl:{e.control} cmd:{e.command} " +
                $"delta:{e.delta} pos:{e.mousePosition}";

            string basis =
                $"SV Rot Euler: {rotEuler.x:0.##}, {rotEuler.y:0.##}, {rotEuler.z:0.##}\n" +
                $"SV Forward: {FormatText(sv.rotation * Vector3.forward)}  Up: {FormatText(sv.rotation * Vector3.up)}  Right: {FormatText(sv.rotation * Vector3.right)}";

            string camBasis =
                $"Cam Pos: {FormatText(camPos)}\n" +
                $"Cam Forward: {FormatText(camFwd)}  Up: {FormatText(camUp)}  Right: {FormatText(camRight)}";

            string pivot =
                $"Pivot: {FormatText(sv.pivot)}";

            string mode =
                $"2D Mode (Unity): {sv.in2DMode}   Ortho: {sv.orthographic}   isRotationLocked: {sv.isRotationLocked}\n" +
                zoomLine;

            System.Text.StringBuilder sb = new System.Text.StringBuilder(512);

            if (SceneViewDebugSettings.HUDToolState) sb.AppendLine(toolState);
            if (SceneViewDebugSettings.HUDMode) sb.AppendLine(mode);
            if (SceneViewDebugSettings.HUDPivot) sb.AppendLine(pivot);
            if (SceneViewDebugSettings.HUDSceneBasis) sb.AppendLine(basis);
            if (SceneViewDebugSettings.HUDCameraBasis) sb.AppendLine(camBasis);
            if (SceneViewDebugSettings.HUDInputState) sb.AppendLine(inputState);

            string text = sb.ToString().TrimEnd();

            Handles.BeginGUI();
            var style = new GUIStyle(EditorStyles.miniLabel)
            {
                richText = false,
                wordWrap = true
            };

            // Background box
            float width = Mathf.Min(520, sv.position.width - 20);
            var rect = new Rect(10, 10, width, 0);
            rect.height = style.CalcHeight(new GUIContent(text), width) + 12;

            // Slight translucent background
            var bg = new Color(0, 0, 0, SceneViewDebugSettings.HUDOpacity);
            EditorGUI.DrawRect(rect, bg);

            // Text
            var textRect = new Rect(rect.x + 6, rect.y + 6, rect.width - 12, rect.height - 12);
            GUI.Label(textRect, text, style);

            Handles.EndGUI();
        }

        private static string FormatText(Vector3 v) => $"({v.x:0.###}, {v.y:0.###}, {v.z:0.###})";
    }
}
