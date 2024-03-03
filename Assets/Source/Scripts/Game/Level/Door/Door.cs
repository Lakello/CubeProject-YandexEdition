using System;
using UnityEngine;

namespace CubeProject.Game
{
	public class Door : MonoBehaviour
	{
		[SerializeField] private Vector3 _openPosition;
		[SerializeField] private Vector3 _openScale;
		[SerializeField] private Vector3 _closePosition;
		[SerializeField] private Vector3 _closeScale;

		public Vector3 OpenPosition => _openPosition;

		public Vector3 OpenScale => _openScale;

		public Vector3 ClosePosition => _closePosition;

		public Vector3 CloseScale => _closeScale;

		public Vector3 StartPosition { get; set; }

		public Vector3 StartScale { get; set; }
	}
}