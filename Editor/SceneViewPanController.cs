using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneViewPanController
    {
        public static void Handle(SceneView sv, Event e)
        {
            if (sv.in2DMode) return;

            if (e.type == EventType.MouseDrag)
            {
                bool wantsPan =
                    e.button == 2 ||
                    (e.alt && e.button == 0) ||
                    e.button == 1;

                if (wantsPan)
                {
                    Pan(sv, e);
                    Tools.viewTool = ViewTool.Pan;
                    e.Use();
                }
            }
        }

        private static void Pan(SceneView sv, Event e)
        {
            Camera cam = sv.camera;

            float panSpeed = sv.size * 0.002f;

            Vector3 right = cam.transform.right;
            Vector3 up = -cam.transform.up;

            sv.pivot += (-right * e.delta.x - up * e.delta.y) * panSpeed;
            sv.Repaint();
        }
    }
}