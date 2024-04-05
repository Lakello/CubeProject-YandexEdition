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

		[Inject]
		private void Inject(PortalColorData data)
		{
			var color = data.GetColor();
			
			color.a = data.ActiveAlpha;
			_activeColor = color;

			color.a = data.InactiveAlpha;
			_inactiveColor = color;
		}

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

		private void OnDisable()
		{
			_chargeConsumer.ChargeChanged -= OnChargeChanged;
		}

		private void OnChargeChanged()
		{
			if (_chargeConsumer.IsCharged)
			{
				_selfMeshRenderer.material.SetColor(ColorProperty, _activeColor);
			}
			else
			{
				_selfMeshRenderer.material.SetColor(ColorProperty, _inactiveColor);
			}
		}
	}
}