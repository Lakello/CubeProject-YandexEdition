using System;
using CubeProject.PlayableCube;
using UnityEngine;

namespace CubeProject.Game
{
	public class BarrierField : MonoBehaviour, IAudioSubject
	{
		[SerializeField] private ChargeConsumer _chargeConsumer;

		public event Action AudioPlaying;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube cube)
				&& _chargeConsumer.IsCharged
				&& cube.Component.ChargeHolder.IsCharged is false)
			{
				AudioPlaying?.Invoke();
				cube.Kill();
			}
		}
	}
}