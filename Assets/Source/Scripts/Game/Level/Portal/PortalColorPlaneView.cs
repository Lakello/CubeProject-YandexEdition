using CubeProject.SO;
using DG.Tweening;
using LeadTools.Extensions;
using LeadTools.Other;
using Reflex.Attributes;
using UnityEngine;

namespace Game.Player
{
	public class PortalColorPlaneView : MonoBehaviour
	{
		private ChargeConsumer _chargeConsumer;
		private MeshRenderer _selfMeshRenderer;
		private Sequence _animation;
		private bool _isInitialized;

		[Inject]
		private void Inject(PortalColorData data)
		{
			gameObject.GetComponentInParentElseThrow(out _chargeConsumer);
			gameObject.GetComponentElseThrow(out _selfMeshRenderer);

			Init(data);
		}

		private void OnEnable()
		{
			_chargeConsumer.ChargeChanged += OnChargeChanged;
			OnChargeChanged();
		}

		private void OnDisable() =>
			_chargeConsumer.ChargeChanged -= OnChargeChanged;

		private void OnDestroy() =>
			_animation?.Kill();

		private void Init(PortalColorData data)
		{
			var portal = gameObject.GetComponentInParentElseThrow<PortalBehaviour>();
			var linkedColorPlane = portal.LinkedPortal.gameObject.GetComponentInChildrenElseThrow<PortalColorPlaneView>();
			var linkedMeshRenderer = linkedColorPlane.gameObject.GetComponentElseThrow<MeshRenderer>();

			linkedColorPlane.enabled = false;
			DestroyImmediate(linkedColorPlane);

			if (_isInitialized)
				return;

			var color = data.GetColor().CalculateIntensityColor(data.Intensity);
			_selfMeshRenderer.material.SetColor(ShaderProperty._Color.GetCurrentName(), color);
			linkedMeshRenderer.material.SetColor(ShaderProperty._Color.GetCurrentName(), color);

			_animation = DOTween
				.Sequence()
				.Append(_selfMeshRenderer.material.DOFloat(
					data.DissolveAmountShow,
					ShaderProperty._DissolveAmount.GetCurrentName(),
					data.DurationAnimation))
				.Join(linkedMeshRenderer.material.DOFloat(
					data.DissolveAmountShow,
					ShaderProperty._DissolveAmount.GetCurrentName(),
					data.DurationAnimation))
				.SetAutoKill(false)
				.Pause();

			_isInitialized = true;
		}

		private void OnChargeChanged()
		{
			if (_chargeConsumer.IsCharged)
				_animation.PlayForward();
			else
				_animation.PlayBackwards();
		}
	}
}