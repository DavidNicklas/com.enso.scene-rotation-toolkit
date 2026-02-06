using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace SceneRotationToolkit.Editor
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class SceneRotationToolkitDebugSettingsMenu : EditorToolbarDropdownToggle
    {
        public const string ID = "SceneRotation/SettingsMenu";

        public SceneRotationToolkitDebugSettingsMenu()
        {
            icon = EditorGUIUtility.IconContent("d_debug").image as Texture2D;
            tooltip = "Toggle Debug Infos";

            this.RegisterValueChangedCallback(OnToggleChanged);
            dropdownClicked += OnDropdownClicked;
        }

        private void OnToggleChanged(ChangeEvent<bool> evt)
        {
            SceneViewDebugHUD.Toggle();
        }

        private void OnDropdownClicked()
        {
            var popUp = new SceneViewDebugSettingsPopUp();
            var mousePosition = Event.current.mousePosition;
            var rect = new Rect(mousePosition.x, mousePosition.y - 20f, 40, 40);
            PopupWindow.Show(rect, popUp);
        }
    }
}
