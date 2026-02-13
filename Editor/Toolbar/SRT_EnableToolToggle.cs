using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneRotationToolkit.Editor
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class SRT_EnableToolToggle : EditorToolbarToggle
    {
        public const string ID = "SceneRotationToolkit/Enable";

        public SRT_EnableToolToggle()
        {
            text = "Enable";
            tooltip = "Enable the tool and the custom orbit/pan logic";
            onIcon = EditorGUIUtility.IconContent("d_GreenCheckmark").image as Texture2D;
            offIcon = EditorGUIUtility.IconContent("d_False").image as Texture2D;

            this.RegisterValueChangedCallback(_ =>
            {
                SRT_SceneViewState.ToggleTool();
            });

            SRT_SceneViewState.onChanged += Sync;
            RegisterCallback<DetachFromPanelEvent>(_ => SRT_SceneViewState.onChanged -= Sync);
            Sync();
        }

        private void Sync()
        {
            SetValueWithoutNotify(SRT_SceneViewState.EnableTool);
            text = SRT_SceneViewState.EnableTool ? "Enabled" : "Disabled";
        }
    }
}