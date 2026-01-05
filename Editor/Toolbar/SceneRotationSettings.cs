using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class SceneRotationSettings : EditorToolbarButton
    {
        public const string ID = "SceneRotation/Settings";

        public SceneRotationSettings()
        {
            tooltip = "Settings";
            icon = EditorGUIUtility.IconContent("d_SettingsIcon").image as Texture2D;

            // COMING SOON
            clicked += OpenSettingsOverlay;
        }

        private void OpenSettingsOverlay()
        {
            // var sceneView = SceneView.lastActiveSceneView;
            // if (sceneView == null) return;
            //
            // sceneView.TryGetOverlay("Scene Rotation Overlay", out var overlay);
            // overlay.displayed = !overlay.displayed;

            EditorApplication.ExecuteMenuItem("Edit/Shortcuts...");
        }
    }
}