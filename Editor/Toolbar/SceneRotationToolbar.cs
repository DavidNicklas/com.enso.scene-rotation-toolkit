using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

namespace SceneRotationToolkit.Editor
{
    [Overlay(typeof(SceneView), "Scene Rotation", true)]
    [Icon("d_SceneAsset Icon")]
    public class SceneRotationToolbar : ToolbarOverlay
    {
        public SceneRotationToolbar()
            : base
            (
                SceneRotationOptionsDropdown.ID,
                Fake2DToggle.ID,
                SceneRotationSettings.ID
            ) {}
    }
}
