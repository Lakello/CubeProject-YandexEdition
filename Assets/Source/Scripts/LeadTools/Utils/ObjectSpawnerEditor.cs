#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CubeProject.LeadTools.Utils
{
	[ExecuteInEditMode]
	public class ObjectSpawnerEditor : MonoBehaviour
	{
		private const string PointName = "Point_";

		private readonly List<Transform> _pointsPool = new List<Transform>();

		[SerializeField] [OnValueChanged(nameof(OnPresetChanged))] [ValueDropdown(nameof(GetPresets))] private ObjectSpawnerPreset _preset;
		[SerializeField] [OnValueChanged(nameof(OnPointsChanged))] private Transform[] _points;
		[SerializeField] private int _height = 1;

		private List<GameObject> _spawnedObjects;
		private bool _isSpawned;
		private GameObject _parent;
		private ObjectSpawnerPreset[] _presets;

		private bool CanShowApplyButton => _isSpawned;

		private bool CanShowSpawnButton => _preset.ObjectPrefab != null && _points is { Length: >= 2 } && _isSpawned == false;

		private void OnPresetChanged() =>
			_parent = GameObject.Find(_preset.ParentName);

		private void FindPresets()
		{
			_presets = Resources.LoadAll<ObjectSpawnerPreset>("ObjectSpawnerSO");
		}

		private ValueDropdownList<ObjectSpawnerPreset> GetPresets()
		{
			FindPresets();

			var list = new ValueDropdownList<ObjectSpawnerPreset>();

			foreach (var preset in _presets)
			{
				list.Add(preset.name, preset);
			}

			return list;
		}

		private void OnPointsChanged()
		{
			for (int i = 0; i < _points.Length; i++)
			{
				if (_points[i] == null || _points[i].name != PointName + i)
				{
					var newPoint = (GameObject)PrefabUtility.InstantiatePrefab(_preset.PointPrefab, SceneManager.GetActiveScene());

					newPoint.name = PointName + i;
					newPoint.transform.position = Vector3.zero;

					_points[i] = newPoint.transform;

					_pointsPool.Add(newPoint.transform);
				}
			}

			var points = _pointsPool.Except(_points).ToArray();

			if (points.Length < 1)
				return;

			for (int i = 0; i < points.Length; i++)
			{
				_pointsPool.Remove(points[i]);
				DestroyImmediate(points[i].gameObject);
			}
		}

		[Button] [ShowIf(nameof(CanShowApplyButton))]
		private void Apply()
		{
			_isSpawned = false;
			_spawnedObjects = null;
		}

		[Button] [ShowIf(nameof(CanShowSpawnButton))]
		private void Spawn()
		{
			_spawnedObjects = new List<GameObject>();
			Vector3 previousPosition = Vector3.positiveInfinity;

			for (int i = 0; i < _height; i++)
			{
				SetRow(i);
			}

			_isSpawned = true;

			return;

			void SetRow(int heightIncrement)
			{
				for (int i = 0; i < _points.Length - 1; i++)
				{
					Vector3 startPoint = _points[i].position;
					Vector3 endPoint = _points[i + 1].position;

					startPoint.y += heightIncrement;
					endPoint.y += heightIncrement;

					Vector3 currentPosition = startPoint;
					Vector3 direction = endPoint - startPoint;

					while (currentPosition != endPoint)
					{
						TryInstantiateObject(currentPosition);

						currentPosition += direction.normalized;
					}

					TryInstantiateObject(currentPosition);
				}
			}

			void TryInstantiateObject(Vector3 currentPosition)
			{
				if (currentPosition == previousPosition)
				{
					return;
				}

				previousPosition = currentPosition;

				var spawnerObject = (GameObject)PrefabUtility.InstantiatePrefab(_preset.ObjectPrefab, SceneManager.GetActiveScene());

				spawnerObject.transform.position = currentPosition;
				spawnerObject.transform.rotation = Quaternion.Euler(_preset.ObjectRotation);

				if (_parent != null)
				{
					spawnerObject.transform.SetParent(_parent.transform);
				}

				_spawnedObjects.Add(spawnerObject);
			}
		}

		[Button] [ShowIf(nameof(_isSpawned))]
		private void Back()
		{
			if (_spawnedObjects is { Count: > 0 })
			{
				for (int i = 0; i < _spawnedObjects.Count; i++)
				{
					DestroyImmediate(_spawnedObjects[i]);
				}
			}

			Apply();
		}
	}
}
#endif