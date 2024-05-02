using System;
using UnityEngine;

namespace CubeProject.Tips
{
	[Serializable]
	public struct TipKeyData
	{
		[SerializeField] private DirectionType _direction;

		public Vector3 Direction => _direction.Value;
	}
}