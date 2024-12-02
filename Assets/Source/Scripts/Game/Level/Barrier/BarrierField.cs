using System;
using CubeProject.Game.AudioSystem;
using CubeProject.Game.Level.Charge;
using CubeProject.Game.Player;
using LeadTools.Common;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game.Level.Barrier
{
	public class BarrierField : MonoBehaviour, IAudioSubject
	{
		private const ShaderProperty Property = ShaderProperty._Tiling;

		[SerializeField] private ChargeConsumer _chargeConsumer;

		public event Action AudioPlaying;

		private void Awake()
		{
			var meshRenderer = gameObject.GetComponentElseThrow<MeshRenderer>();

			var propertyName = Enum.GetName(typeof(ShaderProperty), Property);

			var tiling = meshRenderer.material.GetVector(propertyName);

			tiling.x = transform.localScale.z;

			meshRenderer.material.SetVector(propertyName, tiling);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out CubeEntity cube)
				&& _chargeConsumer.IsCharged
				&& cube.Component.ChargeHolder.IsCharged is false)
			{
				AudioPlaying?.Invoke();
				cube.Kill();
			}
		}
	}
}