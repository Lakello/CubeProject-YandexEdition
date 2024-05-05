using System;
using CubeProject.PlayableCube;
using UnityEngine;

namespace CubeProject.Tips
{
	public class TriggerPushHandler : MonoBehaviour, IPushHandler
	{
		public event Action Pushing;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube _))
				Pushing?.Invoke();
		}
	}
}