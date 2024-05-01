using CubeProject.PlayableCube;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(AudioSource))]
	public class BarrierField : MonoBehaviour
	{
		[SerializeField] private ChargeConsumer _chargeConsumer;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube cube) && _chargeConsumer.IsCharged)
			{
				if (cube.Component.ChargeHolder.IsCharged is false)
				{
					gameObject.GetComponentElseThrow(out AudioSource audio);
					audio.Play();
					cube.Kill();
				}
			}
		}
	}
}