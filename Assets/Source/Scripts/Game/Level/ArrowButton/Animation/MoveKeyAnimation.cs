using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Tips
{
	public class MoveKeyAnimation
	{
		private readonly MonoBehaviour _context;

		private Coroutine _animationCoroutine;

		public MoveKeyAnimation(MonoBehaviour context) =>
			_context = context;

		public void Play(MoveKeyAnimationSettings settings)
		{
			Stop();

			float y;
			var startPosition = settings.KeyTransform.position;
			var targetPosition = startPosition;

			_animationCoroutine = _context.PlaySmoothChangeValue(
				(currentTime) =>
				{
					y = Mathf.Lerp(startPosition.y, settings.TargetPositionY, currentTime);
					targetPosition.y = y;

					settings.KeyTransform.position = targetPosition;
				},
				settings.AnimationTime,
				() => _animationCoroutine = null);
		}

		public void Stop() =>
			_context.StopRoutine(_animationCoroutine);
	}
}