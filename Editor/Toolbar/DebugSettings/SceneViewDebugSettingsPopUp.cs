using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneRotationToolkit.Editor
{
    public class SceneViewDebugSettingsPopUp : PopupWindowContent
    {
        public override VisualElement CreateGUI()
        {
            var settings = SceneViewDebugSettings.instance;

            var root = new VisualElement();
            root.style.paddingLeft = 8;
            root.style.paddingRight = 8;
            root.style.paddingTop = 8;
            root.style.paddingBottom = 8;

            root.Add(Header("Debug HUD"));

            root.Add(Toggle("Pivot gizmo", settings.showPivotGizmo, v => { settings.showPivotGizmo = v; ApplySettingChange(settings); }));
            root.Add(Toggle("Rotation badge", settings.showRotationBadge, v => { settings.showRotationBadge = v; ApplySettingChange(settings); }));
            root.Add(Toggle("HUD panel", settings.showHud, v => { settings.showHud = v; ApplySettingChange(settings); }));

            root.Add(Spacer(6));
            root.Add(Header("HUD sections"));

            root.Add(Toggle("Tool state", settings.hudToolState, state => { settings.hudToolState = state; ApplySettingChange(settings); }));
            root.Add(Toggle("Mode/zoom", settings.hudMode, state => { settings.hudMode = state; settings.SaveNow(); SceneView.RepaintAll(); }));
            root.Add(Toggle("Pivot line", settings.hudPivot, state => { settings.hudPivot = state; settings.SaveNow(); SceneView.RepaintAll(); }));
            root.Add(Toggle("Scene basis", settings.hudSceneBasis, state => { settings.hudSceneBasis = state; settings.SaveNow(); SceneView.RepaintAll(); }));
            root.Add(Toggle("Camera basis", settings.hudCameraBasis, state => { settings.hudCameraBasis = state; settings.SaveNow(); SceneView.RepaintAll(); }));
            root.Add(Toggle("Input state", settings.hudInputState, state => { settings.hudInputState = state; settings.SaveNow(); SceneView.RepaintAll(); }));

            root.Add(Spacer(6));

            var opacity = new Slider("HUD opacity", 0f, 1f) { value = settings.hudOpacity };
            opacity.RegisterValueChangedCallback(e =>
            {
                settings.hudOpacity = e.newValue; ApplySettingChange(settings);
            });
            root.Add(opacity);

            root.Add(Spacer(8));

            var row = new VisualElement { style = { flexDirection = FlexDirection.Row } };
            var reset = new Button(() =>
                {
                    settings.showPivotGizmo = true;
                    settings.showRotationBadge = true;
                    settings.showHud = true;

                    settings.hudToolState = true;
                    settings.hudMode = true;
                    settings.hudPivot = true;
                    settings.hudSceneBasis = true;
                    settings.hudCameraBasis = true;
                    settings.hudInputState = true;

                    settings.hudOpacity = 0.55f;

                    ApplySettingChange(settings);
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

        private static void ApplySettingChange(SceneViewDebugSettings settings)
        {
            settings.SaveNow();
            SceneView.RepaintAll();
        }

        private static Toggle Toggle(string label, bool value, System.Action<bool> onChanged)
        {
            var toggle = new Toggle(label) { value = value };
            toggle.RegisterValueChangedCallback(e => onChanged(e.newValue));
            return toggle;
        }
    }
}