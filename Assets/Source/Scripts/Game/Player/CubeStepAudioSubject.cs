using System;
using CubeProject.Game.AudioSystem;
using CubeProject.Game.Player.CubeService.Messages;
using UniRx;
using UnityEngine;

namespace CubeProject.Game.Player
{
	public class CubeStepAudioSubject : MonoBehaviour, IAudioSubject
	{
		public event Action AudioPlaying;

		private void Awake()
		{
			MessageBroker.Default
				.Receive<M_StepEnded>()
				.Subscribe(_ => OnStepEnded())
				.AddTo(this);
		}

		private void OnStepEnded() =>
			AudioPlaying?.Invoke();
	}
}