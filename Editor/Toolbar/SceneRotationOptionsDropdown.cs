using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

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

            SceneViewState.onChanged += UpdateAppearance;
            UpdateAppearance();
        }

        private void UpdateAppearance()
        {
            text = $"{SceneViewState.GetState().ToString()}";
            style.display = SceneViewState.EnableTool ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void ShowMenu()
        {
            if (!SceneViewState.EnableTool) return;

            var menu = new GenericMenu();

            AddRotation(menu, "Rotate South", RotationState.South, SceneRotationShortcuts.GetShortcutName(ShortcutTypes.RotateSouth));
            AddRotation(menu, "Rotate East", RotationState.East, SceneRotationShortcuts.GetShortcutName(ShortcutTypes.RotateEast));
            AddRotation(menu, "Rotate North", RotationState.North, SceneRotationShortcuts.GetShortcutName(ShortcutTypes.RotateNorth));
            AddRotation(menu, "Rotate West", RotationState.West, SceneRotationShortcuts.GetShortcutName(ShortcutTypes.RotateWest));

            menu.AddSeparator("");

            menu.AddItem(
                new GUIContent($"Toggle Fake 2D Mode\t{SceneRotationShortcuts.GetShortcutName(ShortcutTypes.Toggle2DMode)}"),
                SceneViewState.Is2DMode,
                SceneRotationApplicationController.Toggle2DMode
            );

            menu.ShowAsContext();
        }

        private void AddRotation(GenericMenu menu, string label, RotationState rot, string shortcutLabel)
        {
            bool active = Mathf.Approximately(SceneViewState.SceneZRotation, SceneViewState.GetRotationValue(rot));

            menu.AddItem(
                new GUIContent($"{label}\t{shortcutLabel}"),
                active,
                () => SceneRotationApplicationController.SetRotation(rot)
            );
        }
    }
}
