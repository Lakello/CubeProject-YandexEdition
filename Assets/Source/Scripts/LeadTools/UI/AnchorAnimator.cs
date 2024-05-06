using System;
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
		[SerializeField] [BoxGroup("PlayInAwake")] private bool _isPlayInAwake;
		[SerializeField]
		[BoxGroup("PlayInAwake")]
		[ShowIf(nameof(_isPlayInAwake))]
		private AnchorAnimatorState _awakeState;
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
			var data = _datas[state];

			_sequence?.Kill();

			_sequence = DOTween
				.Sequence()
				.Append(Rect.DOAnchorMin(data.AnchorMin, data.Duration).SetEase(_ease))
				.Join(Rect.DOAnchorMax(data.AnchorMax, data.Duration).SetEase(_ease))
				.Join(DOTween.To(
					_ =>
					{
						Rect.offsetMin = data.OffsetMin;
						Rect.offsetMax = data.OffsetMax;
					},
					0,
					0,
					data.Duration)
					.SetEase(_ease));
		}
	}
}