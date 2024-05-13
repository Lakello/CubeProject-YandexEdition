using LeadTools.Extensions;
using Plugins.Audio.Core;
using Plugins.Audio.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
	[SerializeField] private bool _isThisSourceAudio = true;
	[SerializeField] [HideIf(nameof(_isThisSourceAudio))] private SourceAudio _sourceAudio;
	[SerializeField] private AudioDataProperty _clip;
	[SerializeField] private bool _isPlayInAwake;
	[SerializeField] private bool _isListenAudioSource = true;

	private IAudioSource _source;

	private void Awake()
	{
		if (_isPlayInAwake)
			OnAudioPlaying();

		if (_isThisSourceAudio 
			&& TryGetComponent(out _sourceAudio) == false)
		{
			_sourceAudio = gameObject.AddComponent<SourceAudio>();
		}
		
		if (_isListenAudioSource)
			gameObject.GetComponentElseThrow(out _source);
	}

	private void OnEnable()
	{
		if (_isListenAudioSource)
			_source.AudioPlaying += OnAudioPlaying;
	}

	private void OnDisable()
	{
		if (_isListenAudioSource)
			_source.AudioPlaying -= OnAudioPlaying;
	}

	private void OnAudioPlaying() =>
		_sourceAudio.Play(_clip);
}