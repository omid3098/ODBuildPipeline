using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildScript
{
    private static Stopwatch stopWatch;

    [MenuItem("Build/Android/Build")]
    private static void AndroidBuild()
    {
        string[] buildScenes = GetScenesInBuildSetting();
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = buildScenes,
            locationPathName = $"Builds/Android/{Application.productName}_{Application.version}_{DateTime.UtcNow.ToLocalTime():dd-MM-yyyy}.apk",
            target = BuildTarget.Android,
            targetGroup = BuildTargetGroup.Android,
            options = BuildOptions.None
        };
        PerformAddressablesCleanBuild();
        ExecuteBuild(buildPlayerOptions);
    }

    private static async void PerformAddressablesCleanBuild()
    {
        stopWatch = Stopwatch.StartNew();
        UnityEngine.Debug.Log($"PerformAddressablesCleanBuild: {DateTime.UtcNow.ToLocalTime()}");
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            UnityEngine.Debug.LogError("AddressableDefaultObject Setting is null");
            return;
        }
        var buildInput = new AddressablesDataBuilderInput(
            settings
        );
        AddressablesPlayerBuildResult addressablesPlayerBuildResult = settings.ActivePlayerDataBuilder.BuildData<AddressablesPlayerBuildResult>(buildInput);
        await Task.Delay(TimeSpan.FromSeconds(addressablesPlayerBuildResult.Duration));
        UnityEngine.Debug.Log($"Addressable build took {stopWatch.Elapsed.Seconds} seconds...");
    }

    [MenuItem("Build/Android/ContentUpdate")]
    private static void PerformAddressablesContentUpdate()
    {
        string contentStateDataPath = ContentUpdateScript.GetContentStateDataPath(false);
        if (!File.Exists(contentStateDataPath))
        {
            throw new Exception("Previous Content State Data missing");
        }

        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        ContentUpdateScript.BuildContentUpdate(settings, contentStateDataPath);
    }

    private static void ExecuteBuild(BuildPlayerOptions buildPlayerOptions)
    {
        UnityEngine.Debug.Log($"ExecuteBuild: {DateTime.UtcNow.ToLocalTime()}");
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            stopWatch.Stop();
            UnityEngine.Debug.Log($"Build succeeded: {summary.totalSize} bytes in {stopWatch.Elapsed.Seconds} seconds.");
        }
        else if (summary.result == BuildResult.Failed)
        {
            UnityEngine.Debug.Log("Build failed");
        }
    }

    private static string[] GetScenesInBuildSetting()
    {
        int sceneCount = 0;
        foreach (var buildSettingScene in EditorBuildSettings.scenes)
            if (buildSettingScene.enabled)
                sceneCount++;
        string[] buildScenes = new string[sceneCount];
        int counter = 0;
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            EditorBuildSettingsScene buildSettingScene = EditorBuildSettings.scenes[i];
            if (buildSettingScene.enabled)
            {
                buildScenes[counter] = buildSettingScene.path;
                counter++;
            }
        }
        return buildScenes;
    }

    // [MenuItem("Build/Windows")]
    // private static void WindowsBuild()
    // {
    //     string[] buildScenes = GetScenesInBuildSetting();
    //     BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
    //     {
    //         scenes = buildScenes,
    //         locationPathName = "Builds/Windows/Game.exe",
    //         target = BuildTarget.StandaloneWindows64,
    //         targetGroup = BuildTargetGroup.Standalone,
    //         options = BuildOptions.None
    //     };
    //     ExecuteBuild(buildPlayerOptions);
    // }
    // [MenuItem("Build/IOS")]
    // private static void IosBuild()
    // {
    //     string[] buildScenes = GetScenesInBuildSetting();
    //     BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
    //     {
    //         scenes = buildScenes,
    //         locationPathName = "Builds/IOS/Game.zip",
    //         target = BuildTarget.iOS,
    //         targetGroup = BuildTargetGroup.iOS,
    //         options = BuildOptions.None
    //     };
    //     ExecuteBuild(buildPlayerOptions);
    // }
    // [MenuItem("Build/OSX")]
    // private static void OSXBuild()
    // {
    //     string[] buildScenes = GetScenesInBuildSetting();
    //     BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
    //     {
    //         scenes = buildScenes,
    //         locationPathName = "Builds/OSX/Game.zip",
    //         target = BuildTarget.StandaloneOSX,
    //         targetGroup = BuildTargetGroup.Standalone,
    //         options = BuildOptions.None
    //     };
    //     ExecuteBuild(buildPlayerOptions);
    // }
    // [MenuItem("Build/Linux")]
    // private static void LinuxBuild()
    // {
    //     string[] buildScenes = GetScenesInBuildSetting();
    //     BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
    //     {
    //         scenes = buildScenes,
    //         locationPathName = "Builds/Linux/Game.x86_64",
    //         target = BuildTarget.StandaloneLinux64,
    //         targetGroup = BuildTargetGroup.Standalone,
    //         options = BuildOptions.None
    //     };
    //     ExecuteBuild(buildPlayerOptions);
    // }

}
