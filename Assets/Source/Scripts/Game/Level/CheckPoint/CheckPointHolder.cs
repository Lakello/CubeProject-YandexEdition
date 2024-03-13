using Source.Scripts.Game.Level;
using UnityEngine;

namespace CubeProject.Game
{
	public class CheckPointHolder : MonoBehaviour
	{
		public ICheckPoint CurrentCheckPoint { get; private set; }

		public void Init(SpawnPoint spawnPoint)
		{
			if (CurrentCheckPoint is not null)
			{
				return;
			}
			
			CurrentCheckPoint = spawnPoint;
		}
		
		public void OnActiveChanged(CheckPoint checkPoint)
		{
			if (CurrentCheckPoint is not null
				&& CurrentCheckPoint is CheckPoint point)
			{
				point.DeActive();
			}

			CurrentCheckPoint = checkPoint;
		}
	}
}