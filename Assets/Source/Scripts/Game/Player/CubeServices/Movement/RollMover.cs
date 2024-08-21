using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CubeProject.Game.Player.CubeService.Movement
{
	public class RollMover
	{
		private readonly float _rollSpeed;
		private readonly Transform _targetTransform;

		private float _currentAngle;

		public RollMover(float rollSpeed, Transform targetTransform)
		{
			_rollSpeed = rollSpeed;
			_targetTransform = targetTransform;
		}

		public async UniTask Move(Vector3 direction, CancellationToken cancellationToken)
		{
			const float rollAngle = 90f;

			if (_currentAngle >= rollAngle)
				_currentAngle = 0f;

			var (anchor, axis) = GetRotateData(_targetTransform, direction);

			while (_currentAngle < rollAngle)
			{
				var angle = _rollSpeed;

				if ((_currentAngle + angle) > rollAngle)
				{
					angle = rollAngle - _currentAngle;
					_currentAngle = rollAngle;
				}
				else
				{
					_currentAngle += angle;
				}

				_targetTransform.RotateAround(anchor, axis, angle);

				await UniTask.WaitForFixedUpdate(cancellationToken);
			}

			if (_currentAngle is < rollAngle or > rollAngle)
				_targetTransform.RotateAround(anchor, axis, rollAngle - _currentAngle);
		}

		private (Vector3, Vector3) GetRotateData(Transform origin, Vector3 direction)
		{
			var anchor = origin.position + (Vector3.down + direction) * 0.5f;
			var axis = Vector3.Cross(Vector3.up, direction);

			return (anchor, axis);
		}
	}
}