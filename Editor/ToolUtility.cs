using UnityEditor;

namespace SceneRotationToolkit.Editor
{
    public static class ToolUtility
    {
        private static Tool prevTool;
        private static ViewTool prevViewTool;

        public static void StoreCurrentToolState()
        {
            prevTool = Tools.current;
            prevViewTool = Tools.viewTool;
        }

        public static void SetTool(Tool tool, ViewTool viewTool = ViewTool.None)
        {
            Tools.current = tool;
            Tools.viewTool = viewTool;
        }

        public static void RestorePreviousToolState()
        {
            Tools.current = prevTool;
            Tools.viewTool = prevViewTool;
        }
    }
}