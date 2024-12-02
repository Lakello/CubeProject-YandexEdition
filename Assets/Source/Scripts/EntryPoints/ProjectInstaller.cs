using CubeProject.Game;
using CubeProject.Game.AudioSystem;
using CubeProject.Game.Level.Loader;
using CubeProject.Yandex;
using EasyTransition;
using LeadTools.Extensions;
using LeadTools.FSM;
using LeadTools.FSM.GameFSM;
using LeadTools.FSM.GameFSM.States;
using LeadTools.FSM.WindowFSM;
using LeadTools.FSM.WindowFSM.States;
using LeadTools.SaveSystem;
using Reflex.Core;
using UnityEngine;

namespace CubeProject.EntryPoints
{
	public class ProjectInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField] private Transition _transitionPrefab;
		[SerializeField] private TransitionSettings _transitionSettings;
		[SerializeField] private BackgroundAudioSource _backgroundAudioSourcePrefab;
		[SerializeField] private int _targetFrameRate = 60;

		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			GlobalDisposableHolder globalDisposableHolder;
			InitDisposableHolder();

			InitBackgroundAudio();

			GameStateMachine gameStateMachine;

			InitStateMachine();

			AdObserver adObserver = new AdObserver();
			globalDisposableHolder.Add(adObserver);

			ProjectInit();

			containerBuilder.AddSingleton(adObserver);

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

				sdkInitializeObserver.Init(OnSdkInitialized);

				return;

				void OnSdkInitialized()
				{
					var saver = new GameDataSaver();
					saver.Init(OnSaverInitialized);
				}

				void OnSaverInitialized()
				{
					var transition = Instantiate(_transitionPrefab);
					transition.transitionSettings = _transitionSettings;
					DontDestroyOnLoad(transition.gameObject);

					levelLoader.Init(gameStateMachine, transition);
					levelLoader.LoadCurrentLevel();

					InitAd();
				}
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