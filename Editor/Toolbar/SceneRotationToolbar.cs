using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    [Overlay(typeof(SceneView), "Scene Rotation", true)]
    [Icon("d_ContentSizeFitter Icon")]
    public class SceneRotationToolbar : ToolbarOverlay
    {
        public SceneRotationToolbar()
            : base
            (
                EnableToolToggle.ID,
                SceneRotationOptionsDropdown.ID,
                Fake2DToggle.ID,
                SceneRotationToolkitDebugSettingsMenu.ID
            ) {}
    }
}
