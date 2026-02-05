using UnityEditor;

namespace SceneRotationToolkit.Editor
{
    [FilePath("ProjectSettings/SceneRotationToolkit/DebugSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public sealed class SceneViewDebugSettings : ScriptableSingleton<SceneViewDebugSettings>
    {
        public bool showPivotGizmo = true;
        public bool showRotationBadge = true;
        public bool showHud = true;

        // HUD sections
        public bool hudToolState = true;
        public bool hudMode = true;
        public bool hudPivot = true;
        public bool hudSceneBasis = true;
        public bool hudCameraBasis = true;
        public bool hudInputState = true;

        public float hudOpacity = 0.55f;

        public void SaveNow() => Save(true);
    }
}