using CubeProject.SO;
using LeadTools.Extensions;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Game
{
	public class PortalColorPlane : MonoBehaviour
	{
		private const string ColorProperty = "_Color";

		private ChargeConsumer _chargeConsumer;
		private MeshRenderer _selfMeshRenderer;
		private Color _activeColor;
		private Color _inactiveColor;
		private bool _isInitialized;

		[Inject]
		private void Inject(PortalColorData data) =>
			SyncColor(data);

		private void Awake()
		{
			gameObject.GetComponentInParentElseThrow(out _chargeConsumer);
			gameObject.GetComponentElseThrow(out _selfMeshRenderer);
		}

		private void OnEnable()
		{
			_chargeConsumer.ChargeChanged += OnChargeChanged;
			OnChargeChanged();
		}

		private void OnDisable() =>
			_chargeConsumer.ChargeChanged -= OnChargeChanged;

		private void Init(Color activeColor, Color inactiveColor)
		{
			if (_isInitialized)
				return;

			_activeColor = activeColor;

			_inactiveColor = inactiveColor;

			_isInitialized = true;
		}

		private void SyncColor(PortalColorData data)
		{
			var portal = gameObject.GetComponentInParentElseThrow<PortalBehaviour>();
			var linkedColorPlane = portal.LinkedPortal.gameObject.GetComponentInChildrenElseThrow<PortalColorPlane>();

			if (linkedColorPlane._isInitialized)
				return;

			var color = data.GetColor();

			color.a = data.ActiveAlpha;
			var activeColor = color;

			color.a = data.InactiveAlpha;
			var inactiveColor = color;

			Init(activeColor, inactiveColor);
			linkedColorPlane.Init(activeColor, inactiveColor);
		}

		private void OnChargeChanged()
		{
			if (_chargeConsumer.IsCharged)
				_selfMeshRenderer.material.SetColor(ColorProperty, _activeColor);
			else
				_selfMeshRenderer.material.SetColor(ColorProperty, _inactiveColor);
		}
	}
}