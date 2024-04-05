using CubeProject.Game;
using CubeProject.InputSystem;
using CubeProject.Save.Data;
using LeadTools.Extensions;
using LeadTools.SaveSystem;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using Source.Scripts.Game.Level;
using UnityEngine;

namespace CubeProject
{
	[RequireComponent(typeof(WindowInitializer))]
	public class GameSceneInitializer : MonoBehaviour, ISceneLoadHandlerOnStateAndArgument<GameStateMachine, LevelLoader>
	{
		[SerializeField] private BackToMenu _backToMenu;

		private EndPoint _endPoint;
		private TransitionInitializer<GameStateMachine> _transitionInitializer;
		private LevelLoader _levelLoader;
		private GameStateMachine _gameStateMachine;

		private EndPoint EndPoint => _endPoint ??= gameObject.FindObjectOfTypeElseThrow(out _endPoint);

		private void OnDisable()
		{
			if (_transitionInitializer != null)
			{
				_transitionInitializer.Unsubscribe();
			}

			if (_gameStateMachine != null)
			{
				_gameStateMachine.UnSubscribeTo<EndLevelState>(OnLevelEnded);
			}
		}

		public void OnSceneLoaded<TState>(GameStateMachine machine, LevelLoader levelLoader)
			where TState : State<GameStateMachine>
		{
			_gameStateMachine = machine;
			_gameStateMachine.SubscribeTo<EndLevelState>(OnLevelEnded);

			_levelLoader = levelLoader;

			gameObject.GetComponentElseThrow(out WindowInitializer windowInitializer);
			windowInitializer.WindowsInit(machine.Window);
			machine.EnterIn<TState>();

			_transitionInitializer = new TransitionInitializer<GameStateMachine>(machine);

			_transitionInitializer.InitTransition(_backToMenu, LoadMenu);
			_transitionInitializer.InitTransition<EndLevelState>(EndPoint);

			_transitionInitializer.Subscribe();

			return;

			void LoadMenu() =>
				MenuScene.Load<MenuState<MenuWindowState>>(machine);
		}

		private void OnLevelEnded(bool isEntered)
		{
			if (isEntered == false)
			{
				return;
			}

			if (GameDataSaver.Instance.Get<CurrentLevel>().Value + 1 >= _levelLoader.LevelsCount)
			{
				GameDataSaver.Instance.Set(new CurrentLevel(0));

				MenuScene.Load<MenuState<SelectLevelWindowState>, LevelLoader>(_gameStateMachine, _levelLoader);
			}
			else
			{
				_levelLoader.LoadNextLevel();
			}
		}
	}
}