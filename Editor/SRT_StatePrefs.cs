using UnityEditor;

namespace SceneRotationToolkit.Editor
{
    internal static class SRT_StatePrefs
    {
        private static class Keys
        {
            public const string Z_ROTATION = "SceneRotationToolkit.State.ZRotation";
            public const string IS_2D_MODE = "SceneRotationToolkit.State.Is2DMode";
            public const string ENABLE     = "SceneRotationToolkit.State.Enable";
        }

        public static void LoadIntoModel()
        {
            bool enable = EditorPrefs.GetBool(Keys.ENABLE, false);

            if (!enable)
            {
                SRT_SceneViewState.SetState(SRT_SceneViewState.SOUTH_Z_ROTATION_ANGLE, false, false);
                return;
            }

            float z = EditorPrefs.GetFloat(Keys.Z_ROTATION, SRT_SceneViewState.SOUTH_Z_ROTATION_ANGLE);
            bool is2D = EditorPrefs.GetBool(Keys.IS_2D_MODE, false);

            SRT_SceneViewState.SetState(z, is2D, true);
        }

        public static void SaveFromModel()
        {
            EditorPrefs.SetBool(Keys.ENABLE, SRT_SceneViewState.EnableTool);
            EditorPrefs.SetFloat(Keys.Z_ROTATION, SRT_SceneViewState.SceneZRotation);
            EditorPrefs.SetBool(Keys.IS_2D_MODE, SRT_SceneViewState.Is2DMode);
        }
    }
}