using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneViewController
    {
        private static void OnSceneGUI(SceneView sv)
        {
            SceneViewDebugHUD.Draw(sv);

            if (!SceneViewState.EnableTool) return;

            var e = Event.current;

            if (SceneViewState.Fake2DMode)
            {
                Fake2DModeController.Handle(sv, e);
                return;
            }

            SceneViewOrbitController.Handle(sv, e);
        }

        public static void Toggle(bool enableTool)
        {
            if (enableTool)
            {
                SceneView.duringSceneGui += OnSceneGUI;
            }
            else SceneView.duringSceneGui -= OnSceneGUI;
        }
    }
}