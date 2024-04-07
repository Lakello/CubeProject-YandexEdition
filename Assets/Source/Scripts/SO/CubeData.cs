using Sirenix.OdinInspector;
using UnityEngine;

namespace CubeProject.SO
{
	[CreateAssetMenu(menuName = "Cube/Data")]
	public class CubeData : ScriptableObject
	{
		[SerializeField] private float _rollSpeed = 6;
		[SerializeField] [MinMaxSlider(0, 10f)] [BoxGroup("Shield")]
		private Vector2 _distanceRange = new Vector2(0.02f, 0.3f);
		[SerializeField] [MinMaxSlider(0, 1f)] [BoxGroup("Shield")]
		private Vector2 _fresnelPowerRange = new Vector2(0, 0.5f);
		[SerializeField] [MinMaxSlider(0.001f, 0.01f)] [BoxGroup("Shield")]
		private Vector2 _displacementAmountRange = new Vector2(0.001f, 0.005f);

		public float RollSpeed => _rollSpeed;
		public (Vector2, Vector2, Vector2) GetShieldData => (_distanceRange, _fresnelPowerRange, _displacementAmountRange);
	}
}