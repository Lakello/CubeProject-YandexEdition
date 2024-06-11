using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CubeProject.Game.Player
{
	[Serializable]
	public class Directions
	{
		private Dictionary<Vector3, bool> _directions = new Dictionary<Vector3, bool>
		{
			[Vector3.forward] = true,
			[Vector3.back] = true,
			[Vector3.left] = true,
			[Vector3.right] = true,
		};

		public Vector3 GetAnyDirection() =>
			_directions.FirstOrDefault(direction => direction.Value).Key;

		public void SetValue(Vector3 direction, bool value)
		{
			if (_directions.ContainsKey(direction))
				_directions[direction] = value;
			else
				throw new ArgumentException();
		}
	}
}