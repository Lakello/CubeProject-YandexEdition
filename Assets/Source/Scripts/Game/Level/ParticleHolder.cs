using System;
using UnityEngine;

namespace CubeProject.Game
{
	[Serializable]
	public class ParticleHolder : EffectHolder
	{
		[SerializeField] private ParticleSystem _particle;

		protected override void Play() =>
			_particle.Play();

		protected override void Stop() =>
			_particle.Stop();
	}
}