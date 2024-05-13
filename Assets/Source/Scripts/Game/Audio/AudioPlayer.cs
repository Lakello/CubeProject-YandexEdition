using LeadTools.Extensions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

public class AudioPlayer : SerializedMonoBehaviour
{
	[SerializeField] private bool _isThisSourceAudio = true;
	[SerializeField] [HideIf(nameof(_isThisSourceAudio))] private AudioSource _audioSource;
	[SerializeField] private AudioClip _clip;
	[SerializeField] private bool _isPlayInAwake;
	[SerializeField] private bool _isListenAudioSource = true;
	[SerializeField] [ShowIf(nameof(_isListenAudioSource))] private bool _isThisAudioSource = true;
	[OdinSerialize] [HideIf(nameof(_isThisAudioSource))] private IAudioSource[] _sources;

	private void Awake()
	{
		if (_isPlayInAwake)
			OnAudioPlaying();

		if (_isThisSourceAudio && TryGetComponent(out _audioSource) == false)
			_audioSource = gameObject.AddComponent<AudioSource>();

		if (_isListenAudioSource && _isThisAudioSource)
			gameObject.GetComponentsElseThrow(out _sources);
	}

	private void OnEnable()
	{
		if (_isListenAudioSource)
			_sources.ForEach(source => source.AudioPlaying += OnAudioPlaying);
	}

	private void OnDisable()
	{
		if (_isListenAudioSource)
			_sources.ForEach(source => source.AudioPlaying -= OnAudioPlaying);
	}

	private void OnAudioPlaying()
	{
		_audioSource.Pause();
		_audioSource.clip = _clip;
		_audioSource.Play();
	}
}