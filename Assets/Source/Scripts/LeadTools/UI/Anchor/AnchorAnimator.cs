using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using LeadTools.Extensions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CubeProject.LeadTools.UI
{
	[ExecuteInEditMode]
	public class AnchorAnimator : SerializedMonoBehaviour
	{
		[BoxGroup("Init")]
		[SerializeField] private bool _isInitInAwake;
		[BoxGroup("Init")]
		[SerializeField] private bool _isPlayInInit;
		[BoxGroup("Init")] [ShowIf(nameof(_isPlayInInit))]
		[SerializeField] private AnchorAnimatorState _initState;
		[BoxGroup("Init")]
		[SerializeField] private bool _isJumpStateInInit;
		[BoxGroup("Init")] [ShowIf(nameof(_isJumpStateInInit))]
		[SerializeField] private AnchorAnimatorState _jumpState;

		[OdinSerialize] private Dictionary<AnchorAnimatorState, AnchorAnimatorData> _animatorsData =
			new Dictionary<AnchorAnimatorState, AnchorAnimatorData>
			{
				[AnchorAnimatorState.Initial] = new AnchorAnimatorData(),
				[AnchorAnimatorState.Target] = new AnchorAnimatorData(),
			};

		[BoxGroup("Anchor")]
		[SerializeField] private bool _isSetAllStates;
		[BoxGroup("Anchor")] [HideIf(nameof(_isSetAllStates))]
		[SerializeField] private AnchorAnimatorState _settingAnchorState;
		[BoxGroup("Anchor")]
		[SerializeField] private bool _isUseThisRect;
		[BoxGroup("Anchor")] [HideIf(nameof(_isUseThisRect))]
		[SerializeField] private RectTransform _settingTargetRect;

		private RectTransform _rect;
		private Sequence _sequence;
 
		private RectTransform Rect => _rect ??= gameObject.GetComponentElseThrow<RectTransform>();
		private RectTransform SettingTargetRect => _isUseThisRect
			? Rect
			: _settingTargetRect;

		private void Awake()
		{
			if (_isInitInAwake == false)
				return;

			Init();
		}

		public void Init()
		{
			if (_isJumpStateInInit)
				JumpTo(_jumpState);

			if (_isPlayInInit)
				Play(_initState);
		}

		public void Play(AnchorAnimatorState state)
		{
			_sequence?.Kill();

			_sequence = CreateAnimation(state);
			_sequence.Play();
		}

		public Sequence CreateAnimation(AnchorAnimatorState state)
		{
			var data = _animatorsData[state];

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
			var data = _animatorsData[state];

			Rect.DOAnchorMin(data.AnchorMin, 0);
			Rect.DOAnchorMax(data.AnchorMax, 0);

			if (data.IsAnimateOffset)
			{
				Rect.offsetMin = data.OffsetMin;
				Rect.offsetMax = data.OffsetMax;
			}
		}

		[Button] [BoxGroup("Anchor")]
		private void SetAnchorData()
		{
			var targetRect = SettingTargetRect;

			if (_isSetAllStates)
				_animatorsData.Values.ForEach(Set);
			else
				Set(_animatorsData[_settingAnchorState]);

			return;

			void Set(AnchorAnimatorData data)
			{
				data.AnchorMin = targetRect.anchorMin;
				data.AnchorMax = targetRect.anchorMax;

				EditorUtility.SetDirty(this);
			}
		}
	}
}