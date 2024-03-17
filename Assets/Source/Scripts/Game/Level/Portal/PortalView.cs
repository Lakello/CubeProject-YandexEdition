using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LeadTools.Extensions;
using LeadTools.NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CubeProject.Game
{
	[RequireComponent(typeof(ChargeConsumer))]
	public class PortalView : MonoBehaviour
	{
		private const string ColorProperty = "_EmissionColor";

		[SerializeField] private AnimationCurve _lightIntensityCurve;
		[SerializeField] private AnimationCurve _scaleCurve;
		[SerializeField] [MinMaxSlider(-5f, 5f)] private Vector2 _scaleSpeedRange;
		[SerializeField] [MinMaxSlider(-50f, 50f)] private Vector2 _rotateSpeedRange;
		[SerializeField] [MinMaxSlider(-5f, 5f)] private Vector2 _lightPulseSpeedRange;
		[SerializeField] private GameObject[] _viewObjects;

		private MeshRenderer[] _meshRenderers;
		private Dictionary<MeshRenderer, PortalViewData> _viewDataDictionary;
		private ChargeConsumer _chargeConsumer;
		private Coroutine _updateViewCoroutine;

		private void OnValidate()
		{
			if (_viewObjects is null)
			{
				return;
			}

			if (_viewObjects.Length == 0)
			{
				return;
			}

			for (int i = 0; i < _viewObjects.Length; i++)
			{
				if (_viewObjects[i].TryGetComponent(out MeshRenderer _) is false)
				{
					_viewObjects[i] = null;

					Debug.LogError($"Required {nameof(MeshRenderer)}", gameObject);
				}
			}
		}

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeConsumer);

			_meshRenderers =
				(from view in _viewObjects
				select new
				{
					renderer = view.GetComponent<MeshRenderer>(),
				})
				.ToArray()
				.Select(view => view.renderer)
				.ToArray();

			_viewDataDictionary = new Dictionary<MeshRenderer, PortalViewData>();

			foreach (var renderer in _meshRenderers)
			{
				_viewDataDictionary.Add(
					renderer, 
					new PortalViewData(renderer.material.GetColor(ColorProperty)));
			}
		}

		private void Start() =>
			OnChargeChanged();

		private void OnEnable() =>
			_chargeConsumer.ChargeChanged += OnChargeChanged;

		private void OnDisable() =>
			_chargeConsumer.ChargeChanged -= OnChargeChanged;

		private void OnChargeChanged()
		{
			if (_chargeConsumer.IsCharged is false)
			{
				return;
			}

			this.StopRoutine(_updateViewCoroutine);

			UpdateSpeeds();
			
			_updateViewCoroutine = StartCoroutine(UpdateView());
		}

		private void UpdateSpeeds()
		{
			foreach (var renderer in _meshRenderers)
			{
				var scaleSpeed = Random.Range(_scaleSpeedRange.x, _scaleSpeedRange.y);
				var rotateSpeed = Random.Range(_rotateSpeedRange.x, _rotateSpeedRange.y);
				var lightPulseSpeed = Random.Range(_lightPulseSpeedRange.x, _lightPulseSpeedRange.y);
				
				_viewDataDictionary[renderer].UpdateSpeeds(rotateSpeed, scaleSpeed, lightPulseSpeed);
			}
		}
		
		private IEnumerator UpdateView()
		{
			while (_chargeConsumer.IsCharged)
			{
				foreach (var renderer in _meshRenderers)
				{
					_viewDataDictionary[renderer].UpdateProgress();
				}
				
				SetRotate();
				SetScale();
				SetLight();

				yield return null;
			}
		}

		private void SetRotate()
		{
			foreach (var meshRenderer in _meshRenderers)
			{
				meshRenderer.transform.Rotate(Vector3.up, _viewDataDictionary[meshRenderer].RotateSpeed * Time.deltaTime);
			}
		}

		private void SetScale()
		{
			foreach (var meshRenderer in _meshRenderers)
			{
				var scaleValue = GetCurveValue(
					_viewDataDictionary[meshRenderer].ScaleProgress,
					_scaleCurve);
				
				meshRenderer.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
			}
		}

		private void SetLight()
		{
			foreach (var meshRenderer in _meshRenderers)
			{
				var intensity = GetCurveValue(
					_viewDataDictionary[meshRenderer].LightPulseProgress,
					_lightIntensityCurve);
				
				var color = _viewDataDictionary[meshRenderer].Color;

				meshRenderer.material.SetColor(ColorProperty, color.CalculateIntensityColor(intensity));
			}
		}

		private float GetCurveValue(float progress, AnimationCurve curve) =>
			curve.Evaluate(progress);
	}
}