using CubeProject.UI;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using Source.Scripts.Game.Level;
using Source.Scripts.Game.UI.Buttons;
using Source.Scripts.UI.Buttons;
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
		[SerializeField] private MenuButton _menuButton;

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
				.InitTransition<SelectLevelWindowState, WindowStateMachine>(
					_selectLevelButton,
					machine.Window)
				.InitTransition<LeaderboardWindowState, WindowStateMachine>(
					_leaderboardButton,
					machine.Window)
				.InitTransition<MenuWindowState, WindowStateMachine>(
					_menuButton,
					machine.Window)
				.Subscribe();
		}
	}
}