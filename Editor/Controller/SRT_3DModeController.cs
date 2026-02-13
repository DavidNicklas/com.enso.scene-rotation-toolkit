using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SRT_3DModeController
    {
        private static readonly SRT_OrbitController OrbitController = new();
        private static readonly SRT_FPSController FPSController = new();

        public static void Handle(SceneView sv, Event e)
        {
            if (sv == null) return;
            if (sv.in2DMode) return;

            // Reserve Alt+Shift+LMB for RectTool / handle modifications
            if (e.alt && e.shift && e.button == 0) return;

            // FPS has priority while active or when RMB starts
            if (FPSController.IsRelevant(e) || FPSController.IsActive)
            {
                if (FPSController.Handle(sv, e))
                    return;
            }

            if (OrbitController.IsRelevant(e))
            {
                OrbitController.Handle(sv, e);
            }
        }
    }
}