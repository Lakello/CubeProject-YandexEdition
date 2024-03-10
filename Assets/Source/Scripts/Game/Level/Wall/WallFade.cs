using LeadTools.Extensions;
using UnityEngine;

namespace Source.Scripts.Game.Level.Wall
{
	[RequireComponent(typeof(MeshRenderer))]
	public class WallFade : MonoBehaviour
	{
		private const string ColorProperty = "_Color";
		private const float HideAlpha = 0f;
		private const float ShowAlpha = 0.5f;
		private const float Duration = 0.2f;

		private MeshRenderer _renderer;
		private Color _materialColor;
		private Coroutine _alphaCoroutine;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _renderer);
			var color = _renderer.material.GetColor(ColorProperty);

			color.a = HideAlpha;
			_materialColor = color;
		}

		public void Show() =>
			ChangeAlpha(true);

		public void Hide() =>
			ChangeAlpha(false);

		private void ChangeAlpha(bool isShow)
		{
			this.StopRoutine(_alphaCoroutine);

			this.PlaySmoothChangeValue(
				(currentTime) =>
				{
					if (isShow is false)
					{
						currentTime = 1 - currentTime;
					}
					
					var color = _materialColor;

					color.a = Mathf.Lerp(HideAlpha, ShowAlpha, currentTime);

					_renderer.material.SetColor(ColorProperty, color);
				},
				Duration);
		}
	}
}