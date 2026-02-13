using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    /// <summary>
    /// Routes SceneView input to either Orbit or FPS controller (SRP: routing only).
    /// </summary>
    public static class SceneView3DModeController
    {
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

            if (SceneViewOrbitController.IsRelevant(e))
            {
                SceneViewOrbitController.Handle(sv, e);
            }
        }
    }
}