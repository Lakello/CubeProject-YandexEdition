using DG.Tweening;
using UnityEngine;

namespace CubeProject.Game
{
	public class PortalAnimation : MonoBehaviour
	{
		[SerializeField] private AnimationCurve _scalePlayingCurve;
		[SerializeField] private AnimationCurve _scaleStoppingCurve;
		[SerializeField] private AnimationCurve _scaleCurve;
		[SerializeField] private AnimationCurve _rotateCurve;
		[SerializeField] private float _scalePlayingDuration;
		[SerializeField] private float _scaleStoppingDuration;
		[SerializeField] private float _scaleSpeed;
		[SerializeField] private float _rotateSpeed;

		private Tweener _scaleTweener;
		private Tweener _rotateTweener; 
		
		public void Play()
		{
			StopTweeners();
			
			var endValueScalePlaying = Vector3.one * _scalePlayingCurve.Evaluate(1);
			var endValueRotate = Vector3.one * 360;
			var endValueScale = Vector3.one * _scaleCurve.Evaluate(1);
			
			_scaleTweener = transform
				.DOScale(endValueScalePlaying, _scalePlayingDuration)
				.SetEase(_scalePlayingCurve)
				.OnKill(() =>
				{
					DOTween.Sequence()
						.Append(_rotateTweener = transform.DORotate(endValueRotate, _rotateSpeed).SetEase(_rotateCurve).SetLoops(-1, LoopType.Restart))
						.Join(_scaleTweener = transform.DOScale(endValueScale, _scaleSpeed).SetEase(_scaleCurve).SetLoops(-1, LoopType.Restart));
				});
		}

		public void Stop()
		{
			StopTweeners();
			
			
		}

		private void StopTweeners()
		{
			_scaleTweener.Kill();
			_rotateTweener.Kill();
		}
	}
}