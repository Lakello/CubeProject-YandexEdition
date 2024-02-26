using UnityEngine;

namespace CubeProject.Game
{
	public class LinkToCreatorOpener : MonoBehaviour
	{
		[SerializeField] private string _url;

		public void OnClick() =>
			Application.OpenURL(_url);
	}
}