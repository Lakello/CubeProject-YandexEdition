using System;
using CubeProject.Player;
using UnityEngine;

namespace CubeProject.Tips
{
	public class TipKeyPushHandler : MonoBehaviour, IPushHandler
	{
		public event Action Pushing;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube _))
			{
				Pushing?.Invoke();
			}
		}
	}
}