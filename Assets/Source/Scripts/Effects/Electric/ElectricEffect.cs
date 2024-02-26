using System;
using System.Collections;
using LeadTools.Extensions;
using LeadTools.Object;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace CubeProject.Effects
{
	public class ElectricEffect : MonoBehaviour, IPoolingObject<ElectricEffect, ElectricInit>
	{
		[SerializeField] private VisualEffect _visualEffect;
		[SerializeField] private SupportPoint _startPoint;
		[SerializeField] private SupportPoint _endPoint;
		
		private Func<Vector3> _getStartPosition;
		private ElectricInit _init;
		private Coroutine _lifeTimeCoroutine;
		
		public event Action<IPoolingObject<ElectricEffect, ElectricInit>> Disabled;

		public Type SelfType => typeof(ElectricEffect);

		public ElectricEffect Instance => this;

		public void Init(ElectricInit init)
		{
			_init = init;
			
			UpdateStartPosition();
			_endPoint.transform.position = GetEndPosition();
			_lifeTimeCoroutine = StartCoroutine(LifeTime());
		}
		
		private void OnDisable() =>
			StopCoroutine(_lifeTimeCoroutine);

		private void Update()
		{
			_visualEffect.transform.position = _init.Cube.transform.position;
		
			_startPoint.transform.position = _getStartPosition();
			
			_startPoint.transform.RotateToTarget(_endPoint.transform);
			_endPoint.transform.RotateToTarget(_startPoint.transform);
		}
		
		private void UpdateStartPosition()
		{
			var position = Random.insideUnitSphere;
			position.y = Mathf.Abs(position.y);
		
			position *= _init.StartRadius;
		
			_getStartPosition = () => position + _init.Cube.transform.position;
		}
		
		private Vector3 GetEndPosition()
		{
			var position = Random.insideUnitSphere;
			position.y = 0;
		
			position *= _init.EndRadius;
		
			position += _init.Cube.transform.position;
		
			return position;
		}

		private IEnumerator LifeTime()
		{
			yield return new WaitForSeconds(_init.LifeTime);
			
			Disabled?.Invoke(this);
		}
	}
}