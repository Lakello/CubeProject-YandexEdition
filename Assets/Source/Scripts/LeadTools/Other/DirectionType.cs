using System;
using LeadTools.NaughtyAttributes;
using UnityEngine;

namespace CubeProject.Tips
{
	[Serializable]
	public struct DirectionType
	{
		[SerializeField] [Dropdown(nameof(GetDirection))] private Vector3 _value;

		public Vector3 Value => _value;

		private DropdownList<Vector3> GetDirection() =>
			new DropdownList<Vector3>
			{
				{
					"Right", Vector3.right
				},
				{
					"Left", Vector3.left
				},
				{
					"Forward", Vector3.forward
				},
				{
					"Back", Vector3.back
				},
			};
	}
}