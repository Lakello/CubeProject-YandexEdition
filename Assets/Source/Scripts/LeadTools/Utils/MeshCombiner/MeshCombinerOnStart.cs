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

			combiner.CreateMultiMaterialMesh = _data.CreateMultiMaterialMesh;
			combiner.CombineInactiveChildren = _data.CombineInactiveChildren;
			combiner.DeactivateCombinedChildren = _data.DeactivateCombinedChildren;
			combiner.DeactivateCombinedChildrenMeshRenderers = _data.DeactivateCombinedChildrenMeshRenderers;
			combiner.GenerateUVMap = _data.GenerateUVMap;
			combiner.DestroyCombinedChildren = _data.DestroyCombinedChildren;
			
			combiner.CombineMeshes(_showCreatedMeshInfo);
		}
	}
}