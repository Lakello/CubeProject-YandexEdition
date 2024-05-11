using LeadTools.Extensions;
using Plugins.Audio.Core;
using Plugins.Audio.Utils;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
	[SerializeField] private SourceAudio _sourceAudio;
	[SerializeField] private AudioDataProperty _clip;

	private IAudioSource _source;

	private void Awake() =>
		gameObject.GetComponentElseThrow(out _source);

	private void OnEnable() =>
		_source.AudioPlaying += AudioPlaying;

	private void OnDisable() =>
		_source.AudioPlaying -= AudioPlaying;

	private void AudioPlaying() =>
		_sourceAudio.Play(_clip);
}