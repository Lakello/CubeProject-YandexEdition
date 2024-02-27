using UnityEngine;

namespace CubeProject.Game
{
	public class GameExitButton : MonoBehaviour
	{
		public void OnClick() =>
			Application.Quit();
	}
}