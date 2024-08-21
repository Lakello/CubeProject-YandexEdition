using System.Collections.Generic;
using UnityEngine;

namespace CubeProject.Game.Level.Loader
{
	public static class LevelButtonFactory
	{
		public static Queue<LevelButton> Create(LevelButton levelButtonPrefab, LevelLoader levelLoader)
		{
			Queue<LevelButton> buttons = new Queue<LevelButton>();

			for (int i = 0; i < levelLoader.LevelsCount; i++)
			{
				var button = Object.Instantiate(levelButtonPrefab);
				button.Init(i, levelLoader.LoadLevelAtIndex);

				buttons.Enqueue(button);
			}

			return buttons;
		}
	}
}