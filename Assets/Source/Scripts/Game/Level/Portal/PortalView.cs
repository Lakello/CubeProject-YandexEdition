using System.Collections.Generic;
using LeadTools.Extensions;
using Sirenix.Utilities;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(ChargeConsumer))]
	public class PortalView : MonoBehaviour
	{
		private readonly Dictionary<MeshRenderer, PortalAnimationBehaviour> _animations = new Dictionary<MeshRenderer, PortalAnimationBehaviour>();

		[SerializeField] private MeshRenderer[] _meshRenderers;

		private ChargeConsumer _chargeConsumer;
		private Coroutine _updateViewCoroutine;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeConsumer);

			_meshRenderers.ForEach(meshRenderer => _animations.Add(
				meshRenderer,
				meshRenderer.gameObject.GetComponentElseThrow<PortalAnimationBehaviour>()));
		}

		private void OnEnable() =>
			_chargeConsumer.ChargeChanged += OnChargeChanged;

		private void OnDisable() =>
			_chargeConsumer.ChargeChanged -= OnChargeChanged;

		private void OnChargeChanged()
		{
			if (_chargeConsumer.IsCharged)
				_meshRenderers.ForEach(meshRenderer => _animations[meshRenderer].Play());
			else
				_meshRenderers.ForEach(meshRenderer => _animations[meshRenderer].Stop());
		}
	}
}