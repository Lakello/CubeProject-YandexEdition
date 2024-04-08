using LeadTools.Extensions;
using LeadTools.Other;
using UnityEngine;

namespace Source.Scripts.Game.Level.Wall
{
	public class WallFade : MonoBehaviour
	{
		private const float HideAlpha = 0f;
		private const float ShowAlpha = 0.5f;
		private const float Duration = 0.2f;

		[SerializeField] private ShaderProperty _property = ShaderProperty._Color;
		[SerializeField] private MeshRenderer[] _renderers;

		private Color _materialColor;
		private Coroutine _alphaCoroutine;

		private void Awake()
		{
			if (_renderers is null)
				gameObject.GetComponentElseThrow(out _renderers);

			foreach (var meshRenderer in _renderers)
			{
				var color = meshRenderer.material.GetColor(_property.GetCurrentName());

				color.a = HideAlpha;
				_materialColor = color;
			}

			Hide();
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
						currentTime = 1 - currentTime;

					var color = _materialColor;

					color.a = Mathf.Lerp(HideAlpha, ShowAlpha, currentTime);

					foreach (var meshRenderer in _renderers)
						meshRenderer.material.SetColor(_property.GetCurrentName(), color);
				},
				Duration);
		}
	}
}