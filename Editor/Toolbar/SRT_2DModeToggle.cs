using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneRotationToolkit.Editor
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class SRT_2DModeToggle : EditorToolbarToggle
    {
        public const string ID = "SceneRotation/Fake2D";

        public SRT_2DModeToggle()
        {
            tooltip = "Toggle 2D Mode";
            onIcon = EditorGUIUtility.IconContent("d_SceneView2D On").image as Texture2D;
            offIcon = EditorGUIUtility.IconContent("d_SceneView2D").image as Texture2D;

            this.RegisterValueChangedCallback(_ =>
            {
                SRT_SceneViewState.Toggle2DMode();
            });

            SRT_SceneViewState.onChanged += Sync;
            RegisterCallback<DetachFromPanelEvent>(_ => SRT_SceneViewState.onChanged -= Sync);
            Sync();
        }

        private void Sync()
        {
            SetValueWithoutNotify(SRT_SceneViewState.Is2DMode);
            style.display = SRT_SceneViewState.EnableTool ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
