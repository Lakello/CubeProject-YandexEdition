using UnityEngine;

namespace Source.Scripts.Game.Level
{
	public class LevelButtonSpawner : MonoBehaviour
	{
		[SerializeField] private LevelButton _levelButtonPrefab;
		[SerializeField] private RectTransform _content;
		
		private LevelLoader _levelLoader;
		
		public void Init(LevelLoader levelLoader) =>
			_levelLoader = levelLoader;

		private void Start() =>
			SpawnButtons();

		private void SpawnButtons()
		{
			for (int i = 0; i < _levelLoader.LevelsCount; i++)
			{
				var button = Instantiate(_levelButtonPrefab, _content);
				button.Init(i, (index) => _levelLoader.LoadLevelAtIndex(index));
			}
		}
	}
}