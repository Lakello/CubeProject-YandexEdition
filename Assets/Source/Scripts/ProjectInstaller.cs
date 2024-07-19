using CubeProject.Game.Level;
using CubeProject.Game.Player;
using LeadTools.Extensions;
using LeadTools.SaveSystem;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using Reflex.Core;
using Source.Scripts.Game;
using Source.Scripts.Yandex;
using UnityEngine;

namespace CubeProject
{
	public class ProjectInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField] private BackgroundAudioSource _backgroundAudioSourcePrefab;
		[SerializeField] private int _targetFrameRate = 60;

		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			GlobalDisposableHolder globalDisposableHolder;
			InitDisposableHolder();
				
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
				var windowStateMachine = new WindowStateMachine()
					.AddState<MenuWindowState>()
					.AddState<PlayLevelWindowState>()
					.AddState<EndLevelWindowState>()
					.AddState<SelectLevelWindowState>();

				gameStateMachine = new GameStateMachine(windowStateMachine)
					.SetStateInstanceParameters(windowStateMachine)
					.AddState<MenuState<SelectLevelWindowState>>()
					.AddState<MenuState<MenuWindowState>>()
					.AddState<PlayLevelState<PlayLevelWindowState>>()
					.AddState<EndLevelState<EndLevelWindowState>>();

				containerBuilder.AddSingleton(gameStateMachine, typeof(IStateChangeable<GameStateMachine>));
			}

			void ProjectInit()
			{
				QualitySettings.vSyncCount = 0;
				Application.targetFrameRate = _targetFrameRate;

				var levelLoader = gameObject.GetComponentElseThrow<LevelLoader>();

				levelLoader.SetMode(LoaderMode.ByOrder);

				var sdkInitializeObserver = new GameObject(nameof(SDKInitializeObserver)).AddComponent<SDKInitializeObserver>();

				sdkInitializeObserver.Init(() =>
					{
						var saver = new GameDataSaver();
						saver.Init(() =>
						{
							levelLoader.Init(gameStateMachine);
							levelLoader.LoadCurrentLevel();
							
							InitAd();
						});
					});
			}

			void InitDisposableHolder()
			{
				var holderObject = new GameObject(nameof(GlobalDisposableHolder));
				globalDisposableHolder = holderObject.AddComponent<GlobalDisposableHolder>();
				
				DontDestroyOnLoad(holderObject);
			}
			
			void InitAd()
			{
				globalDisposableHolder.Add(new AdService());
			}
			
			#endregion
		}
	}
}