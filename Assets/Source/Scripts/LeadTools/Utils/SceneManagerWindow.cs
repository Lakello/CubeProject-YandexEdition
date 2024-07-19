#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerWindow : EditorWindow
{
	private List<string> _openedScenes = new List<string>();
	
	private Vector2 _scrollPosition;
	private bool _isPlayWithZeroScene;

	[MenuItem("Window/General/Scene Manager")]
	public static void ShowWindow() =>
		GetWindow<SceneManagerWindow>("Scene Manager");

	private void OnEnable() =>
		EditorApplication.playModeStateChanged += OnPlayModeChange;

	private void OnDisable() =>
		EditorApplication.playModeStateChanged -= OnPlayModeChange;

	private void OnPlayModeChange(PlayModeStateChange state)
	{
		if (state == PlayModeStateChange.ExitingEditMode)
		{
			_openedScenes.Clear();
			
			for (int i = 0; i < EditorSceneManager.sceneCount; i++)
			{
				_openedScenes.Add(EditorSceneManager.GetSceneAt(i).path);
			}
			
			var zeroScene = SceneUtility.GetScenePathByBuildIndex(0);
			EditorSceneManager.SaveOpenScenes();

			if (_isPlayWithZeroScene)
				EditorSceneManager.OpenScene(zeroScene);
		}

		if (state == PlayModeStateChange.EnteredEditMode)
		{
			if (_openedScenes.Count < 1)
				return;
			
			EditorSceneManager.OpenScene(_openedScenes[0]);

			if (_openedScenes.Count > 1)
			{
				for (int i = 1; i < _openedScenes.Count; i++)
				{
					EditorSceneManager.OpenScene(_openedScenes[i], OpenSceneMode.Additive);
				}
			}
		}
	}

	private void OnGUI()
	{
		if (EditorApplication.isPlaying)
		{
			EditorGUILayout.HelpBox("In PlayMode", MessageType.Info);

			return;
		}

		if (EditorSceneManager.sceneCountInBuildSettings == 0)
		{
			EditorGUILayout.HelpBox("Not scenes in build settings", MessageType.Warning);
		}

		_isPlayWithZeroScene = EditorGUILayout.Toggle("Play with zero scene", _isPlayWithZeroScene);

		RenderListScenes();
	}

	private void RenderListScenes()
	{
		_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, false);

		var colors = new[]
		{
			Texture2D.grayTexture, Texture2D.blackTexture,
		};

		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			string path = SceneUtility.GetScenePathByBuildIndex(i);
			string sceneName = Path.GetFileNameWithoutExtension(path);

			var layoutStyle = new GUIStyle();
			layoutStyle.normal.background = colors[i % 2];
			EditorGUILayout.BeginHorizontal(layoutStyle);

			bool isOpenScene = EditorSceneManager.GetSceneByName(sceneName).name != null;
			var style = new GUIStyle(GUI.skin.button);

			EditorGUILayout.LabelField(sceneName, GUILayout.MaxWidth(120));

			RenderSingleLoadButton(isOpenScene, sceneName, style, path);

			RenderAdditiveLoadButton(isOpenScene, sceneName, style, path);

			EditorGUILayout.EndHorizontal();

			GUI.backgroundColor = Color.white;
		}

		EditorGUILayout.EndScrollView();
	}

	private static void RenderAdditiveLoadButton(bool isOpenScene, string sceneName, GUIStyle style, string path)
	{
		var buttonText = string.Empty;

		if (isOpenScene)
		{
			if (EditorSceneManager.sceneCount == 1)
			{
				SetNoInteractionColor(style);
				buttonText = "Opened";
			}
			else
			{
				SetCanCloseColor(style);
				buttonText = "Close ADDITIVE";
			}
		}
		else
		{
			SetCanOpenColor(style);
			buttonText = "Open ADDITIVE";
		}

		if (GUILayout.Button(buttonText, style, GUILayout.MaxWidth(120)))
		{
			if (isOpenScene && EditorSceneManager.sceneCount != 1)
			{
				EditorSceneManager.SaveOpenScenes();
				EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByName(sceneName), true);
			}
			else
			{
				EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
			}
		}
	}

	private static void RenderSingleLoadButton(bool isOpenScene, string sceneName, GUIStyle style, string path)
	{
		var buttonText = string.Empty;

		if (isOpenScene && EditorSceneManager.sceneCount == 1)
		{
			SetNoInteractionColor(style);
			buttonText = "Opened";
		}
		else
		{
			SetCanOpenColor(style);
			buttonText = "Open SINGLE";
		}

		if (GUILayout.Button(buttonText, style, GUILayout.MaxWidth(120)))
		{
			if (EditorSceneManager.GetActiveScene().name != sceneName || EditorSceneManager.sceneCount > 1)
			{
				EditorSceneManager.SaveOpenScenes();
				EditorSceneManager.OpenScene(path);
			}
		}
	}

	private static void SetNoInteractionColor(GUIStyle style)
	{
		style.normal.textColor = Color.white;
		GUI.backgroundColor = Color.gray;
	}

	private static void SetCanOpenColor(GUIStyle style)
	{
		style.normal.textColor = Color.white;
		GUI.backgroundColor = Color.green;
	}

	private static void SetCanCloseColor(GUIStyle style)
	{
		style.normal.textColor = Color.white;
		GUI.backgroundColor = Color.yellow;
	}
}

#endif