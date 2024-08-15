using System;
using Game.Player.Messages;
using UniRx;
using UnityEngine;

namespace Game.Player
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