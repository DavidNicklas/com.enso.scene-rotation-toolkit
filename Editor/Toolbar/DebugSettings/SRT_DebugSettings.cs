using UnityEditor;

namespace SceneRotationToolkit.Editor
{
    public static class SRT_DebugSettings
    {
        private static class Keys
        {
            public const string SHOW_PIVOT_GIZMO     = "SceneRotationToolkit.Debug.showPivotGizmo";
            public const string SHOW_ROTATION_BADGE  = "SceneRotationToolkit.Debug.showRotationBadge";
            public const string SHOW_HUD             = "SceneRotationToolkit.Debug.showHud";

            public const string HUD_TOOL_STATE       = "SceneRotationToolkit.Debug.hudToolState";
            public const string HUD_MODE             = "SceneRotationToolkit.Debug.hudMode";
            public const string HUD_PIVOT            = "SceneRotationToolkit.Debug.hudPivot";
            public const string HUD_SCENE_BASIS      = "SceneRotationToolkit.Debug.hudSceneBasis";
            public const string HUD_CAMERA_BASIS     = "SceneRotationToolkit.Debug.hudCameraBasis";
            public const string HUD_INPUT_STATE      = "SceneRotationToolkit.Debug.hudInputState";

            public const string HUD_OPACITY          = "SceneRotationToolkit.Debug.hudOpacity";
        }

        public static bool ShowPivotGizmo
        {
            get => EditorPrefs.GetBool(Keys.SHOW_PIVOT_GIZMO, true);
            set => EditorPrefs.SetBool(Keys.SHOW_PIVOT_GIZMO, value);
        }

        public static bool ShowRotationBadge
        {
            get => EditorPrefs.GetBool(Keys.SHOW_ROTATION_BADGE, true);
            set => EditorPrefs.SetBool(Keys.SHOW_ROTATION_BADGE, value);
        }

        public static bool ShowHud
        {
            get => EditorPrefs.GetBool(Keys.SHOW_HUD, true);
            set => EditorPrefs.SetBool(Keys.SHOW_HUD, value);
        }

        public static bool HUDToolState
        {
            get => EditorPrefs.GetBool(Keys.HUD_TOOL_STATE, true);
            set => EditorPrefs.SetBool(Keys.HUD_TOOL_STATE, value);
        }

        public static bool HUDMode
        {
            get => EditorPrefs.GetBool(Keys.HUD_MODE, true);
            set => EditorPrefs.SetBool(Keys.HUD_MODE, value);
        }

        public static bool HUDPivot
        {
            get => EditorPrefs.GetBool(Keys.HUD_PIVOT, true);
            set => EditorPrefs.SetBool(Keys.HUD_PIVOT, value);
        }

        public static bool HUDSceneBasis
        {
            get => EditorPrefs.GetBool(Keys.HUD_SCENE_BASIS, true);
            set => EditorPrefs.SetBool(Keys.HUD_SCENE_BASIS, value);
        }

        public static bool HUDCameraBasis
        {
            get => EditorPrefs.GetBool(Keys.HUD_CAMERA_BASIS, true);
            set => EditorPrefs.SetBool(Keys.HUD_CAMERA_BASIS, value);
        }

        public static bool HUDInputState
        {
            get => EditorPrefs.GetBool(Keys.HUD_INPUT_STATE, true);
            set => EditorPrefs.SetBool(Keys.HUD_INPUT_STATE, value);
        }

        public static float HUDOpacity
        {
            get => EditorPrefs.GetFloat(Keys.HUD_OPACITY, 0.55f);
            set => EditorPrefs.SetFloat(Keys.HUD_OPACITY, value);
        }
    }
}