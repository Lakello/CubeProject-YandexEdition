using LeadTools.Common;
using UnityEngine;

namespace LeadTools.Object
{
	public class ObjectRotator : MonoBehaviour
	{
		[SerializeField] private float _speed;
		[SerializeField] private AxisType _axis;

		private void Update() =>
			transform.Rotate(_axis.Value, _speed * Time.deltaTime);
	}
}