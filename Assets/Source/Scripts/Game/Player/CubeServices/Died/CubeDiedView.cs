using System;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeDiedView : MonoBehaviour
	{
		private const float Visible = 0;
		private const float UnVisible = 1;
		private const string Clip = "_Clip";

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

					_dissolveMeshRenderer.material.SetFloat(Clip, clipDissolve);
				},
				_animationTime,
				endCallback);
		}
	}
}