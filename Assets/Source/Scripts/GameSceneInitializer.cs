using System;
using CubeProject.Game;
using CubeProject.InputSystem;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using Source.Scripts.UI.Buttons;
using UnityEngine;

namespace CubeProject
{
	[RequireComponent(typeof(WindowInitializer))]
	public class GameSceneInitializer : MonoBehaviour, ISceneLoadHandlerOnState<GameStateMachine>
	{
		[SerializeField] private MenuButton _menuButton;
		[SerializeField] private PlayAgainButton _playAgainButton;
		[SerializeField] private BackToMenu _backToMenu;
		[SerializeField] private EndLevel _endLevel;

		private Action _unsubscribe;

		private void OnDisable() =>
			_unsubscribe?.Invoke();

		public void OnSceneLoaded<TState>(GameStateMachine machine)
			where TState : State<GameStateMachine>
		{
			gameObject.GetComponentElseThrow(out WindowInitializer windowInitializer);
			windowInitializer.WindowsInit(machine.Window);
			machine.EnterIn<TState>();

			var transitionInitializer = new TransitionInitializer<GameStateMachine>(machine, out var subscribe, out _unsubscribe);

			transitionInitializer.InitTransition(_playAgainButton, ReloadGame);
			transitionInitializer.InitTransition(_menuButton, LoadMenu);
			transitionInitializer.InitTransition(_backToMenu, LoadMenu);
			transitionInitializer.InitTransition<EndLevelState>(_endLevel);

			subscribe?.Invoke();

			return;

			void LoadMenu() =>
				MenuScene.Load<MenuState>(machine);
			
			void ReloadGame() =>
				GameScene.Load<PlayLevelState>(machine);
		}
	}
}