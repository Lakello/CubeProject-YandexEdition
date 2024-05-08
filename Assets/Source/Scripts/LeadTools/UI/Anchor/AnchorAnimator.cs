using System.Collections.Generic;
using DG.Tweening;
using LeadTools.Extensions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace CubeProject.LeadTools.UI
{
	[ExecuteInEditMode]
	public class AnchorAnimator : SerializedMonoBehaviour
	{
		[BoxGroup("Awake")]
		[SerializeField] private bool _isPlayInAwake;
		[BoxGroup("Awake")] [ShowIf(nameof(_isPlayInAwake))]
		[SerializeField] private AnchorAnimatorState _awakeState;
		[BoxGroup("Awake")]
		[SerializeField] private bool _isSetInitialStateInAwake;
		[BoxGroup("Awake")] [ShowIf(nameof(_isSetInitialStateInAwake))]
		[SerializeField] private AnchorAnimatorState _initialState;
		[OdinSerialize] private Dictionary<AnchorAnimatorState, AnchorAnimatorData> _datas;

		private RectTransform _rect;
		private Sequence _sequence;

		private RectTransform Rect => _rect ??= gameObject.GetComponentElseThrow<RectTransform>();

		private void Awake()
		{
			if (_isSetInitialStateInAwake)
				JumpTo(_initialState);

			if (_isPlayInAwake)
				Play(_awakeState);
		}

		public void Play(AnchorAnimatorState state)
		{
			_sequence?.Kill();

			_sequence = CreateAnimation(state);
			_sequence.Play();
		}

		public Sequence CreateAnimation(AnchorAnimatorState state)
		{
			var data = _datas[state];

			var sequence = DOTween
				.Sequence()
				.AppendInterval(data.PlayDelay)
				.Append(Rect.DOAnchorMin(data.AnchorMin, data.Duration).SetEase(data.Ease))
				.Join(Rect.DOAnchorMax(data.AnchorMax, data.Duration).SetEase(data.Ease));

			if (data.IsAnimateOffset)
				_sequence.Join(DOTween
					.To(
						_ =>
						{
							Rect.offsetMin = data.OffsetMin;
							Rect.offsetMax = data.OffsetMax;
						},
						0,
						0,
						data.Duration)
					.SetEase(data.Ease));

			return sequence.Pause();
		}

		[Button]
		private void JumpTo(AnchorAnimatorState state)
		{
			var data = _datas[state];

			Rect.anchorMin = data.AnchorMin;
			Rect.anchorMax = data.AnchorMax;

			if (data.IsAnimateOffset)
			{
				Rect.offsetMin = data.OffsetMin;
				Rect.offsetMax = data.OffsetMax;
			}
		}
	}
}