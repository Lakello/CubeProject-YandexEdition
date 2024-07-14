#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerWindow : EditorWindow
{
	private string _lastScene;

	[MenuItem("Window/General/Scene Manager")]
	public static void ShowWindow()
	{
		GetWindow<SceneManagerWindow>("Scene Manager");
	}

	private void OnEnable()
	{
		EditorApplication.playModeStateChanged += OnPlayModeChange;
	}

	private void OnGUI()
	{
		if (EditorApplication.isPlaying)
		{
			EditorGUILayout.HelpBox("In PlayMode", MessageType.Info);

			return;
		}

		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			string path = SceneUtility.GetScenePathByBuildIndex(i);
			string sceneName = Path.GetFileNameWithoutExtension(path);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(sceneName, GUILayout.MaxWidth(120));
			bool isOpenScene = EditorSceneManager.GetSceneByName(sceneName).name != null;
			var color = new GUIStyle(GUI.skin.button);

			if (EditorSceneManager.GetActiveScene().name == sceneName && EditorSceneManager.sceneCount == 1)
			{
				color.normal.textColor = Color.black;
				GUI.backgroundColor = Color.grey;
			}

			if (GUILayout.Button("Open", color, GUILayout.MaxWidth(120)))
			{
				if (EditorSceneManager.GetActiveScene().name != sceneName || EditorSceneManager.sceneCount > 1)
				{
					EditorSceneManager.SaveOpenScenes();
					EditorSceneManager.OpenScene(path);
				}
			}

			GUI.backgroundColor = Color.white;

			if (isOpenScene && EditorSceneManager.sceneCount == 1)
			{
				color.normal.textColor = Color.black;
				GUI.backgroundColor = Color.grey;
			}

			if (isOpenScene && EditorSceneManager.sceneCount > 1)
			{
				GUI.backgroundColor = Color.gray;
			}

			if (GUILayout.Button(isOpenScene ? "Close Additive" : "Open Additive", color, GUILayout.MaxWidth(120)))
			{
				if (isOpenScene && EditorSceneManager.sceneCount != 1)
				{
					EditorSceneManager.SaveOpenScenes();
					EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByBuildIndex(i), true);
				}
				else
				{
					EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
				}
			}

			EditorGUILayout.EndHorizontal();

			GUILayout.Space(5);

			GUI.backgroundColor = Color.white;
		}

		GUILayout.Space(20);

		if (GUILayout.Button("Play"))
		{
			_lastScene = EditorSceneManager.GetActiveScene().path;
			var zeroScene = SceneUtility.GetScenePathByBuildIndex(0);
			EditorSceneManager.SaveOpenScenes();
			EditorSceneManager.OpenScene(zeroScene);
			EditorApplication.EnterPlaymode();
		}
	}

	private void OnPlayModeChange(PlayModeStateChange state)
	{
		if (_lastScene == null)
		{
			return;
		}

		if (state == PlayModeStateChange.EnteredEditMode)
		{
			EditorSceneManager.OpenScene(_lastScene);
			_lastScene = null;
		}
	}

	private void OnDisable()
	{
		EditorApplication.playModeStateChanged -= OnPlayModeChange;
	}
}

#endif