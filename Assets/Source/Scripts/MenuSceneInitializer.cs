	using System;
using CubeProject.UI;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using LeadTools.TypedScenes;
using Source.Scripts.Game.Level;
using UnityEngine;

namespace CubeProject
{
	[RequireComponent(typeof(WindowInitializer))]
	public class MenuSceneInitializer : MonoBehaviour, ISceneLoadHandlerOnStateAndArgument<GameStateMachine, LevelLoader>
	{
		[SerializeField] private StartButton _startButton;
		[SerializeField] private LevelButtonFabric _levelButtonFabric;

		private TransitionInitializer<GameStateMachine> _transitionInitializer;

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

			machine.EnterIn<TState>();

			_transitionInitializer = new TransitionInitializer<GameStateMachine>(machine);

			_transitionInitializer.InitTransition(_startButton, levelLoader.LoadCurrentLevel);

			_transitionInitializer.Subscribe();
		}
	}
}