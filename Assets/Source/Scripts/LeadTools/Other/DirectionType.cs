using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CubeProject.Tips
{
	[Serializable]
	public class DirectionType
	{
		[SerializeField] [ValueDropdown(nameof(GetDirection))] private Vector3 _value;

		public Vector3 Value => _value;

		private ValueDropdownList<Vector3> GetDirection() =>
			new ValueDropdownList<Vector3>
			{
				{
					"Back", Vector3.left
				},
				{
					"Forward", Vector3.right
				},
				{
					"Right", Vector3.back
				},
				{
					"Left", Vector3.forward
				},
			};
	}
}