// using Plugins.Audio.Core;
// using Plugins.Audio.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPreferences : MonoBehaviour
{
	// [SerializeField] private SourceAudio _sourceAudio;
	// [SerializeField] private AudioDataProperty _clip;

	private IMusicHandler _musicHandler;
	private bool _isActiveMusic = false;

	private void Awake()
	{
		_musicHandler = GetComponent<IMusicHandler>();
	}

	private void OnEnable()
	{
		_musicHandler.Sound += ControlSound;
	}

	private void OnDisable()
	{
		_musicHandler.Sound -= ControlSound;
	}

	private void ControlSound()
	{
		// if (_isActiveMusic == false)
		// {
		// 	_sourceAudio.Play(_clip);
		// 	_isActiveMusic = true;
		// }
		// else if (_isActiveMusic == true)
		// {
		// 	_sourceAudio.Stop();
		// 	_isActiveMusic = false;
		// }       
	}
}