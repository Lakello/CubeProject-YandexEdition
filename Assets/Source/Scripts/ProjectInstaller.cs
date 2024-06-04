using System;
using System.Collections.Generic;
using LeadTools.Extensions;
using LeadTools.SaveSystem;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using Reflex.Core;
using Source.Scripts.Game;
using Source.Scripts.Game.Level;
using UnityEngine;

namespace CubeProject
{
	public class ProjectInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField] private BackgroundAudioSource _backgroundAudioSourcePrefab;
		[SerializeField] private int _targetFrameRate = 60;
		[SerializeField] private bool _isDebug;

		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			InitBackgroundAudio();

			GameStateMachine gameStateMachine;

			InitStateMachine();

			ProjectInit();

			return;

			#region InitMethods

			void InitBackgroundAudio()
			{
				var source = Instantiate(_backgroundAudioSourcePrefab);
				DontDestroyOnLoad(source.gameObject);
			}

			void InitStateMachine()
			{
				var windowStateMachine = new WindowStateMachine(
					() => new Dictionary<Type, State<WindowStateMachine>>
					{
						[typeof(MenuWindowState)] = new MenuWindowState(),
						[typeof(PlayLevelWindowState)] = new PlayLevelWindowState(),
						[typeof(EndLevelWindowState)] = new EndLevelWindowState(),
						[typeof(SelectLevelWindowState)] = new SelectLevelWindowState(),
						[typeof(LeaderboardWindowState)] = new LeaderboardWindowState(),
					});

				gameStateMachine = new GameStateMachine(
					windowStateMachine,
					() => new Dictionary<Type, State<GameStateMachine>>
					{
						[typeof(MenuState<SelectLevelWindowState>)] = new MenuState<SelectLevelWindowState>(
							() => windowStateMachine.EnterIn<SelectLevelWindowState>()),
						[typeof(MenuState<LeaderboardWindowState>)] = new MenuState<LeaderboardWindowState>(
							() => windowStateMachine.EnterIn<LeaderboardWindowState>()),
						[typeof(MenuState<MenuWindowState>)] = new MenuState<MenuWindowState>(
							() => windowStateMachine.EnterIn<MenuWindowState>()),
						[typeof(PlayLevelState)] = new PlayLevelState(() => windowStateMachine.EnterIn<PlayLevelWindowState>()),
						[typeof(EndLevelState)] = new EndLevelState(() => windowStateMachine.EnterIn<EndLevelWindowState>()),
					});

				containerBuilder.AddSingleton(gameStateMachine, typeof(IStateChangeable<GameStateMachine>));
			}

			void ProjectInit()
			{
				QualitySettings.vSyncCount = 0;
				Application.targetFrameRate = _targetFrameRate;
				
				var levelLoader = gameObject.GetComponentElseThrow<LevelLoader>();

				levelLoader.SetMode(LoaderMode.ByOrder);
				
				var projectInitializer = new GameObject(nameof(ProjectInitializer)).AddComponent<ProjectInitializer>();

				projectInitializer.Init(
					gameStateMachine,
					levelLoader,
					() =>
					{
						var saver = new GameDataSaver();
						saver.Init();
						levelLoader.Init(gameStateMachine);
					});
			}

			#endregion
		}
	}
}