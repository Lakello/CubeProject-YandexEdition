using System.Collections;
using CubeProject.Player;
using LeadTools.Extensions;
using LeadTools.Object;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Effects
{
	public class ElectricHandler : MonoBehaviour
	{
		[SerializeField] private ElectricEffect _electricEffectPrefab;
		[SerializeField] private float _startRadius = 1f;
		[SerializeField] private float _endRadius = 3f;
		[SerializeField] [Range(0.1f, 10f)] private float _cooldown = 1f;
		[SerializeField] [Range(0.1f, 10f)] private float _lifeTime = 0.3f;
		
		private Cube _cube;
		private CubeStateHandler _stateHandler;
		private Coroutine _updateEffectCoroutine;
		private ObjectSpawner<ElectricEffect, ElectricInit> _electricSpawner;

		[Inject]
		private void Inject(Cube cube)
		{
			_cube = cube;
			_stateHandler = _cube.ComponentsHolder.StateHandler;
			_stateHandler.StateChanged += OnStateChanged;
			
			OnStateChanged(_stateHandler.CurrentState);
		}

		private void Awake() =>
			_electricSpawner = new ObjectSpawner<ElectricEffect, ElectricInit>(gameObject.transform);

		private void OnEnable()
		{
			if (_stateHandler != null)
			{
				_stateHandler.StateChanged += OnStateChanged;
			}
		}

		private void OnDisable()
		{
			if (_stateHandler != null)
			{
				_stateHandler.StateChanged -= OnStateChanged;
			}
		}

		private void OnStateChanged(CubeState state)
		{
			if (state == CubeState.Normal)
			{
				_updateEffectCoroutine = StartCoroutine(UpdateEffect());
			}
			else
			{
				this.StopRoutine(_updateEffectCoroutine);
			}
		}

		private IEnumerator UpdateEffect()
		{
			while (enabled)
			{
				yield return new WaitForSeconds(_cooldown);

				var init = new ElectricInit(_cube, _startRadius, _endRadius, _lifeTime);

				_electricSpawner.Spawn(_electricEffectPrefab, init);
			}
		}
	}
}