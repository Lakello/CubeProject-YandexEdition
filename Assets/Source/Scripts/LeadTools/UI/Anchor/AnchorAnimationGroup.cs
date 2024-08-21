using System.Linq;
using DG.Tweening;
using Sirenix.Utilities;

namespace LeadTools.UI.Anchor
{
	public class AnchorAnimationGroup
	{
		private readonly AnchorAnimator[] _animators;

		public AnchorAnimationGroup(AnchorAnimator[] animators)
		{
			_animators = animators;
			_animators.ForEach(animator => animator.Init());
		}

		public void Play(AnchorAnimatorState state) =>
			_animators.ForEach(animator => animator.Play(state));

		public Sequence[] CreateAnimations(AnchorAnimatorState state) =>
			_animators.Select(animation => animation.CreateAnimation(state)).ToArray();
	}
}