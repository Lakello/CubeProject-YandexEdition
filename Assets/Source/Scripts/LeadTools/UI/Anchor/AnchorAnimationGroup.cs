using System;
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
	}
}