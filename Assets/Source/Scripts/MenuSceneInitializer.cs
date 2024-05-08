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
		ISceneLoadHandlerOnStateAndArgument<GameStateMachine, LevelLoader>
	{
		[SerializeField] private StartButton _startButton;
		[OdinSerialize] private Dictionary<MenuWindowButton, EventTriggerButton[]> _buttons;

		private LevelButtonFabric _levelButtonFabric;
		private TransitionInitializer<GameStateMachine> _transitionInitializer;

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _levelButtonFabric);

		private void OnDisable() =>
			_transitionInitializer?.Unsubscribe();

		public void OnSceneLoaded<TState>(GameStateMachine machine, LevelLoader levelLoader)
			where TState : State<GameStateMachine>
		{
			_levelButtonFabric.Init(levelLoader);

			gameObject.GetComponentElseThrow(out WindowInitializer _)
				.WindowsInit(machine.Window);

			machine.EnterIn<TState>();

			_transitionInitializer = new TransitionInitializer<GameStateMachine>(machine)
				.InitTransition(_startButton, levelLoader.LoadCurrentLevel)
				.InitTransition<MenuState<SelectLevelWindowState>>(_buttons[MenuWindowButton.SelectLevel])
				.InitTransition<MenuState<LeaderboardWindowState>>(_buttons[MenuWindowButton.Leaderboard])
				.InitTransition<MenuState<MenuWindowState>>(_buttons[MenuWindowButton.Menu])
				.Subscribe();
		}
	}
}