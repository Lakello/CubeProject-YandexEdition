using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	public class DoorHandler : ChargeConsumer
	{
		[SerializeField] private Door[] _doors;
		[SerializeField] private float _animationTime;

		private Coroutine _doorCoroutine;
		private bool _previousCharged;

		protected override void OnEnable()
		{
			base.OnEnable();
			ChargeChanged += OnChargeChanged;
			OnChargeChanged();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			ChargeChanged -= OnChargeChanged;
		}

		private void OnChargeChanged()
		{
			var currentCharged = IsCharged;

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
			foreach (var door in _doors)
			{
				door.StartScaleZ = door.transform.localScale.z;
				door.StartPositionZ = door.transform.localPosition.z;
			}

			_doorCoroutine = this.PlaySmoothChangeValue(
				(currentTime) =>
				{
					foreach (var door in _doors)
					{
						var doorTransform = door.transform;

						var (positionZ, scaleZ) = CalculateDoorData(door, currentTime);

						var position = doorTransform.localPosition;
						var scale = doorTransform.localScale;

						position.z = positionZ;
						scale.z = scaleZ;

						doorTransform.localPosition = position;
						doorTransform.localScale = scale;
					}
				},
				_animationTime);

			return;

			(float, float) CalculateDoorData(Door door, float currentTime)
			{
				var (targetPosition, targetScale) = isCharged
					? (door.OpenPositionZ, door.OpenScaleZ)
					: (door.ClosePositionZ, door.CloseScaleZ);

				var position = Mathf.Lerp(door.StartPositionZ, targetPosition, currentTime);
				var scale = Mathf.Lerp(door.StartScaleZ, targetScale, currentTime);

				return (position, scale);
			}
		}
	}
}