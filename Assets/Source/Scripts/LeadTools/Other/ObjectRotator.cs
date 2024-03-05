using UnityEngine;

namespace Source.Scripts.LeadTools.Other
{
	public class ObjectRotator : MonoBehaviour
	{
		[SerializeField] private float _speed;
		[SerializeField] private AxisType _axis;

		private void Update() =>
			transform.Rotate(_axis.Value, _speed * Time.deltaTime);
	}
}