using System;
using CubeProject.PlayableCube;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(Collider))]
	public class EndPoint : MonoBehaviour
	{
		public event Action LevelEnded;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube _))
			{
				LevelEnded?.Invoke();
			}
		}
	}
}