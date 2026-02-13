using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    [InitializeOnLoad]
    public static class SceneViewController
    {
        private static bool hooked;
        private static readonly bool SuppressSave;

        static SceneViewController()
        {
            SuppressSave = true;
            SceneRotationPrefs.LoadIntoModel();
            SuppressSave = false;

            SceneViewState.onChanged += OnModelChanged;

            SyncHook();
            EditorApplication.delayCall += ApplyCurrentToLastSceneView;

            AssemblyReloadEvents.beforeAssemblyReload += Cleanup;
            EditorApplication.quitting += Cleanup;
        }

        private static void Cleanup()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            hooked = false;

            SceneViewState.onChanged -= OnModelChanged;

            AssemblyReloadEvents.beforeAssemblyReload -= Cleanup;
            EditorApplication.quitting -= Cleanup;
        }

        private static void OnModelChanged()
        {
            // Prevent multiple saves on load
            if (!SuppressSave)
                SceneRotationPrefs.SaveFromModel();

            SyncHook();
            ApplyCurrentToLastSceneView();
        }

        private static void SyncHook()
        {
            if (SceneViewState.EnableTool && !hooked)
            {
                SceneView.duringSceneGui += OnSceneGUI;
                hooked = true;
            }
            else if (!SceneViewState.EnableTool && hooked)
            {
                SceneView.duringSceneGui -= OnSceneGUI;
                hooked = false;
            }
        }

        private static void ApplyCurrentToLastSceneView()
        {
            var sv = SceneView.lastActiveSceneView;
            if (sv == null) return;

            SceneViewCameraUtility.ApplyState(sv, SceneViewState.Is2DMode, SceneViewState.SceneZRotation, BuildMessage());
        }

        private static void OnSceneGUI(SceneView sv)
        {
            if (!SceneViewState.EnableTool) return;

            var e = Event.current;

            if (SceneViewState.Is2DMode)
            {
                Fake2DModeController.Handle(sv, e);
                return;
            }

            SceneView3DModeController.Handle(sv, e);
        }

        private static string BuildMessage()
        {
            var previousState = SceneViewState.PreviousState;

            if (previousState.enableTool != SceneViewState.EnableTool)
                return $"Tool Enabled: {SceneViewState.EnableTool}";

            if (previousState.is2DMode != SceneViewState.Is2DMode)
                return $"2D Mode: {SceneViewState.Is2DMode}";

            if (!Mathf.Approximately(previousState.sceneZRotation, SceneViewState.SceneZRotation))
                return $"Rotate to: {SceneViewState.GetRotationState()}";

            return null;
        }
    }
}