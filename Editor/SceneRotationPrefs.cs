using UnityEditor;

namespace SceneRotationToolkit.Editor
{
    internal static class SceneRotationPrefs
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
                SceneViewState.SetState(SceneViewState.SOUTH_Z_ROTATION_ANGLE, false, false);
                return;
            }

            float z = EditorPrefs.GetFloat(Keys.Z_ROTATION, SceneViewState.SOUTH_Z_ROTATION_ANGLE);
            bool is2D = EditorPrefs.GetBool(Keys.IS_2D_MODE, false);

            SceneViewState.SetState(z, is2D, true);
        }

        public static void SaveFromModel()
        {
            EditorPrefs.SetBool(Keys.ENABLE, SceneViewState.EnableTool);
            EditorPrefs.SetFloat(Keys.Z_ROTATION, SceneViewState.SceneZRotation);
            EditorPrefs.SetBool(Keys.IS_2D_MODE, SceneViewState.Is2DMode);
        }
    }
}