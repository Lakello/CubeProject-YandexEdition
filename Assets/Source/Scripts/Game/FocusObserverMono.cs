using System;
using Agava.WebUtility;
using UnityEngine;

namespace Source.Scripts.Game
{
	public class FocusObserverMono : MonoBehaviour
	{
		private void OnEnable()
		{
			WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeEvent;
		}

		private void OnDisable()
		{
			WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeEvent;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			OnInBackgroundChangeEvent(hasFocus);
		}

		private void OnInBackgroundChangeEvent(bool isBackground)
		{
			Debug.LogError($"Focus = {isBackground}");
			AudioListener.volume = isBackground ? 0 : 1;
		}
	}
}