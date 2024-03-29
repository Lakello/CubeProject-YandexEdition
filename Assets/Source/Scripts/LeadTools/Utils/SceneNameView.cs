using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CubeProject.LeadTools.Utils
{
	public class SceneNameView : MonoBehaviour
	{
		[SerializeField] private TMP_Text _content;

		private void Awake() =>
			_content.text = SceneManager.GetActiveScene().name;
	}
}