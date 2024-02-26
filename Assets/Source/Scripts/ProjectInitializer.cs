using System;
using System.Collections;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using UnityEngine;

namespace CubeProject
{
	public class ProjectInitializer : MonoBehaviour
	{
		private Action _callBack;
		private GameStateMachine _gameStateMachine;

		private void Start() =>
			StartCoroutine(InitSDK());

		public void Init(GameStateMachine gameStateMachine, Action sdkInitSuccessCallBack = null)
		{
			_gameStateMachine = gameStateMachine;
			_callBack = sdkInitSuccessCallBack;
		}

		private IEnumerator InitSDK()
		{
// #if UNITY_WEBGL || !UNITY_EDITOR
//             yield return YandexGamesSdk.Initialize(() => _callBack?.Invoke());
//
//             if (PlayerAccount.IsAuthorized == false)
//                 PlayerAccount.StartAuthorizationPolling(1500);
// #else
			_callBack?.Invoke();
// #endif

			MenuScene.Load<MenuState>(_gameStateMachine);

			yield return null;
		}
	}
}