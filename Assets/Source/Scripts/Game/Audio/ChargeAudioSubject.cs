using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace CubeProject.Game.Player
{
	public class ChargeAudioSubject : SerializedMonoBehaviour, IAudioSubject
	{
		[SerializeField] private bool _targetCharged;
		[OdinSerialize] private IChargeable _chargeable;

		public event Action AudioPlaying;

		private void OnEnable() =>
			_chargeable.ChargeChanged += OnChargeChanged;

		private void OnDisable() =>
			_chargeable.ChargeChanged -= OnChargeChanged;

		private void OnChargeChanged()
		{
			if (_chargeable.IsCharged == _targetCharged)
				AudioPlaying?.Invoke();
		}
	}
}