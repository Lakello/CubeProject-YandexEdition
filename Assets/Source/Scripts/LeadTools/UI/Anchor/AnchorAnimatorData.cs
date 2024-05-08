using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CubeProject.LeadTools.UI
{
	[Serializable]
	public class AnchorAnimatorData
	{
		[SerializeField] private Vector2 _anchorMin;
		[SerializeField] private Vector2 _anchorMax;
		[SerializeField] private bool _isAnimateOffset;
		[SerializeField] [ShowIf(nameof(_isAnimateOffset))] private Vector2 _offsetMin;
		[SerializeField] [ShowIf(nameof(_isAnimateOffset))] private Vector2 _offsetMax;
		[SerializeField] private float _duration;
		[SerializeField] private float _playDelay;
		[SerializeField] private Ease _ease = Ease.InOutBack;

		public Vector2 AnchorMin => _anchorMin;
		public Vector2 AnchorMax => _anchorMax;
		public bool IsAnimateOffset => _isAnimateOffset;
		public Vector2 OffsetMin => _offsetMin;
		public Vector2 OffsetMax => _offsetMax;
		public float Duration => _duration;
		public float PlayDelay => _playDelay;
		public Ease Ease => _ease;
	}
}