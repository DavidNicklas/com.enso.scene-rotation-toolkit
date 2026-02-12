using UnityEditor;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    public static class Fake2DModeController
    {
        private static bool switched;

        public static void Handle(SceneView sv, Event e)
        {
            if (sv.in2DMode) return;

            // Don't interfere with handles (Rect tool resizing, etc.)
            if (GUIUtility.hotControl != 0 || HandleUtility.nearestControl != 0) return;

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
                ToolUtility.StoreCurrentToolState();
                ToolUtility.SetTool(Tool.View, ViewTool.Pan);

                switched = true;

                // Don't Use() because Unity needs this event to pan
                return;
            }

            if (panDrag)
            {
                // Keep forcing during drag so Unity doesn't revert
                if (switched) ToolUtility.SetTool(Tool.View, ViewTool.Pan);

                return;
            }

            if (panUp)
            {
                // reset the tool if input is over
                ToolUtility.RestorePreviousToolState();
                switched = false;
            }
        }
    }
}