using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;

namespace ContactGloveSDK
{
    [InitializeOnLoad]
    public class SteamVRDefineSymbolSetter
    {
        static SteamVRDefineSymbolSetter()
        {
            var isSteamVRAvailable = CompilationPipeline.GetAssemblies().Any(a => a.name == "SteamVR");
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);

            if (isSteamVRAvailable && !defines.Contains("STEAMVR_PRESENT"))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines + ";STEAMVR_PRESENT");
            }
            else if (!isSteamVRAvailable && defines.Contains("STEAMVR_PRESENT"))
            {
                var updatedDefines = defines.Replace("STEAMVR_PRESENT", "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, updatedDefines);
            }
        }
    }
}