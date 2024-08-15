using System;
using Ami.BroAudio;
using UniRx;
using UnityEngine;

namespace Game.Player
{
	public class BackgroundAudioSource : MonoBehaviour
	{
		[SerializeField] private SoundID[] _soundIds;
		[SerializeField] private float _durationClip;

		private int _current;

		private void Awake()
		{
			BroAudio.Play(_soundIds[_current]);

			Observable.Timer(TimeSpan.FromSeconds(_durationClip))
				.Repeat()
				.Subscribe(_ => BroAudio.Play(GetNextSound()))
				.AddTo(this);
		}

		private SoundID GetNextSound()
		{
			_current++;

			if (_current >= _soundIds.Length)
				_current = 0;

			return _soundIds[_current];
		}
	}
}