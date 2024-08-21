using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeadTools.Utils
{
	public class SceneNameView : MonoBehaviour
	{
		[SerializeField] private TMP_Text _output;

		public void Show() =>
			_output.text = SceneManager.GetActiveScene().name;

		public void Hide() =>
			_output.text = string.Empty;
	}
}