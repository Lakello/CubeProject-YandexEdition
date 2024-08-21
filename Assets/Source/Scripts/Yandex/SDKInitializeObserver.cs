using System;
using UnityEngine;
#if !UNITY_EDITOR 
using Cysharp.Threading.Tasks;
#endif

namespace CubeProject.Yandex
{
	public class SDKInitializeObserver : MonoBehaviour
	{
		private Action _callBack;

#if !UNITY_EDITOR 
		private async void Start()
		{
            await Agava.YandexGames.YandexGamesSdk.Initialize(() => _callBack?.Invoke());

            if (Agava.YandexGames.PlayerAccount.IsAuthorized == false)
	            Agava.YandexGames.PlayerAccount.StartAuthorizationPolling(1500);
		}
#else
		private void Start()
		{
			_callBack?.Invoke();
		}
#endif

		public void Init(Action sdkInitSuccessCallBack = null) =>
			_callBack = sdkInitSuccessCallBack;
	}
}