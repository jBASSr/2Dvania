using System.IO;
using UnityEngine;
using UnityEditor;

public class AddScene{

	[@MenuItem("Example/Load Scene Additive")]
	static void Apply(){
		string strScenePath = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (strScenePath == null || !strScenePath.Contains (".unity")) {
			EditorUtility.DisplayDialog ("Seelct Scene", "You Must Select a Scene!", "Ok");
			EditorApplication.Beep ();
			return;
		}
		Debug.LogError ("Opening " + strScenePath + " additively");
		EditorApplication.OpenSceneAdditive (strScenePath);

	}
}
