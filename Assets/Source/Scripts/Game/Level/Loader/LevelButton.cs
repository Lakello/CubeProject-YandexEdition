using System;
using TMPro;
using UnityEngine;

namespace CubeProject.Game.Level
{
	public class LevelButton : MonoBehaviour
	{
		[SerializeField] private TMP_Text _levelNumber;

		private Action _loadLevel;

		public void Init(int index, Action<int> loadLevel)
		{
			_loadLevel = () => loadLevel(index);
			_levelNumber.text = (index + 1).ToString();
		}

		public void OnClick() =>
			_loadLevel();
	}
}