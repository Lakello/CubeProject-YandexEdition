using CubeProject.Game.Messages;
using CubeProject.Game.Player;
using CubeProject.Save.Data;
using Cysharp.Threading.Tasks;
using LeadTools.NaughtyAttributes;
using LeadTools.SaveSystem;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using Source.Scripts.Yandex;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CubeProject.Game.Level
{
	public class LevelLoader : MonoBehaviour
	{
		[SerializeField] [Scene] private string[] _levels;

		private int _currentSceneIndex;
		private GameStateMachine _gameStateMachine;
		private LoaderMode _currentMode;
		private AdService _adService;
		private CompositeDisposable _compositeDisposable;
		private bool _canLoadLevel;

		public int LevelsCount => _levels.Length;

		public void Init(GameStateMachine gameStateMachine)
		{
			if (_gameStateMachine != null)
				return;

			_gameStateMachine = gameStateMachine;

			_currentSceneIndex = GameDataSaver.Instance.Get<CurrentLevel>().Value;
			
			_compositeDisposable = new CompositeDisposable();

			MessageBroker.Default
				.Receive<Message<AdService>>()
				.Where(message => message.Id == MessageId.ResumeLevelLoading)
				.Subscribe(_ => OnResumeLevelLoading())
				.AddTo(_compositeDisposable);
			
			MessageBroker.Default
				.Receive<Message<AdService>>()
				.Where(message => message.Id == MessageId.SuspendLevelLoading)
				.Subscribe(_ => OnSuspendLevelLoading())
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
				.Publish(new Message<LevelLoader>(MessageId.PreLevelLoading));
			
			await UniTask.WaitUntil(() => _canLoadLevel, cancellationToken: this.GetCancellationTokenOnDestroy());
			await TypedScene<GameStateMachine>.LoadScene<PlayLevelState<PlayLevelWindowState>, LevelLoader>(
				_levels[index],
				LoadSceneMode.Single,
				_gameStateMachine,
				this);

#if !UNITY_EDITOR
			Agava.YandexGames.YandexGamesSdk.GameReady();
#endif
		}

		private void OnResumeLevelLoading() =>
			_canLoadLevel = true;

		private void OnSuspendLevelLoading() =>
			_canLoadLevel = false;
	}
}