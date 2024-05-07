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
		[SerializeField] [BoxGroup("Play")] private bool _isPlayInAwake;
		[SerializeField]
		[BoxGroup("Play")]
		[ShowIf(nameof(_isPlayInAwake))]
		private AnchorAnimatorState _awakeState;
		[SerializeField]
		[BoxGroup("Play")]
		private float _playDelay;
		[SerializeField] private Ease _ease;
		[OdinSerialize] private Dictionary<AnchorAnimatorState, AnchorAnimatorData> _datas;

		private RectTransform _rect;
		private Sequence _sequence;

		private RectTransform Rect => _rect ??= gameObject.GetComponentElseThrow<RectTransform>();

		private void Awake()
		{
			if (_isPlayInAwake)
				Play(_awakeState);
		}

		[Button]
		public void Play(AnchorAnimatorState state)
		{
			_sequence?.Kill();

			_sequence = CreateAnimation(state);
			_sequence.Play();
		}

		public Sequence CreateAnimation(AnchorAnimatorState state)
		{
			var data = _datas[state];
			
			return DOTween
				.Sequence()
				.AppendInterval(_playDelay)
				.Append(Rect.DOAnchorMin(data.AnchorMin, data.Duration).SetEase(_ease))
				.Join(Rect.DOAnchorMax(data.AnchorMax, data.Duration).SetEase(_ease))
				.Join(DOTween
					.To(
						_ =>
						{
							Rect.offsetMin = data.OffsetMin;
							Rect.offsetMax = data.OffsetMax;
						},
						0,
						0,
						data.Duration)
					.SetEase(_ease))
				.Pause();
		}
	}
}