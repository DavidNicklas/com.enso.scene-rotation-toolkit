using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneRotationToolkit.Editor
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class SceneRotationSettings : EditorToolbarButton
    {
        public const string ID = "SceneRotation/Settings";

        public SceneRotationSettings()
        {
            SceneViewState.onChanged += UpdateVisibility;

            tooltip = "Settings";
            icon = EditorGUIUtility.IconContent("d_SettingsIcon").image as Texture2D;

            // COMING SOON
            clicked += OpenSettingsOverlay;

            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            style.display = SceneViewState.EnableTool ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OpenSettingsOverlay()
        {
            if (!SceneViewState.EnableTool) return;

            // var sceneView = SceneView.lastActiveSceneView;
            // if (sceneView == null) return;
            //
            // sceneView.TryGetOverlay("Scene Rotation Overlay", out var overlay);
            // overlay.displayed = !overlay.displayed;

            EditorApplication.ExecuteMenuItem("Edit/Shortcuts...");
        }
    }
}
