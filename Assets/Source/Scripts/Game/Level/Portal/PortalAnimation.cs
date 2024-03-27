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

		private Tweener _stateChangedScaleTweener;
		private Tweener _scaleTweener;
		private Tweener _rotateTweener; 
		
		public void Play()
		{
			ResetAnimation();
			
			var endValueScalePlaying = Vector3.one * _scalePlayingCurve.Evaluate(1);
			var endValueRotate = Vector3.up * 360;
			var endValueScale = Vector3.one * _scaleCurve.Evaluate(1);

			_stateChangedScaleTweener = transform
				.DOScale(endValueScalePlaying, _scalePlayingDuration)
				.SetEase(_scalePlayingCurve)
				.OnKill(() => _scaleTweener = transform.DOScale(endValueScale, _scaleSpeed).SetEase(_scaleCurve).SetLoops(-1, LoopType.Restart));
			
			_rotateTweener = transform.DORotate(endValueRotate, _rotateSpeed).SetEase(_rotateCurve).SetLoops(-1, LoopType.Incremental);
		}

		public void Stop()
		{
			ResetAnimation();

			_stateChangedScaleTweener = transform
				.DOScale(Vector3.zero, _scaleStoppingDuration)
				.SetEase(_scaleStoppingCurve);
		}

		private void ResetAnimation()
		{
			_stateChangedScaleTweener.Kill();
			_scaleTweener.Kill();
			_rotateTweener.Kill();

			//transform.rotation = Quaternion.Euler(Vector3.zero);
		}
	}
}