using System;
using CubeProject.Player;
using CubeProject.Tips;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Game
{
	public class UseObject : MonoBehaviour, IUsable
	{
		private UseTipKeyHandler _useTipKeyHandler;

		public event Action<Cube> TryUsing;

		[Inject]
		private void Inject(UseTipKeyHandler useTipKeyHandler) =>
			_useTipKeyHandler = useTipKeyHandler;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube _))
			{
				_useTipKeyHandler.OnCanUseChanged(true);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Cube _))
			{
				_useTipKeyHandler.OnCanUseChanged(false);
			}
		}

		public void TryUse(Cube cube) =>
			TryUsing?.Invoke(cube);
	}
}