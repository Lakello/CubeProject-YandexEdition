using System;
using DG.Tweening;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(ChargeConsumer))]
	public class DoorHandler : MonoBehaviour
	{
		[SerializeField] private Door[] _doors;
		[SerializeField] private float _animationTime;
		[SerializeField] private bool _change;

		private Coroutine _doorCoroutine;
		private bool _previousCharged;
		private ChargeConsumer _chargeConsumer;

		private void OnValidate()
		{
			ChangeState(_change);
		}

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeConsumer);
		}

		private void OnEnable()
		{
			_chargeConsumer.ChargeChanged += OnChargeChanged;
			OnChargeChanged();
		}

		private void OnDisable() =>
			_chargeConsumer.ChargeChanged -= OnChargeChanged;

		private void OnChargeChanged()
		{
			var currentCharged = _chargeConsumer.IsCharged;

			if (_previousCharged == currentCharged)
			{
				return;
			}

			_previousCharged = currentCharged;

			this.StopRoutine(_doorCoroutine);

			ChangeState(currentCharged);
		}

		private void ChangeState(bool isCharged)
		{
			Action<Door> execute = isCharged ? Open : Close;
			
			foreach (var door in _doors)
			{
				execute(door);
			}
			
			return;

			void Open(Door door)
			{
				door.Center.DOLocalRotate(door.OpenRotation, _animationTime)
					.OnKill(() =>
					{
						door.transform.DOLocalMove(door.OpenPosition, _animationTime);
						door.transform.DOScale(door.OpenScale, _animationTime);
					});
			}

			void Close(Door door)
			{
				DOTween.Sequence()
					.Append(door.transform.DOLocalMove(door.ClosePosition, _animationTime))
					.Join(door.transform.DOScale(door.CloseScale, _animationTime))
					.OnKill(() => door.Center.DOLocalRotate(door.CloseRotation, _animationTime));
			}
		}
	}
}