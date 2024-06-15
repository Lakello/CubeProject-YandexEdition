using CubeProject.Game.Level;
using CubeProject.Game.Player;
using CubeProject.InputSystem;
using CubeProject.Save.Data;
using LeadTools.Extensions;
using LeadTools.SaveSystem;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using UnityEditor;
using UnityEngine;

namespace CubeProject
{
	[RequireComponent(typeof(WindowInitializer))]
	public class GameSceneInitializer :
		MonoBehaviour,
		ISceneLoadHandlerOnStateAndArgument<GameStateMachine, LevelLoader>
	{
		[SerializeField] private bool _isDebug;
		[SerializeField] private EndPoint _endPoint;

		private TransitionInitializer<GameStateMachine> _transitionInitializer;
		private LevelLoader _levelLoader;
		private GameStateMachine _gameStateMachine;

		#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_endPoint == null)
			{
				_endPoint = FindObjectOfType<EndPoint>();
				EditorUtility.SetDirty(this);
			}
		}
		#endif

		private void OnDisable()
		{
			_transitionInitializer?.Unsubscribe();
			_gameStateMachine?.UnSubscribeTo<EndLevelState<EndLevelWindowState>>(OnLevelEnded);
		}

		public void OnSceneLoaded<TState>(GameStateMachine machine, LevelLoader levelLoader)
			where TState : State<GameStateMachine>
		{
			_gameStateMachine = machine;
			_gameStateMachine.SubscribeTo<EndLevelState<EndLevelWindowState>>(OnLevelEnded);

			_levelLoader = levelLoader;

			gameObject.GetComponentElseThrow(out WindowInitializer windowInitializer);
			windowInitializer.WindowsInit(_gameStateMachine.Window);
			_gameStateMachine.EnterIn<TState>();

			_transitionInitializer = new TransitionInitializer<GameStateMachine>(_gameStateMachine)
				.InitTransition<EndLevelState<EndLevelWindowState>>(_endPoint);

			if (_isDebug)
			{
				var backToMenuHandler = new BackToMenuHandler(
					gameObject.GetComponentElseThrow<GameSceneInstaller>().InputService);

				_transitionInitializer.InitTransition(backToMenuHandler, LoadMenu);
			}

			_transitionInitializer.Subscribe();

			return;

			void LoadMenu() =>
				MenuScene.Load<MenuState<MenuWindowState>, LevelLoader>(_gameStateMachine, _levelLoader);
		}

		private void OnLevelEnded(bool isEntered)
		{
			if (isEntered == false)
				return;

			if (GameDataSaver.Instance.Get<CurrentLevel>().Value + 1 >= _levelLoader.LevelsCount)
				_levelLoader.SetMode(LoaderMode.Random);

			_levelLoader.LoadNextLevel();
		}
	}
}