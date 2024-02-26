using System;
using CubeProject.UI;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using UnityEngine;

namespace CubeProject
{
	[RequireComponent(typeof(WindowInitializer))]
	public class MenuSceneInitializer : MonoBehaviour, ISceneLoadHandlerOnState<GameStateMachine>
	{
		[SerializeField] private StartButton _startButton;

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
			
			transitionInitializer.InitTransition(_startButton, () => GameScene.Load<PlayLevelState>(machine));
			
			subscribe?.Invoke();
		}
	}
}