using System;
using System.Collections;
using Agava.YandexGames;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using LeadTools.TypedScenes;
using Source.Scripts.Game.Level;
using UnityEngine;

namespace CubeProject
{
	public class ProjectInitializer : MonoBehaviour
	{
		private Action _callBack;
		private LevelLoader _levelLoader;
		private GameStateMachine _gameStateMachine;

		private IEnumerator Start()
		{
#if !UNITY_EDITOR
            yield return YandexGamesSdk.Initialize(() => _callBack?.Invoke());

            if (PlayerAccount.IsAuthorized == false)
                PlayerAccount.StartAuthorizationPolling(1500);
#else
			_callBack?.Invoke();
#endif

			MenuScene.Load<MenuState<MenuWindowState>, LevelLoader>(_gameStateMachine, _levelLoader);

			yield return null;
		}

		public void Init(
			GameStateMachine gameStateMachine,
			LevelLoader levelLoader,
			Action sdkInitSuccessCallBack = null)
		{
			_gameStateMachine = gameStateMachine;
			_levelLoader = levelLoader;
			_callBack = sdkInitSuccessCallBack;
		}
	}
}