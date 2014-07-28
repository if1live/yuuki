using Unity;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScriptBatch : MonoBehaviour
{
    static string gameName = "yuuki";

    static string[] levels = new string[] {
		"Assets/Game/Scenes/Main.unity",
	};

    static string GetBuildPath()
    {
        string basePath = Application.dataPath + "/../..";
        string buildPath = basePath + "/build";
        return buildPath;
    }

    [MenuItem("Yuuki/Build Windows")]
    public static void BuildWinGame()
    {
        string target = string.Format("{0}/win/{1}.exe", GetBuildPath(), gameName);
        BuildPipeline.BuildPlayer(levels, target, BuildTarget.StandaloneWindows, BuildOptions.None);
        // Copy a file from the project folder to the build folder, alongside the built game.
        //FileUtil.CopyFileOrDirectory("Assets/WebPlayerTemplates/Readme.txt", path + "Readme.txt");
        Debug.Log(gameName + " Windows Build Complete");
    }

    /*
    [MenuItem("Yuuki/Build Android")]
    public static void BuildAndroidGame()
    {
        string target = string.Format("{0}/{1}.apk", GetBuildPath(), gameName);
        BuildPipeline.BuildPlayer(levels, target, BuildTarget.Android, BuildOptions.None);
        Debug.Log(gameName + " Android Build Complete");
    }

    [MenuItem("Yuuki/Build Web")]
    public static void BuildWebGame()
    {
        string target = string.Format("{0}/web", GetBuildPath());
        BuildPipeline.BuildPlayer(levels, target, BuildTarget.WebPlayer, BuildOptions.None);
        Debug.Log(gameName + " Web Build Complete");
    }
     */
}
