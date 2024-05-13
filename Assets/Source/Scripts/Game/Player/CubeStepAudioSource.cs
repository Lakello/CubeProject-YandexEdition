using System;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using Reflex.Attributes;
using UnityEngine;

namespace Source.Scripts.Game
{
	public class CubeStepAudioSource : MonoBehaviour, IAudioSource
	{
		private CubeMoveService _moveService;

		public event Action AudioPlaying;

		[Inject]
		private void Inject(Cube cube)
		{
			_moveService = cube.Component.MoveService;

			_moveService.StepEnded += OnStepEnded;
		}

		private void OnDisable() =>
			_moveService.StepEnded -= OnStepEnded;

		private void OnStepEnded() =>
			AudioPlaying?.Invoke();
	}
}