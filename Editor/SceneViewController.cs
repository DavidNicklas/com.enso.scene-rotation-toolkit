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

        static void OnSceneGUI(SceneView sv)
        {
            var e = Event.current;

            if (SceneViewState.Fake2DMode)
            {
                SceneViewPanController.Handle(sv, e);
                return;
            }

            SceneViewOrbitController.Handle(sv, e);
        }
    }
}