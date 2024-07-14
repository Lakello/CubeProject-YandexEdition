using System;
using System.Collections;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CubeProject
{
	public class SDKInitializeObserver : MonoBehaviour
	{
		private Action _callBack;

#if !UNITY_EDITOR
		private async void Start()
		{
            await YandexGamesSdk.Initialize(() => _callBack?.Invoke());

            if (PlayerAccount.IsAuthorized == false)
                PlayerAccount.StartAuthorizationPolling(1500);
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