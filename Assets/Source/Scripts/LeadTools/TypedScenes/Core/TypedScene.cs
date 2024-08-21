using LeadTools.FSM;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeadTools.TypedScenes.Core
{
	public abstract class TypedScene<TMachine> where TMachine : StateMachine<TMachine>
	{
		public static AsyncOperation LoadScene<T>(string sceneName, LoadSceneMode loadSceneMode, T argument)
		{
			LoadingProcessor.Instance.RegisterLoadingModel(argument);

			return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
		}

		public static AsyncOperation LoadScene<TState>(string sceneName, LoadSceneMode loadSceneMode, TMachine machine)
			where TState : State<TMachine>
		{
			LoadingProcessor.Instance.RegisterLoadingModel<TMachine, TState>(machine);

			return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
		}

		public static AsyncOperation LoadScene<TState, T>(string sceneName, LoadSceneMode loadSceneMode, TMachine machine, T argument)
			where TState : State<TMachine>
		{
			LoadingProcessor.Instance.RegisterLoadingModel<TMachine, TState, T>(machine, argument);

			return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
		}
	}
}