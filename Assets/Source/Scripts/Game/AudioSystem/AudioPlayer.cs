using Ami.BroAudio;
using LeadTools.Extensions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace CubeProject.Game.AudioSystem
{
	public class AudioPlayer : SerializedMonoBehaviour
	{
		[SerializeField] private SoundID _soundID;
		[SerializeField] private bool _isPlayInStart;
		[SerializeField] private bool _isListenAudioSubject = true;
		[SerializeField] [ShowIf(nameof(_isListenAudioSubject))] private bool _isThisAudioSubject = true;
		[OdinSerialize] [HideIf(nameof(_isThisAudioSubject))] private IAudioSubject[] _subjects;

		private void Awake()
		{
			if (_isPlayInStart)
				OnAudioPlaying();

			if (_isListenAudioSubject && _isThisAudioSubject)
				gameObject.GetComponentsElseThrow(out _subjects);
		}

		private void OnEnable()
		{
			if (_isListenAudioSubject)
				_subjects.ForEach(source => source.AudioPlaying += OnAudioPlaying);
		}

		private void OnDisable()
		{
			if (_isListenAudioSubject)
				_subjects.ForEach(source => source.AudioPlaying -= OnAudioPlaying);
		}

		private void OnAudioPlaying() =>
			BroAudio.Play(_soundID);
	}
}