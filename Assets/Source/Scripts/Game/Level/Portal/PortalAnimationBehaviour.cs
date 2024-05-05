using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace CubeProject.Game
{
	public class PortalAnimationBehaviour : SerializedMonoBehaviour
	{
		[OdinSerialize] [MinMaxSlider(0, 2)] private Vector2 _playingScaleRange;
		[SerializeField] private Ease _appearScaleEase;
		[SerializeField] private float _appearScaleDuration;
		[SerializeField] private float _playingScaleSpeed;
		[SerializeField] private float _durationTurnover;

		private Tweener _appearTweener;
		private List<Tweener> _playingTweeners = new List<Tweener>();

		private void Awake()
		{
			_appearTweener = PortalAnimationFactory.CreateAppear(transform, _appearScaleDuration, _appearScaleEase);
			_playingTweeners.Add(PortalAnimationFactory.CreateScale(transform, _playingScaleRange, _playingScaleSpeed));
			_playingTweeners.Add(PortalAnimationFactory.CreateRotation(transform, _durationTurnover));
		}

		private void OnDisable()
		{
			_appearTweener.Kill();
			_playingTweeners.ForEach(tweener => tweener.Kill());
		}
		
		public void Play()
		{
			ResetAnimation();

			_appearTweener.PlayForward();
			_playingTweeners.ForEach(tweener => tweener.PlayForward());
		}

		public void Stop()
		{
			_appearTweener.PlayBackwards();
			ResetAnimation();
		}

		private void ResetAnimation()
		{
			_appearTweener.Pause();
			_playingTweeners.ForEach(tweener => tweener.Pause());
		}
	}
}