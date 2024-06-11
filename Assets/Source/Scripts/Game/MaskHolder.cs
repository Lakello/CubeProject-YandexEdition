using System;
using UnityEngine;

namespace CubeProject.Game.Player
{
	[Serializable]
	public struct MaskHolder
	{
		[SerializeField] private LayerMask _groundMask;
		[SerializeField] private LayerMask _wallMask;

		public LayerMask GroundMask => _groundMask;

		public LayerMask WallMask => _wallMask;
	}
}