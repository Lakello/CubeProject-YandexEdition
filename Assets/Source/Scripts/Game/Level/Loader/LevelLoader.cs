using CubeProject.Save.Data;
using LeadTools.NaughtyAttributes;
using LeadTools.SaveSystem;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Scripts.Game.Level
{
	public class LevelLoader : MonoBehaviour
	{
		[SerializeField] [Scene] private string[] _levels;

		private int _currentSceneIndex = -1;
		private GameStateMachine _gameStateMachine;

		public void Init(GameStateMachine gameStateMachine)
		{
			if (_gameStateMachine != null)
			{
				return;
			}

			_gameStateMachine = gameStateMachine;
			_currentSceneIndex = GameDataSaver.Instance.Get<CurrentLevel>().Value;
		}

		public void LoadNextLevel() =>
			LoadLevelAtIndex(++_currentSceneIndex);

		public void LoadCurrentLevel() =>
			LoadLevelAtIndex(_currentSceneIndex);

		public void LoadLevelAtIndex(int index)
		{
			if (index < 0)
			{
				return;
			}

			_currentSceneIndex = index;
			GameDataSaver.Instance.Set(new CurrentLevel(_currentSceneIndex));

			TypedScene<GameStateMachine>.LoadScene<PlayLevelState, LevelLoader>(
				_levels[index],
				LoadSceneMode.Single,
				_gameStateMachine,
				this);
		}
	}
}