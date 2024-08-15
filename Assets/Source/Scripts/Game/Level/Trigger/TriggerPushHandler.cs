using System;
using Game.Player;
using UnityEngine;

namespace CubeProject.Tips
{
	public class TriggerPushHandler : MonoBehaviour, IPushHandler
	{
		public event Action Pushing;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out CubeEntity _))
				Pushing?.Invoke();
		}
	}
}