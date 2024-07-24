using System;
using Agava.WebUtility;
using UnityEngine;

namespace Source.Scripts.Game
{
	public class FocusObserver : IDisposable
	{
		public FocusObserver()
		{
			WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeEvent;
		}

		public void Dispose()
		{
			WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeEvent;
		}

		private void OnInBackgroundChangeEvent(bool isBackground)
		{
			Debug.LogError($"Focus = {isBackground}");
			AudioListener.volume = isBackground ? 0 : 1;
		}
	}
}