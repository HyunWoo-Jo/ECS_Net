using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public static class CustomBuild {
    [MenuItem("Custom/Build/Windows")]
    private static void Build() {
        string[] scenes = { "Assets/01.Scenes/PlayScene.unity" };

        BuildPlayerOptions buildOptions = new BuildPlayerOptions();
        buildOptions.scenes = scenes;
        buildOptions.locationPathName = "/Builds/Windows/Game.exe";
        buildOptions.target = BuildTarget.StandaloneWindows;

        BuildPipeline.BuildPlayer(buildOptions);
    }

    //[MenuItem("Custom/Build/Test")]
    //private static void TestBuild() {
    //    string[] scenes = { "Assets/01.Scenes/TestScene.unity" };
    //    BuildPlayerOptions buildOptions = new BuildPlayerOptions();
    //    buildOptions.scenes = scenes;
    //    buildOptions.locationPathName = "/Builds/Test/Test.exe";
    //    buildOptions.target = BuildTarget.StandaloneWindows;
    //    buildOptions.options = BuildOptions.IncludeTestAssemblies;

    //    BuildPipeline.BuildPlayer(buildOptions);
    //}
}