using System;
using LeadTools.UI.Anchor;
using UnityEngine;

namespace LeadTools.FSM.WindowFSM
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