using System;
using DG.Tweening;
using LeadTools.Common;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game.Player.CubeService.Died
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

			ShortcutExtensions.DOFloat(
					_dissolveMeshRenderer.material,
					target,
					(string)ShaderProperty._Clip.GetCurrentName(),
					_animationTime)
				.OnKill(() => endCallback?.Invoke());
		}
	}
}