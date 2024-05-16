using System;
using System.Collections.Generic;
using LeadTools.Extensions;
using LeadTools.Object;
using LeadTools.Other;
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
		[SerializeField] private AudioSourceHolder _audioSourceHolderPrefab;
		[SerializeField] private AudioClip _backgroundClip;
		[SerializeField] [Range(0, 1)] private float _volume;
		[SerializeField] private int _targetFrameRate = 60;
		[SerializeField] private bool _isDebug;

		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = _targetFrameRate;

			DontDestroyMono mono;
			InitMono();

			ObjectSpawner<AudioSourceHolder, AudioInitData> audioSpawner;
			InitAudioSpawner();

			InitBackgroundAudio();

			GameStateMachine gameStateMachine;

			InitStateMachine();

			ProjectInit();

			return;

			#region InitMethods
			
			void InitMono()
			{
				mono = new GameObject(nameof(DontDestroyMono)).AddComponent<DontDestroyMono>();
				DontDestroyOnLoad(mono.gameObject);
			}

			void InitAudioSpawner()
			{
				audioSpawner = new ObjectSpawner<AudioSourceHolder, AudioInitData>(mono.transform, _audioSourceHolderPrefab);

				containerBuilder.AddSingleton(audioSpawner);
			}

			void InitBackgroundAudio()
			{
				_ = audioSpawner.Spawn(
					new AudioInitData
					{
						Clip = _backgroundClip,
						IsLoop = true,
						Volume = _volume,
					});
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
				var levelLoader = gameObject.GetComponentElseThrow<LevelLoader>();

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