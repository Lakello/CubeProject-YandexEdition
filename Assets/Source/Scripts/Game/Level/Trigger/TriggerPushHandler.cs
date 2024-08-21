using System;
using CubeProject.Game.Level.Push;
using CubeProject.Game.Player;
using UnityEngine;

namespace CubeProject.Game.Level.Trigger
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