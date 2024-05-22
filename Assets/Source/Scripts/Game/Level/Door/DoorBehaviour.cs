using System;
using DG.Tweening;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(ChargeConsumer))]
	public class DoorBehaviour : MonoBehaviour
	{
		[SerializeField] private Door[] _doors;
		[SerializeField] private float _animationTime;
		[SerializeField] private bool _change;

		private ChargeConsumer _chargeConsumer;
		private Tweener _scaleTweener;
		private Tweener _rotateTweener;
		private Tweener _moveTweener;

		private void OnValidate() =>
			ChangeState(_change);

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _chargeConsumer);

		private void OnEnable()
		{
			_chargeConsumer.ChargeChanged += OnChargeChanged;
			OnChargeChanged();
		}

		private void OnDisable()
		{
			_chargeConsumer.ChargeChanged -= OnChargeChanged;
			StopTweeners();
		}

		private void OnChargeChanged() =>
			ChangeState(_chargeConsumer.IsCharged);

		private void ChangeState(bool isCharged)
		{
			StopTweeners();

			Action<Door> execute = isCharged ? Open : Close;

			foreach (var door in _doors)
				execute(door);

			return;

			void Open(Door door)
			{
				_rotateTweener = door.Center.DOLocalRotate(door.OpenRotation, _animationTime)
					.OnKill(() =>
					{
						_moveTweener = door.transform.DOLocalMove(door.OpenPosition, _animationTime);
						_scaleTweener = door.transform.DOScale(door.OpenScale, _animationTime);
					});
			}

			void Close(Door door)
			{
				DOTween.Sequence()
					.Append(_moveTweener = door.transform.DOLocalMove(door.ClosePosition, _animationTime))
					.Join(_scaleTweener = door.transform.DOScale(door.CloseScale, _animationTime))
					.OnKill(() => _rotateTweener = door.Center.DOLocalRotate(door.CloseRotation, _animationTime));
			}
		}

		private void StopTweeners()
		{
			_moveTweener.Kill();
			_rotateTweener.Kill();
			_scaleTweener.Kill();
		}
	}
}