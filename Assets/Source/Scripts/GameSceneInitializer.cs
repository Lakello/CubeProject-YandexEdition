using CubeProject.Game.Level;
using Game.Player;
using CubeProject.InputSystem;
using CubeProject.LeadTools.Utils;
using CubeProject.Save.Data;
using LeadTools.Extensions;
using LeadTools.SaveSystem;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using Source.Scripts.Game.UI.Buttons;
using UnityEditor;
using UnityEngine;

namespace CubeProject
{
	[RequireComponent(typeof(WindowInitializer))]
	public class GameSceneInitializer :
		MonoBehaviour,
		ISceneLoadHandlerOnStateAndArgument<GameStateMachine, LevelLoader>
	{
		[SerializeField] private NextLevelButton _nextLevelButton;
		[SerializeField] private SceneNameView _sceneNameView;
		[SerializeField] private bool _isDebug;
		[SerializeField] private EndPoint _endPoint;

		private TransitionInitializer<GameStateMachine> _transitionInitializer;
		private LevelLoader _levelLoader;

		#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_endPoint == null)
			{
				_endPoint = FindObjectOfType<EndPoint>();
				EditorUtility.SetDirty(this);
			}
		}
		#endif

		private void OnDisable()
		{
			_transitionInitializer?.Unsubscribe();
			_nextLevelButton.StateTransiting -= OnLevelEnded;
		}

		public void OnSceneLoaded<TState>(GameStateMachine machine, LevelLoader levelLoader)
			where TState : State<GameStateMachine>
		{
			_levelLoader = levelLoader;

			_nextLevelButton.StateTransiting += OnLevelEnded;
			
			gameObject.GetComponentElseThrow(out WindowInitializer windowInitializer);
			windowInitializer.WindowsInit(machine.Window);
			machine.EnterIn<TState>();

			_transitionInitializer = new TransitionInitializer<GameStateMachine>(machine)
				.InitTransition<EndLevelState<EndLevelWindowState>>(_endPoint);

			if (_isDebug)
			{
				var backToMenuHandler = new BackToMenuHandler(
					gameObject.GetComponentElseThrow<GameSceneInstaller>().InputService);

				_transitionInitializer.InitTransition(backToMenuHandler, LoadMenu);
				
				_sceneNameView.Show();
			}
			else
			{
				_sceneNameView.Hide();
			}

			_transitionInitializer.Subscribe();

			return;

			void LoadMenu() =>
				MenuScene.Load<MenuState<MenuWindowState>, LevelLoader>(machine, _levelLoader);
		}

		private void OnLevelEnded()
		{
			if (GameDataSaver.Instance.Get<CurrentLevel>().Value + 1 >= _levelLoader.LevelsCount)
				_levelLoader.SetMode(LoaderMode.Random);

			_levelLoader.LoadNextLevel();
		}
	}
}