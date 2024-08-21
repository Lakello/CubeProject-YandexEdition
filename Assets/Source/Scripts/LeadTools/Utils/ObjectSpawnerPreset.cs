using UnityEngine;

namespace LeadTools.Utils
{
	[CreateAssetMenu(fileName = "preset", menuName = "Utility/new preset")]
	public class ObjectSpawnerPreset : ScriptableObject
	{
		[SerializeField] private GameObject _objectPrefab;
		[SerializeField] private Vector3 _objectRotation;
		[SerializeField] private string _parentName;
		[SerializeField] private GameObject _pointPrefab;

		public string ParentName => _parentName;
		public GameObject ObjectPrefab => _objectPrefab;
		public Vector3 ObjectRotation => _objectRotation;
		public GameObject PointPrefab => _pointPrefab;
	}
}