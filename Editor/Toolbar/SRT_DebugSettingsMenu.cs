using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace SceneRotationToolkit.Editor
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class SRT_DebugSettingsMenu : EditorToolbarDropdownToggle
    {
        public const string ID = "SceneRotation/SettingsMenu";

        public SRT_DebugSettingsMenu()
        {
            UpdateIcon();
            tooltip = "Toggle Debug Infos";

            this.RegisterValueChangedCallback(OnToggleChanged);
            dropdownClicked += OnDropdownClicked;
            SRT_SceneViewState.onChanged += Sync;
            RegisterCallback<DetachFromPanelEvent>(_ => dropdownClicked -= OnDropdownClicked);
            RegisterCallback<DetachFromPanelEvent>(_ => SRT_SceneViewState.onChanged -= Sync);
            Sync();
        }

        private void Sync()
        {
            SetValueWithoutNotify(SRT_SceneViewState.DebugMode);
            SRT_DebugHUD.Toggle(SRT_SceneViewState.DebugMode);
            UpdateIcon();
        }

        private void OnToggleChanged(ChangeEvent<bool> evt)
        {
            SRT_SceneViewState.ToggleDebugMode();
            SRT_DebugHUD.Toggle(evt.newValue);
            UpdateIcon();
        }

        private void OnDropdownClicked()
        {
            var popUp = new SRT_DebugSettingsPopup();
            var mousePosition = Event.current.mousePosition;
            var rect = new Rect(mousePosition.x, mousePosition.y - 20f, 40, 40);
            PopupWindow.Show(rect, popUp);
        }

        private void UpdateIcon()
        {
            var debugIcon = value ? EditorGUIUtility.IconContent("debug On").image as Texture2D
                : EditorGUIUtility.IconContent("d_debug").image as Texture2D;
            icon = debugIcon;
        }
    }
}
