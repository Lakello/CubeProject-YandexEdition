using LeadTools.Extensions;
using LeadTools.Object;
using Reflex.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Source.Scripts.Game;
using UnityEngine;

public class AudioPlayer : SerializedMonoBehaviour
{
	[SerializeField] private AudioClip _clip;
	[SerializeField] private bool _isPlayAfterInit;
	[SerializeField] private bool _isListenAudioSource = true;
	[SerializeField] [ShowIf(nameof(_isListenAudioSource))] private bool _isThisAudioSource = true;
	[OdinSerialize] [HideIf(nameof(_isThisAudioSource))] private IAudioSource[] _sources;

	private ObjectSpawner<AudioSourceHolder, AudioInitData> _audioSpawner;
	private AudioInitData _data;

	[Inject]
	private void Inject(ObjectSpawner<AudioSourceHolder, AudioInitData> audioSpawner)
	{
		_audioSpawner = audioSpawner;

		_data = new AudioInitData
		{
			Clip = _clip
		};

		if (_isPlayAfterInit)
			OnAudioPlaying();

		if (_isListenAudioSource && _isThisAudioSource)
			gameObject.GetComponentsElseThrow(out _sources);
		
		Subscribe();
	}

	private void OnEnable() =>
		Subscribe();

	private void Subscribe()
	{
		if (_isListenAudioSource && _audioSpawner != null)
			_sources.ForEach(source => source.AudioPlaying += OnAudioPlaying);
	}

	private void OnDisable()
	{
		if (_isListenAudioSource)
			_sources.ForEach(source => source.AudioPlaying -= OnAudioPlaying);
	}

	private void OnAudioPlaying()
	{
		Debug.Log("Spawn");
		var audio = _audioSpawner.Spawn(_data);
		
		Debug.Log(audio != null);
	}
}