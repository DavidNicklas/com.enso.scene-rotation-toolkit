using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class SceneRotationOptionsDropdown : EditorToolbarDropdown
    {
        public const string ID = "SceneRotationToolkit/OptionsDropdown";

        public SceneRotationOptionsDropdown()
        {
            tooltip = "Scene Rotation Toolkit Options";
            icon = EditorGUIUtility.IconContent("RotateTool").image as Texture2D;

            clicked += ShowMenu;

            SceneViewState.onChanged += UpdateLabel;
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            text = $"{SceneViewState.SceneZRotation:0}°";
        }

        private void ShowMenu()
        {
            var menu = new GenericMenu();

            AddRotation(menu, "Rotate 0°", 0f, "1");
            AddRotation(menu, "Rotate 90°", 90f, "2");
            AddRotation(menu, "Rotate 180°", 180f, "3");
            AddRotation(menu, "Rotate 270°", 270f, "4");

            menu.AddSeparator("");

            menu.AddItem(
                new GUIContent("Toggle Fake 2D Mode"),
                SceneViewState.Fake2DMode,
                () => SceneViewState.ToggleFake2DMode()
            );

            menu.ShowAsContext();
        }

        private void AddRotation(GenericMenu menu, string label, float rot, string shortcutLabel)
        {
            bool active = Mathf.Approximately(SceneViewState.SceneZRotation, rot);

            menu.AddItem(
                new GUIContent($"{label}\t{shortcutLabel}"),
                active,
                () => SceneViewState.SetRotation(rot)
            );
        }
    }
}