using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneRotationToolkit.Editor
{
    public class SceneViewDebugSettingsPopUp : PopupWindowContent
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

            root.Add(Toggle("Pivot gizmo", SceneViewDebugSettings.ShowPivotGizmo, v => { SceneViewDebugSettings.ShowPivotGizmo = v; SceneView.RepaintAll(); }));
            root.Add(Toggle("Rotation badge", SceneViewDebugSettings.ShowRotationBadge, v => { SceneViewDebugSettings.ShowRotationBadge = v; SceneView.RepaintAll(); }));
            root.Add(Toggle("HUD panel", SceneViewDebugSettings.ShowHud, v => { SceneViewDebugSettings.ShowHud = v; SceneView.RepaintAll(); }));

            root.Add(Spacer(6));
            root.Add(Header("HUD sections"));

            root.Add(Toggle("Tool state", SceneViewDebugSettings.HUDToolState, state => { SceneViewDebugSettings.HUDToolState = state; SceneView.RepaintAll(); }));
            root.Add(Toggle("Mode/zoom", SceneViewDebugSettings.HUDMode, state => { SceneViewDebugSettings.HUDMode = state; SceneView.RepaintAll(); }));
            root.Add(Toggle("Pivot line", SceneViewDebugSettings.HUDPivot, state => { SceneViewDebugSettings.HUDPivot = state; SceneView.RepaintAll(); }));
            root.Add(Toggle("Scene basis", SceneViewDebugSettings.HUDSceneBasis, state => { SceneViewDebugSettings.HUDSceneBasis = state; SceneView.RepaintAll(); }));
            root.Add(Toggle("Camera basis", SceneViewDebugSettings.HUDCameraBasis, state => { SceneViewDebugSettings.HUDCameraBasis = state; SceneView.RepaintAll(); }));
            root.Add(Toggle("Input state", SceneViewDebugSettings.HUDInputState, state => { SceneViewDebugSettings.HUDInputState = state; SceneView.RepaintAll(); }));

            root.Add(Spacer(6));

            var opacity = new Slider("HUD opacity", 0f, 1f) { value = SceneViewDebugSettings.HUDOpacity };
            opacity.RegisterValueChangedCallback(e =>
            {
                SceneViewDebugSettings.HUDOpacity = e.newValue; SceneView.RepaintAll();
            });
            root.Add(opacity);

            root.Add(Spacer(8));

            var row = new VisualElement { style = { flexDirection = FlexDirection.Row } };
            var reset = new Button(() =>
                {
                    SceneViewDebugSettings.ShowPivotGizmo = true;
                    SceneViewDebugSettings.ShowRotationBadge = true;
                    SceneViewDebugSettings.ShowHud = true;

                    SceneViewDebugSettings.HUDToolState = true;
                    SceneViewDebugSettings.HUDMode = true;
                    SceneViewDebugSettings.HUDPivot = true;
                    SceneViewDebugSettings.HUDSceneBasis = true;
                    SceneViewDebugSettings.HUDCameraBasis = true;
                    SceneViewDebugSettings.HUDInputState = true;

                    SceneViewDebugSettings.HUDOpacity = 0.55f;

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