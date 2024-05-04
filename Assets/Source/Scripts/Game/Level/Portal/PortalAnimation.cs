using DG.Tweening;
using LeadTools.Extensions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.Mathematics;
using UnityEngine;

namespace CubeProject.Game
{
	public class PortalAnimation : SerializedMonoBehaviour
	{
		[OdinSerialize] [MinMaxSlider(0, 2)] private Vector2 _playingScaleRange;
		[SerializeField] private Ease _appearScaleEase;
		[SerializeField] private float _appearScaleDuration;
		[SerializeField] private float _playingScaleSpeed;
		[SerializeField] private float _perDuration;

		private Tweener _appearTweener;
		private Sequence _playingSequence;

		private void Awake()
		{
			Vector3 newScale = new Vector3();
			Vector3 newPlayingScale = new Vector3();
			Vector3 newRotation = new Vector3();

			_appearTweener = DOTween
				.To(
					progress =>
					{
						newScale = newScale.SetAll(progress);
						transform.localScale = newScale;
					},
					0,
					1,
					_appearScaleDuration)
				.SetEase(_appearScaleEase)
				.SetAutoKill(false)
				.Pause();

			_playingSequence = DOTween
				.Sequence()
				.SetAutoKill(false)
				.Append(DOTween
					.To(progress =>
						{
							newPlayingScale = newPlayingScale.SetAll(progress);
							transform.localScale = newPlayingScale;
						},
						_playingScaleRange.x,
						_playingScaleRange.y,
						_playingScaleSpeed)
					.SetLoops(-1, LoopType.Yoyo))
				.Join(DOTween
					.To(progress =>
						{
							newRotation.y = progress;
							transform.localRotation = quaternion.Euler(newRotation);
						},
						0,
						360,
						_perDuration)
					.SetLoops(-1, LoopType.Incremental))
				.Pause();
		}

		private void OnDisable()
		{
			_appearTweener.Kill();
			_playingSequence.Kill();
		}

		public void Play()
		{
			ResetAnimation();

			_appearTweener.PlayForward();
			_playingSequence.PlayForward();
		}

		public void Stop()
		{
			_appearTweener.PlayBackwards();
			ResetAnimation();
		}

		private void ResetAnimation()
		{
			_appearTweener.Pause();
			_playingSequence.Pause();
		}
	}
}