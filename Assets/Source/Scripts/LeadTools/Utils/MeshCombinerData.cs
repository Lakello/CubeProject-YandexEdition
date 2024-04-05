using System;

namespace LeadTools.Utils
{
	[Serializable]
	public class MeshCombinerData
	{
		public bool CreateMultiMaterialMesh = false;
		public bool CombineInactiveChildren = false;
		public bool DeactivateCombinedChildren = true;
		public bool DeactivateCombinedChildrenMeshRenderers = false;
		public bool GenerateUVMap = false;
		public bool DestroyCombinedChildren = false;
	}
}