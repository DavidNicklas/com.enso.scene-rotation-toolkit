using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    [InitializeOnLoad]
    public static class SceneViewController
    {
        static SceneViewController()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

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
    }
}