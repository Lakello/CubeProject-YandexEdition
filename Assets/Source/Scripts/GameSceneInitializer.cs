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
		[SerializeField] private EndPoint _endPoint;

		private Action _unsubscribe;
		private LevelLoader _levelLoader;

		private void OnEnable()
		{
			_endPoint.LevelEnded += OnLevelEnded;
		}

		private void OnDisable()
		{
			_endPoint.LevelEnded -= OnLevelEnded;
			_unsubscribe?.Invoke();
		}

		public void OnSceneLoaded<TState>(GameStateMachine machine, LevelLoader levelLoader)
			where TState : State<GameStateMachine>
		{
			_levelLoader = levelLoader;
			
			gameObject.GetComponentElseThrow(out WindowInitializer windowInitializer);
			windowInitializer.WindowsInit(machine.Window);
			machine.EnterIn<TState>();

			var transitionInitializer = new TransitionInitializer<GameStateMachine>(machine, out var subscribe, out _unsubscribe);

			transitionInitializer.InitTransition(_playAgainButton, ReloadGame);
			transitionInitializer.InitTransition(_menuButton, LoadMenu);
			transitionInitializer.InitTransition(_backToMenu, LoadMenu);

			subscribe?.Invoke();

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