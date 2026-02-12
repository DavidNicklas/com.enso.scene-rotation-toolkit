using UnityEditor;

namespace SceneRotationToolkit.Editor
{
    [InitializeOnLoad]
    public static class SceneRotationApplicationController
    {
        private static bool previousToolEnabled;

        static SceneRotationApplicationController()
        {
            SceneViewState.onChanged += HandleStateChanged;

            previousToolEnabled = !SceneViewState.EnableTool;
            SyncSceneViewController();

            EditorApplication.delayCall += ApplyCurrentStateToLastSceneView;
        }

        public static void ToggleTool() => SceneViewState.ToggleTool();

        public static void Toggle2DMode()
        {
            if (!SceneViewState.EnableTool) return;
            SceneViewState.Toggle2DMode();
        }

        public static void SetRotation(RotationState rotation)
        {
            if (!SceneViewState.EnableTool) return;
            SceneViewState.SetRotation(rotation);
        }

        private static void HandleStateChanged()
        {
            SyncSceneViewController();
            ApplyCurrentStateToLastSceneView();
        }

        private static void SyncSceneViewController()
        {
            if (previousToolEnabled == SceneViewState.EnableTool) return;

            SceneViewController.SetEnabled(SceneViewState.EnableTool);
            previousToolEnabled = SceneViewState.EnableTool;
        }

        private static void ApplyCurrentStateToLastSceneView()
        {
            SceneViewCameraUtility.ApplyState(SceneView.lastActiveSceneView, SceneViewState.Is2DMode, SceneViewState.SceneZRotation);
        }
    }
}
