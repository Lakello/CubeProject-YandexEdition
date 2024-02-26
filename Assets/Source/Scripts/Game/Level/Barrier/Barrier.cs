using CubeProject.Player;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(AudioSource))]
	public class Barrier : ChargeConsumer
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube cube) && IsCharged)
			{
				if (cube.ComponentsHolder.ChargeHolder.IsCharged is false)
				{
					gameObject.GetComponentElseThrow(out AudioSource audio);
					audio.Play();
					cube.Kill();
				}
			}
		}
	}
}