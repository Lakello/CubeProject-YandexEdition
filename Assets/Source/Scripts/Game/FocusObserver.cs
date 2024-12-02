using Agava.WebUtility;
using CubeProject.Yandex;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Game
{
	public class FocusObserver : MonoBehaviour
	{
		private AdObserver _adObserver;

		[Inject]
		private void Inject(AdObserver adObserver) =>
			_adObserver = adObserver;

		private void OnEnable()
		{
			Application.focusChanged += OnInBackgroundChangeApp;
			WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeWeb;
		}

		private void OnDisable()
		{
			Application.focusChanged -= OnInBackgroundChangeApp;
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
			if (_adObserver.IsMute == false)
				AudioListener.volume = value ? 0 : 1;
		}
	}
}