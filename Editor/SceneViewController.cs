using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneViewController
    {
        private static bool isEnabled;

        private static void OnSceneGUI(SceneView sv)
        {
            if (!SceneViewState.EnableTool) return;

            var e = Event.current;

            if (SceneViewState.Is2DMode)
            {
                Fake2DModeController.Handle(sv, e);
                return;
            }

            SceneViewOrbitController.Handle(sv, e);
        }

        public static void SetEnabled(bool enableTool)
        {
            if (isEnabled == enableTool) return;

            if (enableTool) SceneView.duringSceneGui += OnSceneGUI;
            else SceneView.duringSceneGui -= OnSceneGUI;

            isEnabled = enableTool;
        }
    }
}
