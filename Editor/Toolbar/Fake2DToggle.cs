using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneRotationToolkit.Editor
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class Fake2DToggle : EditorToolbarToggle
    {
        public const string ID = "SceneRotation/Fake2D";

        public Fake2DToggle()
        {
            tooltip = "Toggle 2D Mode";
            onIcon = EditorGUIUtility.IconContent("d_SceneView2D On").image as Texture2D;
            offIcon = EditorGUIUtility.IconContent("d_SceneView2D").image as Texture2D;

            this.RegisterValueChangedCallback(_ =>
            {
                SceneViewState.Toggle2DMode();
            });

            SceneViewState.onChanged += Sync;
            RegisterCallback<DetachFromPanelEvent>(_ => SceneViewState.onChanged -= Sync);
            Sync();
        }

        private void Sync()
        {
            SetValueWithoutNotify(SceneViewState.Is2DMode);
            style.display = SceneViewState.EnableTool ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
