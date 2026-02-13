using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    [InitializeOnLoad]
    public static class SRT_Router
    {
        private static bool hooked;
        private static readonly bool SuppressSave;

        static SRT_Router()
        {
            SuppressSave = true;
            SRT_StatePrefs.LoadIntoModel();
            SuppressSave = false;

            SRT_SceneViewState.onChanged += OnModelChanged;

            SyncHook();
            EditorApplication.delayCall += ApplyCurrentToLastSceneView;

            AssemblyReloadEvents.beforeAssemblyReload += Cleanup;
            EditorApplication.quitting += Cleanup;
        }

        private static void Cleanup()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            hooked = false;

            SRT_SceneViewState.onChanged -= OnModelChanged;

            AssemblyReloadEvents.beforeAssemblyReload -= Cleanup;
            EditorApplication.quitting -= Cleanup;
        }

        private static void OnModelChanged()
        {
            // Prevent multiple saves on load
            if (!SuppressSave)
                SRT_StatePrefs.SaveFromModel();

            SyncHook();
            ApplyCurrentToLastSceneView();
        }

        private static void SyncHook()
        {
            if (SRT_SceneViewState.EnableTool && !hooked)
            {
                SceneView.duringSceneGui += OnSceneGUI;
                hooked = true;
            }
            else if (!SRT_SceneViewState.EnableTool && hooked)
            {
                SceneView.duringSceneGui -= OnSceneGUI;
                hooked = false;
            }
        }

        private static void ApplyCurrentToLastSceneView()
        {
            var sv = SceneView.lastActiveSceneView;
            if (sv == null) return;

            SceneViewCameraUtility.ApplyState(sv, SRT_SceneViewState.Is2DMode, SRT_SceneViewState.SceneZRotation, BuildMessage());
        }

        private static void OnSceneGUI(SceneView sv)
        {
            if (!SRT_SceneViewState.EnableTool) return;

            var e = Event.current;

            if (SRT_SceneViewState.Is2DMode)
            {
                SRT_2DModeController.Handle(sv, e);
                return;
            }

            SRT_3DModeController.Handle(sv, e);
        }

        private static string BuildMessage()
        {
            var previousState = SRT_SceneViewState.PreviousState;

            if (previousState.enableTool != SRT_SceneViewState.EnableTool)
                return $"Tool Enabled: {SRT_SceneViewState.EnableTool}";

            if (previousState.is2DMode != SRT_SceneViewState.Is2DMode)
                return $"2D Mode: {SRT_SceneViewState.Is2DMode}";

            if (!Mathf.Approximately(previousState.sceneZRotation, SRT_SceneViewState.SceneZRotation))
                return $"Rotate to: {SRT_SceneViewState.GetRotationState()}";

            return null;
        }
    }
}