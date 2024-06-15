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
					"Back", Vector3.right
				},
				{
					"Forward", Vector3.left
				},
				{
					"Right", Vector3.forward
				},
				{
					"Left", Vector3.back
				},
			};
	}
}