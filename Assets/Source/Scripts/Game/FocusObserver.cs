using Agava.WebUtility;
using UnityEngine;

namespace Source.Scripts.Game
{
	public class FocusObserver : MonoBehaviour
	{
		private void OnEnable()
		{
			Application.focusChanged += OnInBackgroundChangeApp;
			WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeWeb;
		}

		private void OnDisable()
		{
			WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeWeb;
		}

		private void OnInBackgroundChangeApp(bool inApp)
		{
			ChangeVolume(!inApp);
		}

		private void OnInBackgroundChangeWeb(bool isBackground)
		{
			ChangeVolume(isBackground);
		}

		private void ChangeVolume(bool value)
		{
			AudioListener.volume = value ? 0 : 1;
		}
	}
}