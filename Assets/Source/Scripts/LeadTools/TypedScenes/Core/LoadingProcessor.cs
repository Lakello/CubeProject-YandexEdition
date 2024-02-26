using System;
using LeadTools.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeadTools.TypedScenes
{
	public class LoadingProcessor : MonoBehaviour
	{
		private static LoadingProcessor _instance;

		private Action _loadingModelAction;

		public static LoadingProcessor Instance
		{
			get
			{
				if (_instance == null)
					Initialize();

				return _instance;
			}
		}

		private static void Initialize()
		{
			_instance = new GameObject(nameof(LoadingProcessor)).AddComponent<LoadingProcessor>();
			_instance.transform.SetParent(null);
			DontDestroyOnLoad(_instance);
		}

		public void ApplyLoadingModel()
		{
			_loadingModelAction?.Invoke();
			_loadingModelAction = null;
		}

		public void RegisterLoadingModel<TMachine, TState, T>(TMachine machine, T argument)
			where TMachine : StateMachine<TMachine>
			where TState : State<TMachine> =>
			_loadingModelAction = () =>
				CallSceneLoaded<ISceneLoadHandlerOnStateAndArgument<TMachine, T>>(handler => handler.OnSceneLoaded<TState>(machine, argument));

		public void RegisterLoadingModel<TMachine, TState>(TMachine machine)
			where TMachine : StateMachine<TMachine>
			where TState : State<TMachine> =>
			_loadingModelAction = () =>
				CallSceneLoaded<ISceneLoadHandlerOnState<TMachine>>(handler => handler.OnSceneLoaded<TState>(machine));

		public void RegisterLoadingModel<T>(T argument) =>
			_loadingModelAction = () =>
				CallSceneLoaded<ISceneLoadHandler>(handler => handler.OnSceneLoaded(argument));

		private void CallSceneLoaded<THandler>(Action<THandler> onSceneLoaded)
		{
			foreach (var rootObjects in SceneManager.GetActiveScene().GetRootGameObjects())
			{
				foreach (var handler in rootObjects.GetComponentsInChildren<THandler>())
				{
					onSceneLoaded(handler);
				}
			}
		}
	}
}