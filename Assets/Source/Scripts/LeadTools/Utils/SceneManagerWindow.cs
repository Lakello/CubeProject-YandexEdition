#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerWindow : EditorWindow
{
	private string _lastScene;
	private Vector2 _scrollPosition;

	[MenuItem("Window/General/Scene Manager")]
	public static void ShowWindow() =>
		GetWindow<SceneManagerWindow>("Scene Manager");

	private void OnEnable() =>
		EditorApplication.playModeStateChanged += OnPlayModeChange;

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

		ShowSceneList();
	}

	private void ShowSceneList()
	{
		_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, false);

		var colors = new []
		{
			Texture2D.grayTexture, 
			Texture2D.blackTexture, 
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

			RenderAdditiveLoadButton(isOpenScene, style, i, path);

			EditorGUILayout.EndHorizontal();

			GUI.backgroundColor = Color.white;
		}
		
		EditorGUILayout.EndScrollView();
	}

	private static void RenderAdditiveLoadButton(bool isOpenScene, GUIStyle style, int i, string path)
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
				EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByBuildIndex(i), true);
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

	private void OnPlayModeChange(PlayModeStateChange state)
	{
		if (state == PlayModeStateChange.ExitingEditMode)
		{
			_lastScene = EditorSceneManager.GetActiveScene().path;
			var zeroScene = SceneUtility.GetScenePathByBuildIndex(0);
			EditorSceneManager.SaveOpenScenes();
			EditorSceneManager.OpenScene(zeroScene);
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

	private static void SetWhiteBackground()
	{
		
	}
	
	private static void SetGrayBackground()
	{
		
	}
}

#endif