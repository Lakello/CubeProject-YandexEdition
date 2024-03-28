using System.Collections.Generic;
using LeadTools.NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CubeProject.LeadTools.Utils
{
	[ExecuteInEditMode]
	public class ObjectSpawnerEditor : MonoBehaviour
	{
		[SerializeField] private GameObject _prefab;
		[SerializeField] private Transform _startPoint;
		[SerializeField] private Transform _endPoint;

		private List<GameObject> _spawnedObjects;
		private bool _isSpawned;

		private bool CanShowSpawnButton => _prefab != null && _startPoint != null && _endPoint != null;  
		
		[Button] [ShowIf(nameof(CanShowSpawnButton))]
		private void Spawn()
		{
			Vector3 currentPosition = _startPoint.position;
			Vector3 direction = _endPoint.position - _startPoint.position;

			while (currentPosition != _endPoint.position)
			{
				InstanttiateObject();

				currentPosition += direction.normalized;
			}
			
			InstanttiateObject();

			_isSpawned = true;

			return;
			
			void InstanttiateObject()
			{
				var spawnerObject = (GameObject)PrefabUtility.InstantiatePrefab(_prefab, SceneManager.GetActiveScene());

				spawnerObject.transform.position = currentPosition;
				
				_spawnedObjects.Add(spawnerObject);
			}
		}

		[Button] [ShowIf(nameof(_isSpawned))]
		private void Back()
		{
			
		}
	}
}