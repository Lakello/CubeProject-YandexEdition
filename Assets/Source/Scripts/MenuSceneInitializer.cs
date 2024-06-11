using System.Collections.Generic;
using CubeProject.LeadTools.UI.PageSystem;
using CubeProject.SO;
using CubeProject.UI;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using CubeProject.Game.Level;
using UnityEngine;

namespace CubeProject
{
	[RequireComponent(typeof(WindowInitializer))]
	public class MenuSceneInitializer :
		SerializedMonoBehaviour,
		ISceneLoadHandlerOnStateAndArgument<GameStateMachine, LevelLoader>
	{
		[SerializeField] private PageBehaviour _pageBehaviour;
		[SerializeField] private LevelButton _levelButtonPrefab;
		[SerializeField] private StartButton _startButton;
		[OdinSerialize] private Dictionary<MenuWindowButton, EventTriggerButton[]> _buttons;

		private TransitionInitializer<GameStateMachine> _transitionInitializer;

		private void OnDisable() =>
			_transitionInitializer?.Unsubscribe();

		public void OnSceneLoaded<TState>(GameStateMachine machine, LevelLoader levelLoader)
			where TState : State<GameStateMachine>
		{
			_pageBehaviour.Init(LevelButtonFactory.Create(_levelButtonPrefab, levelLoader));

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