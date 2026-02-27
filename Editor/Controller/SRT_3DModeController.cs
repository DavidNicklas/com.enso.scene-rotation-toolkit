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

            if (IsZoomRelevant(e)) return;

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

        private static bool IsZoomRelevant(Event e)
        {
            return e.alt && e.type == EventType.MouseDown && e.button == 1;
        }
    }
}