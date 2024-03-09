using System;
using LeadTools.Extensions;
using UnityEngine;

namespace Source.Scripts.Game.Level.Wall
{
    [RequireComponent(typeof(MeshRenderer))]
	public class WallFade : MonoBehaviour
	{
		private const string Alpha = "_Alpha";
		private const float HideAlpha = 0f;
		private const float ShowAlpha = 0.5f;
		
		private MeshRenderer _renderer;

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _renderer);

		public void Show() =>
			_renderer.material.SetFloat(Alpha, ShowAlpha);

		public void Hide() =>
			_renderer.material.SetFloat(Alpha, HideAlpha);
	}
}