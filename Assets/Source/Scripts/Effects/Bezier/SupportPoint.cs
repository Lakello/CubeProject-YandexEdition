using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Effects
{
	public class SupportPoint : MonoBehaviour
	{
		private ControlPoint _controlPoint;

		public ControlPoint ControlPoint => _controlPoint ??= gameObject.GetComponentInChildrenElseThrow(out _controlPoint);
	}
}