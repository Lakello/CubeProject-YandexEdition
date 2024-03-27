using System.Collections.Generic;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(ChargeConsumer))]
	public class PortalView : MonoBehaviour
	{
		private readonly Dictionary<MeshRenderer, PortalAnimation> _animations = new Dictionary<MeshRenderer, PortalAnimation>();

		[SerializeField] private MeshRenderer[] _meshRenderers;
		
		private ChargeConsumer _chargeConsumer;
		private Coroutine _updateViewCoroutine;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeConsumer);

			foreach (var renderer in _meshRenderers)
			{
				_animations.Add(
					renderer,
					renderer.gameObject.GetComponentElseThrow<PortalAnimation>());
			}
		}

		private void OnEnable() =>
			_chargeConsumer.ChargeChanged += OnChargeChanged;

		private void OnDisable() =>
			_chargeConsumer.ChargeChanged -= OnChargeChanged;

		private void OnChargeChanged()
		{
			if (_chargeConsumer.IsCharged)
			{
				foreach (var meshRenderer in _meshRenderers)
				{
					_animations[meshRenderer].Play();
				}
			}
			else
			{
				foreach (var meshRenderer in _meshRenderers)
				{
					_animations[meshRenderer].Stop();
				}
			}
		}
	}
}