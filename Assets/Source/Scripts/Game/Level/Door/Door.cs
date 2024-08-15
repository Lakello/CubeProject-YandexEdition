using UnityEngine;

namespace Game.Player
{
	public class Door : MonoBehaviour
	{
		[SerializeField] private Transform _center;
		[SerializeField] private Vector3 _openRotation;
		[SerializeField] private Vector3 _openPosition;
		[SerializeField] private Vector3 _openScale;
		[SerializeField] private Vector3 _closeRotation;
		[SerializeField] private Vector3 _closePosition;
		[SerializeField] private Vector3 _closeScale;

		public Transform Center => _center;
		public Vector3 OpenRotation => _openRotation;
		public Vector3 OpenPosition => _openPosition;
		public Vector3 OpenScale => _openScale;
		public Vector3 CloseRotation => _closeRotation;
		public Vector3 ClosePosition => _closePosition;
		public Vector3 CloseScale => _closeScale;
	}
}