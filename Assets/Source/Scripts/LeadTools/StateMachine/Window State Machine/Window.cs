using System;
using CubeProject.LeadTools.UI;
using UnityEngine;

namespace LeadTools.StateMachine
{
	public abstract class Window : MonoBehaviour
	{
		private AnchorAnimationGroup _animations;

		public AnchorAnimationGroup Animations => _animations ??= InitAnimations();
		public abstract Type WindowType { get; }

		private AnchorAnimationGroup InitAnimations()
		{
			var animators = GetComponentsInChildren<AnchorAnimator>();

			if (animators is { Length: 0 })
			{
				_animations = null;

				return null;
			}

			_animations = new AnchorAnimationGroup(animators);

			return _animations;
		}
	}
}