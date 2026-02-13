using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    [Overlay(typeof(SceneView), "Scene Rotation", true)]
    [Icon("d_ContentSizeFitter Icon")]
    public class SRT_Toolbar : ToolbarOverlay
    {
        public SRT_Toolbar()
            : base
            (
                SRT_EnableToolToggle.ID,
                SRT_UtilityOptionsDropdown.ID,
                SRT_2DModeToggle.ID,
                SRT_DebugSettingsMenu.ID
            ) {}
    }
}
