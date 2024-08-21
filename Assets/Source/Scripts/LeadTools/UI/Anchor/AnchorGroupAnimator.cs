using System;
using DG.Tweening;

namespace LeadTools.UI.Anchor
{
	public class AnchorGroupAnimator
	{
		private readonly AnchorAnimationGroup _group;

		private Sequence _sequence;

		public AnchorGroupAnimator(AnchorAnimationGroup group) =>
			_group = group;

		public void PlayAnimations(
			AnchorAnimatorState state,
			Action completeCallback = null)
		{
			if (_group == null)
			{
				completeCallback?.Invoke();

				return;
			}

			if (_sequence != null)
				_sequence.onKill = null;

			_sequence = DOTween.Sequence().Pause();

			var sequences = _group.CreateAnimations(state);

			if (sequences == null || sequences.Length < 1)
			{
				completeCallback?.Invoke();

				return;
			}

			_sequence.Append(sequences[0]);

			if (sequences.Length > 1)
			{
				for (int i = 1; i < sequences.Length; i++)
					_sequence.Join(sequences[i]);
			}

			_sequence.OnKill(() => completeCallback?.Invoke());
			_sequence.Play();
		}
	}
}