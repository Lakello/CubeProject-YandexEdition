using System;
using UnityEngine;
using UnityEngine.VFX;

namespace CubeProject.Game
{
	[Serializable]
	public class VFXHolder : EffectHolder
	{
		[SerializeField] private VisualEffect[] _visualEffects;

		protected override void Play()
		{
			foreach (var effect in _visualEffects)
			{
				effect.enabled = true;
			}
		}

		protected override void Stop()
		{
			foreach (var effect in _visualEffects)
			{
				effect.enabled = false;
			}
		}
	}
}