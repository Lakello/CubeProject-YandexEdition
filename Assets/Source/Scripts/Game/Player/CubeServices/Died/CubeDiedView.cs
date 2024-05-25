using System;
using LeadTools.Extensions;
using LeadTools.Other;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeDiedView : MonoBehaviour
	{
		private const float Visible = 0;
		private const float UnVisible = 1;

		[SerializeField] private float _animationTime;
		[SerializeField] private MeshRenderer _dissolveMeshRenderer;

		public void Play(bool isVisible, Action endCallback)
		{
			var (current, target) = isVisible
				? (UnVisible, Visible)
				: (Visible, UnVisible);

			this.PlaySmoothChangeValue(
				(currentTime) =>
				{
					var clipDissolve = Mathf.Lerp(current, target, currentTime);

					_dissolveMeshRenderer.material.SetFloat(ShaderProperty._Clip.GetCurrentName(), clipDissolve);
				},
				_animationTime,
				endCallback);
		}
	}
}