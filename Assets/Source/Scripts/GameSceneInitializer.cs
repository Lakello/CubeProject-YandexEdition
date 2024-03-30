using System;
using CubeProject.Game;
using CubeProject.InputSystem;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using Source.Scripts.Game.Level;
using Source.Scripts.UI.Buttons;
using UnityEngine;

namespace CubeProject
{
	[RequireComponent(typeof(WindowInitializer))]
	public class GameSceneInitializer : MonoBehaviour, ISceneLoadHandlerOnStateAndArgument<GameStateMachine, LevelLoader>
	{
		[SerializeField] private MenuButton _menuButton;
		[SerializeField] private PlayAgainButton _playAgainButton;
		[SerializeField] private BackToMenu _backToMenu;

		private EndPoint _endPoint;
		private TransitionInitializer<GameStateMachine> _transitionInitializer;
		private LevelLoader _levelLoader;

		private void Awake() =>
			_endPoint = FindObjectOfType<EndPoint>();

		private void OnEnable()
		{
			_endPoint.LevelEnded += OnLevelEnded;
		}

		private void OnDisable()
		{
			_endPoint.LevelEnded -= OnLevelEnded;

			if (_transitionInitializer != null)
			{
				_transitionInitializer.Unsubscribe();
			}
		}

		public void OnSceneLoaded<TState>(GameStateMachine machine, LevelLoader levelLoader)
			where TState : State<GameStateMachine>
		{
			_levelLoader = levelLoader;
			
			gameObject.GetComponentElseThrow(out WindowInitializer windowInitializer);
			windowInitializer.WindowsInit(machine.Window);
			machine.EnterIn<TState>();

			_transitionInitializer = new TransitionInitializer<GameStateMachine>(machine);

			_transitionInitializer.InitTransition(_playAgainButton, ReloadGame);
			_transitionInitializer.InitTransition(_menuButton, LoadMenu);
			_transitionInitializer.InitTransition(_backToMenu, LoadMenu);

			_transitionInitializer.Subscribe();

			return;

			void LoadMenu() =>
				MenuScene.Load<MenuState>(machine);
			
			void ReloadGame() =>
				GameScene.Load<PlayLevelState>(machine);
		}
		
		private void OnLevelEnded()
		{
			_levelLoader.LoadNextLevel();
		}
	}
}