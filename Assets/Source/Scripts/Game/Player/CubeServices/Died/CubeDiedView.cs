using System;
using DG.Tweening;
using LeadTools.Extensions;
using LeadTools.Other;
using UnityEngine;

namespace Game.Player
{
	public class CubeDiedView : MonoBehaviour
	{
		private const float Visible = 0;
		private const float UnVisible = 1;

		[SerializeField] private float _animationTime;
		[SerializeField] private MeshRenderer _dissolveMeshRenderer;

		public void Play(bool isVisible, Action endCallback)
		{
			var target = isVisible ? Visible : UnVisible;

			_dissolveMeshRenderer.material
				.DOFloat(target, ShaderProperty._Clip.GetCurrentName(), _animationTime)
				.OnKill(() => endCallback?.Invoke());
		}
	}
}