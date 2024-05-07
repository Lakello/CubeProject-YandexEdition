using System;
using System.Collections.Generic;
using CubeProject.SO;
using CubeProject.UI;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Source.Scripts.Game.Level;
using UnityEngine;

namespace CubeProject
{
	[RequireComponent(typeof(WindowInitializer))]
	[RequireComponent(typeof(LevelButtonFabric))]
	public class MenuSceneInitializer :
		SerializedMonoBehaviour,
		ISceneLoadHandlerOnStateAndArgument<GameStateMachine, LevelLoader>,
		ITransitInitializer
	{
		[SerializeField] private StartButton _startButton;
		[OdinSerialize] private Dictionary<MenuWindowButton, EventTriggerButton[]> _buttons;

		private LevelButtonFabric _levelButtonFabric;
		private TransitionInitializer<GameStateMachine> _gameTransitionInitializer;
		private TransitionInitializer<WindowStateMachine> _menuWindowTransitionInitializer;

		public event Action<MenuWindowButton> Transiting;

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _levelButtonFabric);

		private void OnDisable()
		{
			_gameTransitionInitializer?.Unsubscribe();
			_menuWindowTransitionInitializer?.Unsubscribe();
		}

		public void OnSceneLoaded<TState>(GameStateMachine machine, LevelLoader levelLoader)
			where TState : State<GameStateMachine>
		{
			_levelButtonFabric.Init(levelLoader);

			gameObject.GetComponentElseThrow(out WindowInitializer _)
				.WindowsInit(machine.Window);

			machine.EnterIn<TState>();

			_gameTransitionInitializer = new TransitionInitializer<GameStateMachine>(machine)
				.InitTransition(
					_startButton,
					levelLoader.LoadCurrentLevel)
				.Subscribe();

			_menuWindowTransitionInitializer = new TransitionInitializer<WindowStateMachine>(machine.Window)
				.InitTransition<SelectLevelWindowState>(_buttons[MenuWindowButton.SelectLevel])
				.InitTransition<LeaderboardWindowState>(_buttons[MenuWindowButton.Leaderboard])
				.InitTransition<MenuWindowState>(_buttons[MenuWindowButton.Menu])
				.Subscribe();
		}
	}
}