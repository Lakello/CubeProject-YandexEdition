using System;
using System.Collections;
using DG.Tweening;
using LeadTools.Extensions;
using LeadTools.Object;
using UnityEngine;

namespace Source.Scripts.Game
{
	public class AudioSourceHolder : MonoBehaviour, IPoolingObject<AudioSourceHolder, AudioInitData>
	{
		[SerializeField] private bool _isSmoothChangingVolume;
		[SerializeField] private float _changingDuration;
		
		private AudioSource _source;

		public event Action<IPoolingObject<AudioSourceHolder, AudioInitData>> Disabled;

		public Type SelfType => typeof(AudioSourceHolder);
		public AudioSourceHolder Instance => this;

		private AudioSource Source => _source ??= gameObject.GetComponentElseThrow<AudioSource>();

		private void OnDisable() =>
			Disabled?.Invoke(this);

		public void Disable() =>
			SetVolume(0, () => Source.Stop());

		public void Init(AudioInitData init)
		{
			Source.clip = init.Clip;
			Source.loop = init.IsLoop;
			Source.Play();

			SetVolume(init.Volume);
			
			if (init.IsLoop == false)
				StartCoroutine(DisableAfterPlaying());
		}

		private void SetVolume(float target, Action endCallback = null)
		{
			if (_isSmoothChangingVolume)
			{
				DOTween.To(
					progress => Source.volume = progress,
					Source.volume,
					target,
					_changingDuration)
					.OnKill(() => endCallback?.Invoke());
			}
			else
			{
				Source.volume = target;
				endCallback?.Invoke();
			}
		}

		private IEnumerator DisableAfterPlaying()
		{
			yield return new WaitUntil(() => Source.isPlaying == false);
			gameObject.SetActive(false);
		}
	}
}