using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneView3DModeController
    {
        private static readonly SceneViewOrbitController OrbitController = new();
        private static readonly SceneViewFPSController SceneViewFPSController = new();

        public static void Handle(SceneView sv, Event e)
        {
            if (sv == null) return;
            if (sv.in2DMode) return;

            // Reserve Alt+Shift+LMB for RectTool / handle modifications
            if (e.alt && e.shift && e.button == 0) return;

            // FPS has priority while active or when RMB starts
            if (SceneViewFPSController.IsRelevant(e) || SceneViewFPSController.IsActive)
            {
                if (SceneViewFPSController.Handle(sv, e))
                    return;
            }

            if (OrbitController.IsRelevant(e))
            {
                OrbitController.Handle(sv, e);
            }
        }
    }
}