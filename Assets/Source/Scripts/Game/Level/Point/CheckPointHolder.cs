using UnityEngine;

namespace CubeProject.Game
{
	public class CheckPointHolder : MonoBehaviour
	{
		public CheckPoint CurrentCheckPoint { get; private set; }

		public void OnActiveChanged(CheckPoint checkPoint)
		{
			if (CurrentCheckPoint is not null)
			{
				CurrentCheckPoint.DeActive();
			}

			CurrentCheckPoint = checkPoint;
		}
	}
}