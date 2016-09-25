using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor.Callbacks;
using Ionic.Zip;
//using System.Diagnostics;

public class BuildWindow : EditorWindow {
	[MenuItem("Build/Builder")]
	public static void ShowWindow() {
		EditorWindow.GetWindow(typeof(BuildWindow));
	}
	static string path;
	

	class Build {
		public string name;
		public bool isActive;

		public Build(string name, bool isActive) {
			this.name = name;
			this.isActive = isActive;
		}
	}

	static Dictionary<BuildTarget, Build> builds = new Dictionary<BuildTarget, Build>();

	static Vector2 scrollPos, scrollPos2;
	static bool show, backupOldFiles;

	static bool changeFileName = false;
	static string filename = "";

	static bool isSaved;
	static bool load = true;

	int b = 0;

	void Start() {

	}



	static void SaveData() {
		//Debug.Log(Application.dataPath);
		try {
			System.IO.StreamWriter wr = System.IO.File.CreateText(Application.dataPath + "/AutomatedBuild/Editor/save.dat");

			wr.WriteLine(path);
			wr.WriteLine(changeFileName.ToString() + "\t" + filename);

			foreach (BuildTarget tar in builds.Keys) {
				if (builds[tar].name == "") {
					//Debug.Log(tar.ToString());
					continue;
				}

				wr.Write(tar.ToString());
				wr.WriteLine();

				wr.Write("\t" + builds[tar].name.ToString());
				wr.WriteLine();
			}

			wr.Close();
		} catch (Exception e) { Debug.LogException(e); } finally {
			isSaved = true;
		}
	}

	[PostProcessBuild()]
	static void Load(BuildTarget target, string str) {
		load = true;
    }

	static void LoadData() {
		try {
			StreamReader re = File.OpenText(Application.dataPath + "/AutomatedBuild/Editor/save.dat");

			path = re.ReadLine();
			string buffer = re.ReadLine();
			changeFileName = buffer.Split('\t')[0] == "True";
			filename = buffer.Split('\t')[1];

			foreach (BuildTarget bt in Enum.GetValues(typeof(BuildTarget))) {
				var type = typeof(BuildTarget);
				var memInfo = type.GetMember(bt.ToString());

				if (Attribute.IsDefined(memInfo[0], typeof(ObsoleteAttribute)))
					continue;

				if (!builds.ContainsKey(bt))
					builds.Add(bt, new Build("", false));
			}
			//Debug.Log("load");
			string line = "";
			while (line != null) {
				line = re.ReadLine();

				foreach (BuildTarget tar in builds.Keys) {
					//Debug.Log(line + "/" + tar);
					if (line == null)
						break;

					if (line.Equals(tar.ToString())) {
						builds[tar].isActive = true;
						builds[tar].name = re.ReadLine().TrimStart('\t');
						//Debug.Log(builds[tar].name);

					}
				}
			}
			re.Close();
		} catch (Exception e) { Debug.LogException(e); } finally { load = false; }
	}

	string zipSuffix;

	void OnGUI() {
		if (load)
			LoadData();

		GUILayout.Label("Build Versions", EditorStyles.boldLabel);

		EditorGUILayout.Space();
		this.titleContent.tooltip = "Builds all your stuff at once";

		EditorGUILayout.LabelField("Build Path:");

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(32));

		GUILayout.Label(path);

		EditorGUILayout.EndScrollView();


		if (GUILayout.Button("Set Buildpath", GUILayout.Width(160))) {
			path = EditorUtility.SaveFolderPanel("Choose Location of Build Folder", "", "");
			isSaved = false;
		}

		EditorGUILayout.Space();

		bool Ftemp = EditorGUILayout.ToggleLeft("Change Buildfile Name",changeFileName);
		isSaved = Ftemp != changeFileName;
		changeFileName = Ftemp;
		
			GUI.enabled = changeFileName;
			string FNtemp = EditorGUILayout.TextField(filename);
			GUI.enabled = true;
			isSaved = FNtemp != filename;
			filename = FNtemp;

		

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Zipfile name Suffix (Appname *suffix* Buildname)");
		zipSuffix = EditorGUILayout.TextField(zipSuffix);

		EditorGUILayout.Space();


		show = EditorGUILayout.Foldout(show, "Namesets");
		if (show) {
			EditorGUI.indentLevel++;

			scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Width(position.width), GUILayout.Height(600 < position.height - 250 ? 600 : position.height - 250));
			foreach (BuildTarget bt in Enum.GetValues(typeof(BuildTarget))) {
				var type = typeof(BuildTarget);
				var memInfo = type.GetMember(bt.ToString());

				if (Attribute.IsDefined(memInfo[0], typeof(ObsoleteAttribute)))
					continue;

				if (!builds.ContainsKey(bt))
					builds.Add(bt, new Build("", false));



				EditorGUILayout.BeginVertical();
				EditorGUILayout.BeginHorizontal();

				bool temp = EditorGUILayout.ToggleLeft(bt.ToString(), builds[bt].isActive);
				GUI.enabled = temp;
				string tempS = EditorGUILayout.TextField(builds[bt].name);
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
				EditorGUILayout.Space();

                if (builds[bt].isActive != temp || !builds[bt].name.Equals(tempS)) {
					Debug.Log("save");
					isSaved = false;
				}

				builds[bt].isActive = temp;
				builds[bt].name = tempS;
			}

			EditorGUILayout.EndScrollView();
			EditorGUI.indentLevel--;
		}

		if (!isSaved) {
			SaveData();
		}

		backupOldFiles = GUILayout.Toggle(backupOldFiles, "Move Old Files");


		if (GUILayout.Button("Build!")) {
			if (path == "")
				throw (new ArgumentNullException("the path can't be empty!"));


			foreach (BuildTarget build in builds.Keys) {
				if (builds[build].isActive) {
					string realPath = path + "/" + builds[build].name + "/";
					string fileName = (changeFileName ? filename: Application.productName) + (build.ToString().Contains("Windows") ? ".exe" : build.ToString().Contains("Linux") ? ".x86" : "");
					//Debug.Log(build.ToString());

					if (backupOldFiles) {
						try {
							System.IO.Directory.Move(realPath, path + "/" + b + Application.productName + "/");
						} catch (Exception) { }
					} else {
						if (Directory.Exists(realPath))
							Directory.Delete(realPath,true);

					}
					Debug.Log("Building: " + build.ToString());
					BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, realPath + fileName, build, 0);

					Debug.Log("Creating .zip File!");

					using (ZipFile zf = new ZipFile((changeFileName ? filename : Application.productName)+" " +zipSuffix  + " "+ builds[build].name)) {
						zf.AddDirectory(path + "/" + builds[build].name + "/");
						zf.Save(path + "/" + zf.Name + ".zip");
					}
					Debug.Log("Created .zip File!");
				}

			}
			System.Diagnostics.Process.Start(path);
		}
	}
}