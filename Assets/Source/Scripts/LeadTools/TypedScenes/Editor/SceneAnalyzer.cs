#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeadTools.TypedScenes.Editor
{
	public static class SceneAnalyzer
	{
		public static bool TryAddTypedProcessor(AnalyzableScene analyzableScene)
		{
			var scene = analyzableScene.Scene;
			var componentTypes = GetAllTypes(scene);

			if (componentTypes.Contains(typeof(TypedProcessor))) return false;

			var gameObject = new GameObject(nameof(TypedProcessor));
			gameObject.AddComponent<TypedProcessor>();
			scene.GetRootGameObjects().Append(gameObject);
			Undo.RegisterCreatedObjectUndo(gameObject, "Typed processor added");
			EditorSceneManager.SaveScene(scene);

			return true;
		}

		private static IEnumerable<Component> GetAllComponents(Scene activeScene)
		{
			var rootObjects = activeScene.GetRootGameObjects();
			var components = new List<Component>();

			foreach (var gameObject in rootObjects)
			{
				components.AddRange(gameObject.GetComponentsInChildren<Component>());
			}

			return components;
		}

		private static IEnumerable<Type> GetAllTypes(Scene activeScene)
		{
			var components = GetAllComponents(activeScene);
			var types = new HashSet<Type>();

			foreach (var component in components)
			{
				types.Add(component.GetType());
			}

			return types;
		}
	}
}
#endif