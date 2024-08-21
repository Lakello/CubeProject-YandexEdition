using UnityEngine;

namespace LeadTools.Utils.MeshCombineSystem
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
			combiner.IsDeactivateCombinedChildrenMeshRenderers = _data.DeactivateCombinedChildrenMeshRenderers;
			combiner.IsGenerateUVMap = _data.GenerateUVMap;
			combiner.IsDestroyCombinedChildren = _data.DestroyCombinedChildren;

			combiner.CombineMeshes(_showCreatedMeshInfo);
		}
	}
}