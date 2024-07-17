using System;
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