using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public class SRT_FPSController
    {
        private int fpsControlId;

        public bool IsActive { get; private set; }

        private bool wKey, aKey, sKey, dKey, qKey, eKey, shiftKey;

        private Quaternion rot;
        private double lastTime;

        private const float MOUSE_SENSITIVITY = 0.003f * Mathf.Rad2Deg;
        private const float BASE_SPEED = 4.0f;
        private const float SHIFT_MULTIPLIER = 3.0f;
        private const float THRESHOLD = 0.0001f;

        private float accelRampTime = 1.3f;
        private float accelMaxMult = 8.0f;
        private float accelDecayTime = 0.15f;
        private float accelT;

        public bool IsRelevant(Event e)
        {
            // If alt is pressed return to keep zoom mode
            if (e.alt) return false;

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

        public bool Handle(SceneView sv, Event e)
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

        private bool IsStart(Event e) => e.button == 1 && e.type == EventType.MouseDown;
        private bool IsDrag(Event e) => e.button == 1 && e.type == EventType.MouseDrag;
        private bool IsEnd(Event e) => e.button == 1 && (e.type == EventType.MouseUp || e.type == EventType.MouseLeaveWindow || e.type == EventType.Ignore);

        private bool IsMovementKey(Event e)
        {
            return e.keyCode switch
            {
                KeyCode.W or KeyCode.A or KeyCode.S or KeyCode.D or KeyCode.Q or KeyCode.E or KeyCode.LeftShift
                    or KeyCode.RightShift => true,
                _ => false
            };
        }

        private void UpdateKeyState(Event e)
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
                case KeyCode.LeftShift:
                case KeyCode.RightShift:
                    shiftKey = down;
                    break;
            }
        }

        private void Start(SceneView sv)
        {
            if (IsActive) return;

            IsActive = true;

            GUIUtility.hotControl = fpsControlId;

            ToolUtility.StoreCurrentToolState();
            ToolUtility.SetTool(Tool.View, ViewTool.FPS);
            EditorGUIUtility.SetWantsMouseJumping(1);

            rot = sv.rotation;

            lastTime = EditorApplication.timeSinceStartup;

            accelT = 0f;

            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
        }

        private void End()
        {
            if (!IsActive) return;

            IsActive = false;

            GUIUtility.hotControl = 0;

            EditorGUIUtility.SetWantsMouseJumping(0);
            ToolUtility.RestorePreviousToolState();

            wKey = aKey = sKey = dKey = qKey = eKey = shiftKey = false;
            accelT = 0f;

            EditorApplication.update -= Tick;
        }

        private void Tick()
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

            bool moving = dir.sqrMagnitude >= THRESHOLD;

            // Acceleration ramp
            if (moving)
            {
                float ramp = (accelRampTime <= 0f) ? 1f : (dt / accelRampTime);
                accelT = Mathf.Clamp01(accelT + ramp);
            }
            else
            {
                float decay = (accelDecayTime <= 0f) ? 1f : (dt / accelDecayTime);
                accelT = Mathf.Clamp01(accelT - decay);
                if (accelT <= 0f) return;
            }

            if (moving)
            {
                dir.Normalize();
            }
            else
            {
                // Here we simply stop (with accel decay above).
                return;
            }

            float shiftMul = shiftKey ? SHIFT_MULTIPLIER : 1f;
            float accelMul = Mathf.Lerp(1f, accelMaxMult, accelT);
            float speed = BASE_SPEED * shiftMul * accelMul;

            // Keep camera position fixed while adjusting pivot
            Vector3 camPos = sv.camera.transform.position;
            float dist = Vector3.Distance(sv.pivot, camPos);
            if (dist < THRESHOLD) dist = THRESHOLD;

            camPos += dir * speed * dt;

            sv.rotation = rot;
            sv.pivot = camPos + (rot * Vector3.forward) * dist;

            sv.Repaint();
        }

        private void ApplyMouseLook(SceneView sv, Event e)
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

        private Vector3 GetSceneUp()
        {
            return Quaternion.AngleAxis(SRT_SceneViewState.SceneZRotation, Vector3.forward) * Vector3.up;
        }
    }
}