using System;
using CubeProject.Player;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Game
{
	public class CheckPoint : MonoBehaviour, IChargeable
	{
		[SerializeField] private bool _isStartActive;

		private CheckPointHolder _holder;

		public event Action ChargeChanged;

		public bool IsCharged { get; private set; }

		[Inject]
		private void Inject(CheckPointHolder holder) =>
			_holder = holder;

		private void Start() =>
			ChangeActive(_isStartActive);

		private void OnTriggerEnter(Collider other)
		{
			if (IsCharged)
			{
				return;
			}

			if (other.TryGetComponent(out Cube _))
			{
				Active();
			}
		}

		public void DeActive() =>
			ChangeActive(false);

		private void Active() =>
			ChangeActive(true);

		private void ChangeActive(bool value)
		{
			IsCharged = value;
			ChargeChanged?.Invoke();

			if (IsCharged)
			{
				_holder.OnActiveChanged(this);
			}
		}
	}
}