using System;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public enum ShortcutTypes { ToggleTool, ToggleFake2D, RotateSouth, RotateNorth, RotateWest, RotateEast }

    public static class SceneRotationShortcuts
    {
        private const string TOGGLE_TOOL_SHORTCUT_ID = "Scene Rotation Toolkit/Toggle Tool";
        private const string ROTATE_SOUTH_SHORTCUT_ID = "Scene Rotation Toolkit/South";
        private const string ROTATE_EAST_SHORTCUT_ID = "Scene Rotation Toolkit/East";
        private const string ROTATE_NORTH_SHORTCUT_ID = "Scene Rotation Toolkit/North";
        private const string ROTATE_WEST_SHORTCUT_ID = "Scene Rotation Toolkit/West";
        private const string TOGGLE_2D_SHORTCUT_ID = "Scene Rotation Toolkit/Toggle Fake 2D";

        [Shortcut(TOGGLE_TOOL_SHORTCUT_ID, typeof(SceneView), KeyCode.R, ShortcutModifiers.Alt)]
        private static void ToggleToolShortcut()
        {
            SceneViewState.ToggleTool();
        }

        [Shortcut(ROTATE_SOUTH_SHORTCUT_ID, typeof(SceneView), KeyCode.Alpha1)]
        public static void RotateSouth()
        {
            if (SceneViewState.EnableTool) SceneViewState.SetRotation(RotationState.South);
        }

        [Shortcut(ROTATE_EAST_SHORTCUT_ID, typeof(SceneView), KeyCode.Alpha2)]
        public static void RotateEast()
        {
            if (SceneViewState.EnableTool) SceneViewState.SetRotation(RotationState.East);
        }

        [Shortcut(ROTATE_NORTH_SHORTCUT_ID, typeof(SceneView), KeyCode.Alpha3)]
        public static void RotateNorth()
        {
            if (SceneViewState.EnableTool) SceneViewState.SetRotation(RotationState.North);
        }

        [Shortcut(ROTATE_WEST_SHORTCUT_ID, typeof(SceneView), KeyCode.Alpha4)]
        public static void RotateWest()
        {
            if (SceneViewState.EnableTool) SceneViewState.SetRotation(RotationState.West);
        }

        [Shortcut(TOGGLE_2D_SHORTCUT_ID, typeof(SceneView), KeyCode.Alpha5)]
        public static void ToggleFake2D()
        {
            if (SceneViewState.EnableTool) SceneViewState.ToggleFake2DMode();
        }

        public static string GetShortcutName(ShortcutTypes type)
        {
            string id = type switch
            {
                ShortcutTypes.ToggleTool => TOGGLE_TOOL_SHORTCUT_ID,
                ShortcutTypes.ToggleFake2D => TOGGLE_2D_SHORTCUT_ID,
                ShortcutTypes.RotateSouth => ROTATE_SOUTH_SHORTCUT_ID,
                ShortcutTypes.RotateNorth => ROTATE_NORTH_SHORTCUT_ID,
                ShortcutTypes.RotateWest => ROTATE_WEST_SHORTCUT_ID,
                ShortcutTypes.RotateEast => ROTATE_EAST_SHORTCUT_ID,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            var shortcutBinding = ShortcutManager.instance.GetShortcutBinding(id);
            return shortcutBinding.ToString();
        }
    }
}