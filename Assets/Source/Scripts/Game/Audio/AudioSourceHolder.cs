using System;
using System.Collections;
using LeadTools.Extensions;
using LeadTools.Object;
using UnityEngine;

namespace Source.Scripts.Game
{
	public class AudioSourceHolder : MonoBehaviour, IPoolingObject<AudioSourceHolder, AudioInitData>
	{
		private AudioSource _source;

		public event Action<IPoolingObject<AudioSourceHolder, AudioInitData>> Disabled;

		public Type SelfType => typeof(AudioSourceHolder);
		public AudioSourceHolder Instance => this;
		
		private AudioSource Source => _source ??= gameObject.GetComponentElseThrow<AudioSource>();

		public void Init(AudioInitData init)
		{
			Source.clip = init.Clip;
			Source.loop = init.IsLoop;
			Source.Play();

			StartCoroutine(WaitEndPlaying());
		}

		private IEnumerator WaitEndPlaying()
		{
			yield return new WaitUntil(() => Source.isPlaying == false);
			gameObject.SetActive(false);
		}

		private void OnDisable() =>
			Disabled?.Invoke(this);
	}
}