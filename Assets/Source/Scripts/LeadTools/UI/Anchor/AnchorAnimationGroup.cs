using System;
using System.Linq;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;

namespace CubeProject.LeadTools.UI
{
	[Serializable]
	public class AnchorAnimationGroup
	{
		[SerializeField] private AnchorAnimator[] _animators;

		public void Play(AnchorAnimatorState state) =>
			_animators.ForEach(animator => animator.Play(state));

		public Sequence[] CreateAnimations(AnchorAnimatorState state) =>
			_animators.Select(animation => animation.CreateAnimation(state)).ToArray();
	}
}