using System;
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
		[SerializeField] private bool _isDebug;

		private int _currentSceneIndex = 0;
		private GameStateMachine _gameStateMachine;

		public int LevelsCount => _levels.Length;

		public void Init(GameStateMachine gameStateMachine)
		{
			if (_gameStateMachine != null)
			{
				return;
			}

			_gameStateMachine = gameStateMachine;

			if (_isDebug is false)
			{
				_currentSceneIndex = GameDataSaver.Instance.Get<CurrentLevel>().Value;
			}
		}

		public void LoadNextLevel() =>
			LoadLevelAtIndex(++_currentSceneIndex);

		public void LoadCurrentLevel() =>
			LoadLevelAtIndex(_currentSceneIndex);

		public void LoadLevelAtIndex(int index)
		{
			Debug.Log($"LOAD LEVEL = {_currentSceneIndex}");
			if (index < 0)
			{
				return;
			}

			if (index > _levels.Length - 1)
			{
				index = 0;
			}

			_currentSceneIndex = index;
			
			if (_isDebug is false)
			{
				GameDataSaver.Instance.Set(new CurrentLevel(_currentSceneIndex));
			}

			TypedScene<GameStateMachine>.LoadScene<PlayLevelState, LevelLoader>(
				_levels[index],
				LoadSceneMode.Single,
				_gameStateMachine,
				this);
		}
	}
}