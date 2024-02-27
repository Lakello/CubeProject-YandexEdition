using System;
using System.Collections;
using UnityEngine;

namespace CubeProject.Player.Movement
{
	public class RollMover
	{
		private readonly AudioSource _audioSource;

		private float _currentAngle;

		public RollMover(AudioSource source) =>
			_audioSource = source;

		public IEnumerator Move(Transform origin, Vector3 direction, Func<float> getSpeed, Action endCallback = null)
		{
			const float rollAngle = 90f;

			if (_currentAngle >= rollAngle)
			{
				_currentAngle = 0f;
			}

			var (anchor, axis) = GetRotateData(origin, direction);

			while (_currentAngle < rollAngle)
			{
				var angle = getSpeed();

				if ((_currentAngle + angle) > rollAngle)
				{
					angle = rollAngle - _currentAngle;
					_currentAngle = rollAngle;
				}
				else
				{
					_currentAngle += angle;
				}

				origin.RotateAround(anchor, axis, angle);

				yield return new WaitForFixedUpdate();
			}

			if (_currentAngle is < rollAngle or > rollAngle)
			{
				origin.RotateAround(anchor, axis, rollAngle - _currentAngle);
			}

			_audioSource.Play();

			endCallback?.Invoke();
		}

		private (Vector3, Vector3) GetRotateData(Transform origin, Vector3 direction)
		{
			var anchor = origin.position + (Vector3.down + direction) * 0.5f;
			var axis = Vector3.Cross(Vector3.up, direction);

			return (anchor, axis);
		}
	}
}