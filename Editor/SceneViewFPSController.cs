using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneViewFPSController
    {
        private static int fpsControlId;

        public static bool IsActive { get; private set; }

        private static bool wKey, aKey, sKey, dKey, qKey, eKey, shiftKey;

        private static Quaternion rot;
        private static double lastTime;

        private const float MOUSE_SENSITIVITY = 0.003f * Mathf.Rad2Deg;
        private const float BASE_SPEED = 7.0f;
        private const float SHIFT_MULTIPLIER = 3.0f;
        private const float THRESHOLD = 0.0001f;

        public static bool IsRelevant(Event e)
        {
            // RMB mouse events (start/drag/end) + key events while active
            if (e.button == 1 &&
                (e.type == EventType.Layout ||
                 e.type == EventType.MouseDown ||
                 e.type == EventType.MouseDrag ||
                 e.type == EventType.MouseUp ||
                 e.type == EventType.MouseLeaveWindow ||
                 e.type == EventType.Ignore))
                return true;

            if (IsActive && (e.type == EventType.KeyDown || e.type == EventType.KeyUp))
                return true;

            return false;
        }

        public static bool Handle(SceneView sv, Event e)
        {
            if (fpsControlId == 0)
                fpsControlId = GUIUtility.GetControlID(FocusType.Passive);

            if (e.type == EventType.Layout)
            {
                // Claim while active, and also on RMB layout to ensure we receive the RMB down properly.
                if (IsActive || e.button == 1)
                {
                    HandleUtility.AddDefaultControl(fpsControlId);
                }

                return IsActive;
            }

            if (IsStart(e))
            {
                Start(sv);
                e.Use();
                sv.Repaint();
                return true;
            }

            if (IsActive && IsEnd(e))
            {
                End();
                e.Use();
                sv.Repaint();
                return true;
            }

            if (IsActive && (e.type == EventType.KeyDown || e.type == EventType.KeyUp))
            {
                if (IsMovementKey(e))
                {
                    UpdateKeyState(e);
                    e.Use();
                    sv.Repaint();
                }

                return true;
            }

            if (IsActive && IsDrag(e) && GUIUtility.hotControl == fpsControlId)
            {
                ToolUtility.SetTool(Tool.View, ViewTool.FPS);
                ApplyMouseLook(sv, e);
                return true;
            }

            return IsActive;
        }

        private static bool IsStart(Event e) => e.button == 1 && e.type == EventType.MouseDown;
        private static bool IsDrag(Event e) => e.button == 1 && e.type == EventType.MouseDrag;
        private static bool IsEnd(Event e) => e.button == 1 && (e.type == EventType.MouseUp || e.type == EventType.MouseLeaveWindow || e.type == EventType.Ignore);

        private static bool IsMovementKey(Event e)
        {
            return e.keyCode switch
            {
                KeyCode.W or KeyCode.A or KeyCode.S or KeyCode.D or KeyCode.Q or KeyCode.E or KeyCode.LeftShift
                    or KeyCode.RightShift => true,
                _ => false
            };
        }

        private static void UpdateKeyState(Event e)
        {
            bool down = e.type == EventType.KeyDown;

            switch (e.keyCode)
            {
                case KeyCode.W: wKey = down; break;
                case KeyCode.A: aKey = down; break;
                case KeyCode.S: sKey = down; break;
                case KeyCode.D: dKey = down; break;
                case KeyCode.Q: qKey = down; break;
                case KeyCode.E: eKey = down; break;
                case KeyCode.LeftShift or KeyCode.RightShift: shiftKey = down; break;
            }
        }

        private static void Start(SceneView sv)
        {
            if (IsActive) return;

            IsActive = true;

            GUIUtility.hotControl = fpsControlId;

            ToolUtility.StoreCurrentToolState();
            ToolUtility.SetTool(Tool.View, ViewTool.FPS);
            EditorGUIUtility.SetWantsMouseJumping(1);

            rot = sv.rotation;

            lastTime = EditorApplication.timeSinceStartup;

            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
        }

        private static void End()
        {
            if (!IsActive) return;

            IsActive = false;

            GUIUtility.hotControl = 0;

            EditorGUIUtility.SetWantsMouseJumping(0);
            ToolUtility.RestorePreviousToolState();

            wKey = aKey = sKey = dKey = qKey = eKey = shiftKey = false;

            EditorApplication.update -= Tick;
        }

        private static void Tick()
        {
            if (!IsActive) return;

            var sv = SceneView.lastActiveSceneView;
            if (sv == null) return;

            double now = EditorApplication.timeSinceStartup;
            float dt = (float)(now - lastTime);
            lastTime = now;
            if (dt <= 0f) return;

            Vector3 sceneUp = GetSceneUp();

            // Forward on the sceneUp-plane
            Vector3 camForward = rot * Vector3.forward;
            Vector3 forward = Vector3.ProjectOnPlane(camForward, sceneUp);
            if (forward.sqrMagnitude < THRESHOLD) return;
            forward.Normalize();

            Vector3 right = Vector3.Cross(sceneUp, forward).normalized;

            Vector3 dir = Vector3.zero;
            if (wKey) dir += forward;
            if (sKey) dir -= forward;
            if (dKey) dir += right;
            if (aKey) dir -= right;
            if (eKey) dir += sceneUp;
            if (qKey) dir -= sceneUp;

            if (dir.sqrMagnitude < THRESHOLD) return;
            dir.Normalize();

            float speed = BASE_SPEED * (shiftKey ? SHIFT_MULTIPLIER : 1f);

            // Keep camera position fixed while adjusting pivot
            Vector3 camPos = sv.camera.transform.position;
            float dist = Vector3.Distance(sv.pivot, camPos);
            if (dist < THRESHOLD) dist = THRESHOLD;

            camPos += dir * speed * dt;

            sv.rotation = rot;
            sv.pivot = camPos + (rot * Vector3.forward) * dist;

            sv.Repaint();
        }

        private static void ApplyMouseLook(SceneView sv, Event e)
        {
            if (e.type != EventType.MouseDrag) return;

            // Cache camera world position so we can keep it fixed
            Vector3 camPos = sv.camera.transform.position;
            float dist = Vector3.Distance(sv.pivot, camPos);
            if (dist < THRESHOLD) dist = THRESHOLD;

            Vector3 sceneUp = GetSceneUp();

            // Yaw around sceneUp (mouse X)
            rot = Quaternion.AngleAxis(e.delta.x * MOUSE_SENSITIVITY, sceneUp) * rot;

            // Pitch around stable right axis (prevents 90/270 "only vertical" movement)
            Vector3 right = rot * Vector3.right;
            rot = Quaternion.AngleAxis(e.delta.y * MOUSE_SENSITIVITY, right) * rot;

            sv.rotation = rot;
            sv.pivot = camPos + (rot * Vector3.forward) * dist;

            e.Use();
            sv.Repaint();
        }

        private static Vector3 GetSceneUp()
        {
            return Quaternion.AngleAxis(SceneViewState.SceneZRotation, Vector3.forward) * Vector3.up;
        }
    }
}