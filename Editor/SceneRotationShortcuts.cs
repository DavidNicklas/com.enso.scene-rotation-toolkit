using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class SceneRotationShortcuts
    {
        [Shortcut("Scene Rotation Toolkit/Toggle UI", typeof(SceneView), KeyCode.R, ShortcutModifiers.Alt)]
        private static void ToggleOverlayShortcut()
        {
            // Toggle the overlay
            var sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null) return;

            sceneView.TryGetOverlay("Scene Rotation", out var overlay);
            overlay.displayed = !overlay.displayed;
        }

        [Shortcut("Scene Rotation Toolkit/0°", typeof(SceneView), KeyCode.Alpha1)]
        public static void Rotate0()
        {
            SceneViewState.SetRotation(0f);
        }

        [Shortcut("Scene Rotation Toolkit/90°", typeof(SceneView), KeyCode.Alpha2)]
        public static void Rotate90()
        {
            SceneViewState.SetRotation(90f);
        }

        [Shortcut("Scene Rotation Toolkit/180°", typeof(SceneView), KeyCode.Alpha3)]
        public static void Rotate180()
        {
            SceneViewState.SetRotation(180f);
        }

        [Shortcut("Scene Rotation Toolkit/270°", typeof(SceneView), KeyCode.Alpha4)]
        public static void Rotate270()
        {
            SceneViewState.SetRotation(270f);
        }

        [Shortcut("Scene Rotation Toolkit/Toggle Fake 2D", typeof(SceneView), KeyCode.Alpha5)]
        public static void ToggleFake2D()
        {
            SceneViewState.ToggleFake2DMode();
        }
    }
}