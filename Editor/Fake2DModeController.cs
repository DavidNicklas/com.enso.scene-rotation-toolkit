using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class Fake2DModeController
    {
        private static Tool prevTool;
        private static ViewTool prevViewTool;
        private static bool switched;

        public static void Handle(SceneView sv, Event e)
        {
            if (!SceneViewState.Fake2DMode) return;
            if (sv.in2DMode) return;

            // Don't interfere with handles (Rect tool resizing, etc.)
            if (GUIUtility.hotControl != 0 || HandleUtility.nearestControl != 0)
                return;

            bool alt = (e.modifiers & EventModifiers.Alt) != 0;

            // Only for MMB drag
            bool mmbDown = e.type == EventType.MouseDown && e.button == 2;
            bool mmbDrag = e.type == EventType.MouseDrag && e.button == 2;
            bool mmbUp   = e.type == EventType.MouseUp   && e.button == 2;

            // Alt + LMB events
            bool altLmbDown = alt && e.type == EventType.MouseDown && e.button == 0;
            bool altLmbDrag = alt && e.type == EventType.MouseDrag && e.button == 0;
            bool altLmbUp   = alt && e.type == EventType.MouseUp   && e.button == 0;

            bool panDown = mmbDown || altLmbDown;
            bool panDrag = mmbDrag || altLmbDrag || (switched && e.type == EventType.MouseDrag && (e.button == 2 || e.button == 0));
            bool panUp = (mmbUp || altLmbUp) && switched;

            if (panDown)
            {
                prevTool = Tools.current;
                prevViewTool = Tools.viewTool;

                SetPanTool();
                switched = true;

                // Don't Use() because Unity needs this event to pan
                return;
            }

            if (panDrag)
            {
                // Keep forcing during drag so Unity doesn't revert
                if (switched)
                {
                    SetPanTool();
                }
                return;
            }

            if (panUp)
            {
                // reset the tool if input is over
                Tools.current = prevTool;
                Tools.viewTool = prevViewTool;
                switched = false;
            }
        }

        private static void SetPanTool()
        {
            Tools.current = Tool.View;
            Tools.viewTool = ViewTool.Pan;
        }
    }
}
