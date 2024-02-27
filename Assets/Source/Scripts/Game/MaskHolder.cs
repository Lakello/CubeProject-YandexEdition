using System;
using UnityEngine;

namespace Source.Scripts.Game
{
	[Serializable]
	public struct MaskHolder
	{
		[SerializeField] private LayerMask _groundMask;
		[SerializeField] private LayerMask _wallMask;
		[SerializeField] private LayerMask _overlapMask;

		public LayerMask GroundMask => _groundMask;

		public LayerMask WallMask => _wallMask;

		public LayerMask OverlapMask => _overlapMask;
	}
}