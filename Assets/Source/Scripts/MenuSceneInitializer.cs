using System;
using CubeProject.UI;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using Source.Scripts.Game.Level;
using Source.Scripts.Game.UI.Buttons;
using UnityEngine;

namespace CubeProject
{
	[RequireComponent(typeof(WindowInitializer))]
	[RequireComponent(typeof(LevelButtonFabric))]
	public class MenuSceneInitializer : MonoBehaviour, ISceneLoadHandlerOnStateAndArgument<GameStateMachine, LevelLoader>
	{
		[SerializeField] private StartButton _startButton;
		[SerializeField] private SelectLevelButton _selectLevelButton;
		[SerializeField] private LeaderboardButton _leaderboardButton;

		private LevelButtonFabric _levelButtonFabric;
		private TransitionInitializer<GameStateMachine> _transitionInitializer;

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _levelButtonFabric);

		private void OnDisable()
		{
			if (_transitionInitializer != null)
			{
				_transitionInitializer.Unsubscribe();
			}
		}

		public void OnSceneLoaded<TState>(GameStateMachine machine, LevelLoader levelLoader)
			where TState : State<GameStateMachine>
		{
			_levelButtonFabric.Init(levelLoader);

			gameObject.GetComponentElseThrow(out WindowInitializer windowInitializer);
			windowInitializer.WindowsInit(machine.Window);

			Debug.Log($"STATE MACHINE = {machine.Window != null}");
			machine.EnterIn<TState>();

			_transitionInitializer = new TransitionInitializer<GameStateMachine>(machine);

			_transitionInitializer.InitTransition(_startButton, levelLoader.LoadCurrentLevel);
			_transitionInitializer.InitTransition<SelectLevelWindowState, WindowStateMachine>(_selectLevelButton, machine.Window);
			_transitionInitializer.InitTransition<LeaderboardWindowState, WindowStateMachine>(_leaderboardButton, machine.Window);

			_transitionInitializer.Subscribe();
		}
	}
}