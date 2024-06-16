using System;
using System.Collections;
using Agava.YandexGames;
using UnityEngine;

namespace CubeProject
{
	public class SDKInitializeObserver : MonoBehaviour
	{
		private Action _callBack;

		private IEnumerator Start()
		{
#if !UNITY_EDITOR
            yield return YandexGamesSdk.Initialize(() => _callBack?.Invoke());

            if (PlayerAccount.IsAuthorized == false)
                PlayerAccount.StartAuthorizationPolling(1500);
#else
			_callBack?.Invoke();
#endif
			yield return null;
		}

		public void Init(Action sdkInitSuccessCallBack = null) =>
			_callBack = sdkInitSuccessCallBack;
	}
}