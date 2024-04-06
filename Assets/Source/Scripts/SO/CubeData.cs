using Sirenix.OdinInspector;
using UnityEngine;

namespace CubeProject.SO
{
	[CreateAssetMenu(menuName = "Cube/Data")]
	public class CubeData : ScriptableObject
	{
		[ShowPropertyResolver] public float RollSpeed { get; }

	}
}