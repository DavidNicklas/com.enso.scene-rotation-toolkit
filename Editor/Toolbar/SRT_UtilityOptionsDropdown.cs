using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneRotationToolkit.Editor
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class SRT_UtilityOptionsDropdown : EditorToolbarDropdown
    {
        public const string ID = "SceneRotationToolkit/OptionsDropdown";

        public SRT_UtilityOptionsDropdown()
        {
            tooltip = "Scene Rotation Toolkit Options";
            icon = EditorGUIUtility.IconContent("RotateTool").image as Texture2D;

            clicked += ShowMenu;
            RegisterCallback<DetachFromPanelEvent>(_ => clicked -= ShowMenu);

            SRT_SceneViewState.onChanged += UpdateAppearance;
            RegisterCallback<DetachFromPanelEvent>(_ => SRT_SceneViewState.onChanged -= UpdateAppearance);

            UpdateAppearance();
        }

        private void UpdateAppearance()
        {
            text = $"{SRT_SceneViewState.GetRotationState().ToString()}";
            style.display = SRT_SceneViewState.EnableTool ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void ShowMenu()
        {
            if (!SRT_SceneViewState.EnableTool) return;

            var menu = new GenericMenu();

            AddRotation(menu, "Rotate South", RotationState.South, SRT_Shortcuts.GetShortcutName(ShortcutTypes.RotateSouth));
            AddRotation(menu, "Rotate East", RotationState.East, SRT_Shortcuts.GetShortcutName(ShortcutTypes.RotateEast));
            AddRotation(menu, "Rotate North", RotationState.North, SRT_Shortcuts.GetShortcutName(ShortcutTypes.RotateNorth));
            AddRotation(menu, "Rotate West", RotationState.West, SRT_Shortcuts.GetShortcutName(ShortcutTypes.RotateWest));

            menu.AddSeparator("");

            menu.AddItem(
                new GUIContent($"Toggle Fake 2D Mode\t{SRT_Shortcuts.GetShortcutName(ShortcutTypes.Toggle2DMode)}"),
                SRT_SceneViewState.Is2DMode,
                SRT_SceneViewState.Toggle2DMode
            );

            menu.ShowAsContext();
        }

        private void AddRotation(GenericMenu menu, string label, RotationState rot, string shortcutLabel)
        {
            bool active = Mathf.Approximately(SRT_SceneViewState.SceneZRotation, SRT_SceneViewState.GetRotationValue(rot));

            menu.AddItem(
                new GUIContent($"{label}\t{shortcutLabel}"),
                active,
                () => SRT_SceneViewState.SetRotation(rot)
            );
        }
    }
}