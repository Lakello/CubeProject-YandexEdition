using LeadTools.Utils;
using UnityEngine;

namespace CubeProject.LeadTools.Utils
{
	public class MeshCombinerOnStart : MonoBehaviour
	{
		[SerializeField] private MeshCombinerData _data;
		[SerializeField] private bool _showCreatedMeshInfo;
		
		private void Start()
		{
			MeshCombiner combiner = gameObject.AddComponent<MeshCombiner>();
			
			combiner.Init(_data);
			combiner.CombineMeshes(_showCreatedMeshInfo);
		}
	}
}