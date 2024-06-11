using Sirenix.OdinInspector;
using CubeProject.Game.Player;
using CubeProject.Game.Player.Shield;
using UnityEngine;

namespace CubeProject.SO
{
	[CreateAssetMenu(menuName = "Cube/Data")]
	public class CubeData : ScriptableObject
	{
		[SerializeField] [BoxGroup("Base")]
		private PlayerEntity _playerEntityPrefab;
		[SerializeField] [BoxGroup("Base")]
		private float _rollSpeed = 6;

		[SerializeField] [BoxGroup("Shield")]
		private ShieldData _shieldData;

		public PlayerEntity PlayerEntityPrefab => _playerEntityPrefab;
		public float RollSpeed => _rollSpeed;
		public ShieldData ShieldData => _shieldData;
	}
}