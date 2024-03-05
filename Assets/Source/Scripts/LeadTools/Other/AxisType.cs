using System;
using LeadTools.NaughtyAttributes;
using UnityEngine;

namespace Source.Scripts.LeadTools.Other
{
	[Serializable]
	public class AxisType
	{
		[SerializeField] [Dropdown(nameof(GetDirection))] private Vector3 _value;

		public Vector3 Value => _value;

		private DropdownList<Vector3> GetDirection() =>
			new DropdownList<Vector3>
			{
				{
					"X", Vector3.right
				},
				{
					"Y", Vector3.up
				},
				{
					"Z", Vector3.forward
				}
			};
	}
}