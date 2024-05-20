using System;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using Reflex.Attributes;
using Source.Scripts.Game.Messages;
using UniRx;
using UnityEngine;

namespace Source.Scripts.Game
{
	public class CubeStepAudioSource : MonoBehaviour, IAudioSource
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