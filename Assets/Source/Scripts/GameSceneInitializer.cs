using CubeProject.Game;
using CubeProject.InputSystem;
using CubeProject.Save.Data;
using DG.DemiLib;
using LeadTools.Extensions;
using LeadTools.SaveSystem;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using Source.Scripts.Game.Level;
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

		private void OnValidate()
		{
			if (_endPoint == null)
			{
				_endPoint = FindObjectOfType<EndPoint>();
				EditorUtility.SetDirty(this);
			}
		}

		private void OnDisable()
		{
			_transitionInitializer?.Unsubscribe();
			_gameStateMachine?.UnSubscribeTo<EndLevelState>(OnLevelEnded);
		}

		public void OnSceneLoaded<TState>(GameStateMachine machine, LevelLoader levelLoader)
			where TState : State<GameStateMachine>
		{
			_gameStateMachine = machine;
			_gameStateMachine.SubscribeTo<EndLevelState>(OnLevelEnded);

			_levelLoader = levelLoader;

			gameObject.GetComponentElseThrow(out WindowInitializer windowInitializer);
			windowInitializer.WindowsInit(_gameStateMachine.Window);
			_gameStateMachine.EnterIn<TState>();
			
			_transitionInitializer = new TransitionInitializer<GameStateMachine>(_gameStateMachine)
				.InitTransition<EndLevelState>(_endPoint);

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