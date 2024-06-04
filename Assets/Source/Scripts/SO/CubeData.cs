using Sirenix.OdinInspector;
using Source.Scripts.Game;
using Source.Scripts.Game.Level.Shield;
using UnityEngine;

namespace CubeProject.SO
{
	[CreateAssetMenu(menuName = "Cube/Data")]
	public class CubeData : ScriptableObject
	{
		[SerializeField] [BoxGroup("Base")]
		private Player _playerPrefab;
		[SerializeField] [BoxGroup("Base")]
		private float _rollSpeed = 6;

		[SerializeField] [BoxGroup("Shield")]
		private ShieldData _shieldData;

		public Player PlayerPrefab => _playerPrefab;
		public float RollSpeed => _rollSpeed;
		public ShieldData ShieldData => _shieldData;
	}
}