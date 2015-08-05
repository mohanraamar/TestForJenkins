using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using FileInfos = System.Collections.Generic.List<System.IO.FileInfo>;

public class MyEditorScript {
	static string[] SCENES = FindEnabledEditorScenes();
	
	static string APP_NAME = "Android";
	static string TARGET_DIR = "target/Android"; //PlayerSettings.
	
	[MenuItem ("Custom/CI/Build Mac OS X")]
	static void PerformMacOSXBuild ()
	{
		string target_dir = APP_NAME + ".app";
		GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneOSXUniversal,BuildOptions.None) ;
	}

	[MenuItem ("Custom/CI/Build IPhone")]
	static void PerformIPhoneBuild ()
	{
		string target_dir = APP_NAME + "";
		GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget,iPhone,BuildOptions.None) ;
	}

	[MenuItem ("Custom/CI/Build Android")]
	static void PerformAndroidBuild ()
	{
		string target_dir = APP_NAME + "";
		GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.Android,BuildOptions.None);
	}
	
	private static string[] FindEnabledEditorScenes() {
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}
	
	static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
		string res = BuildPipeline.BuildPlayer(scenes,target_dir,build_target,build_options);
		if (res.Length > 0) {
			throw new Exception("BuildPlayer failure: " + res);
		}
	}

	[MenuItem ("Example/Get Unique Path")]
	static void GetUniqueProjectPath () {
		// Prints the unique path
		//Debug.Log (syst);//Application.dataPath);
	}
}
