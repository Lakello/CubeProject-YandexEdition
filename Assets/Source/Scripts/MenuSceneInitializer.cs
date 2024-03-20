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
		[SerializeField] private LevelButtonSpawner _levelButtonSpawner;

		private Action _unsubscribe;

		private void OnDisable() =>
			_unsubscribe?.Invoke();

		public void OnSceneLoaded<TState>(GameStateMachine machine, LevelLoader levelLoader)
			where TState : State<GameStateMachine>
		{
			_levelButtonSpawner.Init(levelLoader);
			
			gameObject.GetComponentElseThrow(out WindowInitializer windowInitializer);
			windowInitializer.WindowsInit(machine.Window);

			machine.EnterIn<TState>();

			var transitionInitializer = new TransitionInitializer<GameStateMachine>(machine, out var subscribe, out _unsubscribe);

			transitionInitializer.InitTransition(_startButton, levelLoader.LoadCurrentLevel);

			subscribe?.Invoke();
		}
	}
}