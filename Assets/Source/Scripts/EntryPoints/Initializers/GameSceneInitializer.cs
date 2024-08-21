using CubeProject.Game;
using CubeProject.Game.Level.LevelPoint;
using CubeProject.Game.Level.Loader;
using CubeProject.Game.UI.Buttons;
using CubeProject.Saves.Data;
using LeadTools.Extensions;
using LeadTools.FSM;
using LeadTools.FSM.GameFSM;
using LeadTools.FSM.GameFSM.States;
using LeadTools.FSM.Transit;
using LeadTools.FSM.WindowFSM;
using LeadTools.FSM.WindowFSM.States;
using LeadTools.SaveSystem;
using LeadTools.TypedScenes.Core;
using LeadTools.TypedScenes.Scenes;
using LeadTools.Utils;
using UnityEditor;
using UnityEngine;

namespace CubeProject.EntryPoints.Initializers
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