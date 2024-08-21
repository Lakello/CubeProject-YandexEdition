using System.Collections.Generic;
using CubeProject.Game.Level.Loader;
using CubeProject.Game.UI.Buttons;
using LeadTools.Extensions;
using LeadTools.FSM;
using LeadTools.FSM.GameFSM;
using LeadTools.FSM.GameFSM.States;
using LeadTools.FSM.Transit;
using LeadTools.FSM.WindowFSM;
using LeadTools.FSM.WindowFSM.States;
using LeadTools.TypedScenes.Core;
using LeadTools.UI.PageSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace CubeProject.EntryPoints.Initializers
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
				.InitTransition<MenuState<MenuWindowState>>(_buttons[MenuWindowButton.Menu])
				.Subscribe();
		}
	}
}