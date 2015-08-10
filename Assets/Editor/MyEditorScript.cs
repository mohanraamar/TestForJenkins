using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using FileInfos = System.Collections.Generic.List<System.IO.FileInfo>;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEditor.Callbacks;
using System.Linq;

public class MyEditorScript {
	static string[] SCENES = FindEnabledEditorScenes();
	
	static string APP_NAME = "Android";
	static string TARGET_DIR = "target"; //PlayerSettings.
	
	[MenuItem ("Custom/CI/Build Mac OS X")]
	static void PerformMacOSXBuild ()
	{
		string target_dir = APP_NAME + ".app";
		GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneOSXUniversal,BuildOptions.None) ;
	}

	[MenuItem ("Custom/CI/Build IPhone")]
	static void PerformIPhoneBuild ()
	{
		string Path = TARGET_DIR + "/IPhone/";
		if (!Directory.Exists (TARGET_DIR + "/IPhone")) {
			System.IO.Directory.CreateDirectory (TARGET_DIR + "/IPhone");
		}

		string target_dir = "BuildIPhone" + "";
		GenericBuild(SCENES, Path + target_dir, BuildTarget.iPhone ,BuildOptions.None) ;
	}

	[MenuItem ("Custom/CI/Build Android")]
	static void PerformAndroidBuild ()
	{
		string Path = TARGET_DIR + "/Android/";
		if (!Directory.Exists (TARGET_DIR + "/Android")) {
			System.IO.Directory.CreateDirectory (TARGET_DIR + "/Android");
		}

		string target_dir = "BuildAndroid" + "";
		GenericBuild(SCENES, Path + target_dir, BuildTarget.Android,BuildOptions.None);
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

	[PostProcessBuild(200)]
	public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
	{
		#if UNITY_IOS
		string temp = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
		PBXProject proj = new PBXProject();
		proj.ReadFromString(File.ReadAllText(temp));
		
		string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
		proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
		proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-lc++");
		proj.AddBuildProperty(target, "CODE_SIGN_RESOURCE_RULES_PATH", "$(SDKROOT)/ResourceRules.plist");
		proj.AddBuildProperty(target, "CLANG_ENABLE_MODULES","YES");

		File.WriteAllText(temp, proj.WriteToString());
		#endif
	}
















}
