using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LeadTools.UI.Anchor
{
	[Serializable]
	public class AnchorAnimatorData
	{
		public Vector2 AnchorMin;
		public Vector2 AnchorMax;
		public bool IsAnimateOffset;
		[ShowIf(nameof(IsAnimateOffset))] public Vector2 OffsetMin;
		[ShowIf(nameof(IsAnimateOffset))] public Vector2 OffsetMax;
		public float Duration;
		public float PlayDelay;
		public Ease Ease = Ease.InOutBack;
	}
}