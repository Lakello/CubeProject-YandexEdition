using System;
using LeadTools.NaughtyAttributes;
using UnityEngine;

namespace CubeProject.Tips
{
	[Serializable]
	public class DirectionType
	{
		[SerializeField] [Dropdown(nameof(GetDirection))] private Vector3 _value;

		public Vector3 Value => _value;

		private DropdownList<Vector3> GetDirection() =>
			new DropdownList<Vector3>
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