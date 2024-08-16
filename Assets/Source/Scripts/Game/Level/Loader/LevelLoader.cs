using Game.Level.Message;
using CubeProject.Game.Messages;
using Game.Player;
using CubeProject.Save.Data;
using Cysharp.Threading.Tasks;
using EasyTransition;
using LeadTools.NaughtyAttributes;
using LeadTools.SaveSystem;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using Yandex;
using Yandex.Messages;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CubeProject.Game.Level
{
	public class LevelLoader : MonoBehaviour
	{
		private readonly M_PreLevelLoading _preLoadingMessage = new M_PreLevelLoading();
		
		[SerializeField] [Scene] private string[] _levels;

		private int _currentSceneIndex;
		private GameStateMachine _gameStateMachine;
		private LoaderMode _currentMode;
		private AdService _adService;
		private CompositeDisposable _compositeDisposable;
		private Transition _sceneTransitionAnimation;
		private bool _canLoadLevel;

		public int LevelsCount => _levels.Length;

		public void Init(GameStateMachine gameStateMachine, Transition transition)
		{
			if (_gameStateMachine != null)
				return;

			_gameStateMachine = gameStateMachine;

			_sceneTransitionAnimation = transition;

			_currentSceneIndex = GameDataSaver.Instance.Get<CurrentLevel>().Value;
			
			_compositeDisposable = new CompositeDisposable();

			MessageBroker.Default
				.Receive<M_ADCooldown>()
				.Subscribe(_ => ResumeLevelLoading())
				.AddTo(_compositeDisposable);
			
			MessageBroker.Default
				.Receive<M_ADReady>()
				.Subscribe(_ => SuspendLevelLoading())
				.AddTo(_compositeDisposable);
			
			_canLoadLevel = true;
		}

		public void SetMode(LoaderMode mode) =>
			_currentMode = mode;

		public void LoadNextLevel()
		{
			switch (_currentMode)
			{
				case LoaderMode.Random:
					Random();

					break;
				default:
					ByOrder();

					break;
			}

			return;

			void ByOrder() =>
				LoadLevelAtIndex(++_currentSceneIndex);

			void Random(int currentIteration = 0)
			{
				const int maxIteration = 10;

				var randomIndex = UnityEngine.Random.Range(0, LevelsCount);

				if (currentIteration < maxIteration && randomIndex == _currentSceneIndex)
					Random(++currentIteration);
				else
					LoadLevelAtIndex(randomIndex);
			}
		}

		public void LoadCurrentLevel() =>
			LoadLevelAtIndex(_currentSceneIndex);

		public async void LoadLevelAtIndex(int index)
		{
			if (index < 0)
				index = 0;

			if (index > _levels.Length - 1)
				index = 0;

			_currentSceneIndex = index;

			GameDataSaver.Instance.Set(new CurrentLevel(_currentSceneIndex));
			
			MessageBroker.Default
				.Publish(_preLoadingMessage);
			
			await UniTask.WaitUntil(() => _canLoadLevel, cancellationToken: this.GetCancellationTokenOnDestroy());
			
			_sceneTransitionAnimation.InPlay();

			await UniTask.WaitForSeconds(_sceneTransitionAnimation.transitionSettings.transitionTime);
			
			await TypedScene<GameStateMachine>.LoadScene<PlayLevelState<PlayLevelWindowState>, LevelLoader>(
				_levels[index],
				LoadSceneMode.Single,
				_gameStateMachine,
				this);
			
			_sceneTransitionAnimation.OutPlay();

			await UniTask.WaitForSeconds(_sceneTransitionAnimation.transitionSettings.destroyTime);
			
#if !UNITY_EDITOR
			Agava.YandexGames.YandexGamesSdk.GameReady();
#endif
		}

		private void ResumeLevelLoading() =>
			_canLoadLevel = true;

		private void SuspendLevelLoading() =>
			_canLoadLevel = false;
	}
}