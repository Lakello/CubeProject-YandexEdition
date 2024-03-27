using DG.Tweening;
using UnityEngine;

namespace CubeProject.Game
{
	public class PortalAnimation : MonoBehaviour
	{
		[SerializeField] private Vector3 _playingScale;
		[SerializeField] private Vector3 _stoppingScale;
		[SerializeField] private AnimationCurve _scaleCurve;
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
			
			var endValueRotate = Vector3.up * 360;
			var endValueScale = Vector3.one * _scaleCurve.Evaluate(1);

			_stateChangedScaleTweener = transform
				.DOScale(_playingScale, _scalePlayingDuration)
				.SetEase(Ease.InOutFlash)
				.OnKill(() =>
				{
					if (_scaleTweener == null)
					{
						_scaleTweener = transform.DOScale(endValueScale, _scaleSpeed).SetEase(_scaleCurve).SetLoops(-1, LoopType.Restart);
					}
					else
					{
						_scaleTweener.Play();
					}
				});
			
			if (_rotateTweener == null)
			{
				_rotateTweener = transform.DORotate(endValueRotate, _rotateSpeed).SetLoops(-1, LoopType.Incremental);
			}
			else
			{
				_rotateTweener.Play();
			}
		}

		public void Stop()
		{
			ResetAnimation();

			_stateChangedScaleTweener = transform
				.DOScale(_stoppingScale, _scaleStoppingDuration);
		}

		private void ResetAnimation()
		{
			_stateChangedScaleTweener.Kill();
			_stateChangedScaleTweener = null;
			
			_scaleTweener.Pause();
			_rotateTweener.Pause();
		}
	}
}