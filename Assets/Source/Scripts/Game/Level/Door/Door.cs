using UnityEngine;

namespace CubeProject.Game
{
	public class Door : MonoBehaviour
	{
		[SerializeField] private float _openPositionZ;
		[SerializeField] private float _openScaleZ;
		[SerializeField] private float _closePositionZ;
		[SerializeField] private float _closeScaleZ;

		public float OpenPositionZ => _openPositionZ;

		public float OpenScaleZ => _openScaleZ;
		
		public float ClosePositionZ => _closePositionZ;

		public float CloseScaleZ => _closeScaleZ;

		public float StartPositionZ { get; set; }
		
		public float StartScaleZ { get; set; }
	}
}