using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneRotationToolkit.Editor
{
    public class SRT_DebugSettingsPopup : PopupWindowContent
    {
        public override VisualElement CreateGUI()
        {
            var root = new VisualElement
            {
                style =
                {
                    paddingLeft = 8,
                    paddingRight = 8,
                    paddingTop = 8,
                    paddingBottom = 8
                }
            };

            root.Add(Header("Debug HUD"));

            root.Add(Toggle("Pivot gizmo", SRT_DebugSettings.ShowPivotGizmo, v => { SRT_DebugSettings.ShowPivotGizmo = v; SceneView.RepaintAll(); }));
            root.Add(Toggle("Rotation badge", SRT_DebugSettings.ShowRotationBadge, v => { SRT_DebugSettings.ShowRotationBadge = v; SceneView.RepaintAll(); }));
            root.Add(Toggle("HUD panel", SRT_DebugSettings.ShowHud, v => { SRT_DebugSettings.ShowHud = v; SceneView.RepaintAll(); }));

            root.Add(Spacer(6));
            root.Add(Header("HUD sections"));

            root.Add(Toggle("Tool state", SRT_DebugSettings.HUDToolState, state => { SRT_DebugSettings.HUDToolState = state; SceneView.RepaintAll(); }));
            root.Add(Toggle("Mode/zoom", SRT_DebugSettings.HUDMode, state => { SRT_DebugSettings.HUDMode = state; SceneView.RepaintAll(); }));
            root.Add(Toggle("Pivot line", SRT_DebugSettings.HUDPivot, state => { SRT_DebugSettings.HUDPivot = state; SceneView.RepaintAll(); }));
            root.Add(Toggle("Scene basis", SRT_DebugSettings.HUDSceneBasis, state => { SRT_DebugSettings.HUDSceneBasis = state; SceneView.RepaintAll(); }));
            root.Add(Toggle("Camera basis", SRT_DebugSettings.HUDCameraBasis, state => { SRT_DebugSettings.HUDCameraBasis = state; SceneView.RepaintAll(); }));
            root.Add(Toggle("Input state", SRT_DebugSettings.HUDInputState, state => { SRT_DebugSettings.HUDInputState = state; SceneView.RepaintAll(); }));

            root.Add(Spacer(6));

            var opacity = new Slider("HUD opacity", 0f, 1f) { value = SRT_DebugSettings.HUDOpacity };
            opacity.RegisterValueChangedCallback(e =>
            {
                SRT_DebugSettings.HUDOpacity = e.newValue; SceneView.RepaintAll();
            });
            root.Add(opacity);

            root.Add(Spacer(8));

            var row = new VisualElement { style = { flexDirection = FlexDirection.Row } };
            var reset = new Button(() =>
                {
                    SRT_DebugSettings.ShowPivotGizmo = true;
                    SRT_DebugSettings.ShowRotationBadge = true;
                    SRT_DebugSettings.ShowHud = true;

                    SRT_DebugSettings.HUDToolState = true;
                    SRT_DebugSettings.HUDMode = true;
                    SRT_DebugSettings.HUDPivot = true;
                    SRT_DebugSettings.HUDSceneBasis = true;
                    SRT_DebugSettings.HUDCameraBasis = true;
                    SRT_DebugSettings.HUDInputState = true;

                    SRT_DebugSettings.HUDOpacity = 0.55f;

                    SceneView.RepaintAll();
                })
                { text = "Reset" };

            var shortCutButton = new Button(() => EditorApplication.ExecuteMenuItem("Edit/Shortcuts...")) { text = "Manage Shortcuts" };
            reset.style.flexGrow = 1;
            shortCutButton.style.flexGrow = 1;
            shortCutButton.style.marginLeft = 6;

            row.Add(reset);
            row.Add(shortCutButton);
            root.Add(row);

            return root;
        }

        private static Label Header(string text)
        {
            var header = new Label(text) { style = { unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 4 } };
            return header;
        }

        private static VisualElement Spacer(float h)
        {
            var spacer = new VisualElement { style = { height = h } };
            return spacer;
        }

        private static Toggle Toggle(string label, bool value, System.Action<bool> onChanged)
        {
            var toggle = new Toggle(label) { value = value };
            toggle.RegisterValueChangedCallback(e => onChanged(e.newValue));
            return toggle;
        }
    }
}