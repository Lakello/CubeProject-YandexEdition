using System;
using CubeProject.Game.Messages;
using CubeProject.Game.Player.Movement;
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
				.Receive<Message<CubeMoveService>>()
				.Where(message => message.Id == MessageId.StepEnded)
				.Subscribe(_ => OnStepEnded())
				.AddTo(this);
		}

		private void OnStepEnded() =>
			AudioPlaying?.Invoke();
	}
}