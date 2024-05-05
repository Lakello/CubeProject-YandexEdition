using CubeProject.PlayableCube;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(AudioSource))]
	public class BarrierField : MonoBehaviour
	{
		[SerializeField] private ChargeConsumer _chargeConsumer;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube cube)
				&& _chargeConsumer.IsCharged
				&& cube.Component.ChargeHolder.IsCharged is false)
				cube.Kill();
		}
	}
}