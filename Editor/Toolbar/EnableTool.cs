using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneRotationToolkit.Editor
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class EnableTool : EditorToolbarToggle
    {
        public const string ID = "SceneRotationToolkit/Enable";

        public EnableTool()
        {
            SceneViewState.onChanged += Sync;

            text = "Enable";
            tooltip = "Enable the tool and the custom orbit/pan logic";
            onIcon = EditorGUIUtility.IconContent("d_GreenCheckmark").image as Texture2D;
            offIcon = EditorGUIUtility.IconContent("d_False").image as Texture2D;

            this.RegisterValueChangedCallback(_ =>
            {
                SceneViewState.ToggleTool();
                Sync();
            });

            Sync();
        }

        private void Sync()
        {
            SetValueWithoutNotify(SceneViewState.EnableTool);
            text = SceneViewState.EnableTool ? "Enabled" : "Disabled";
        }
    }
}