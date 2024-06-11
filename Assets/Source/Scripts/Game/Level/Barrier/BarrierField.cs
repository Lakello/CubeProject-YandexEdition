using System;
using CubeProject.Game.Player;
using UnityEngine;

namespace CubeProject.Game.Player
{
	public class BarrierField : MonoBehaviour, IAudioSubject
	{
		[SerializeField] private ChargeConsumer _chargeConsumer;

		public event Action AudioPlaying;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out CubeEntity cube)
				&& _chargeConsumer.IsCharged
				&& cube.Component.ChargeHolder.IsCharged is false)
			{
				AudioPlaying?.Invoke();
				cube.Kill();
			}
		}
	}
}