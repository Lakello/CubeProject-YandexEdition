using System;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using DG.Tweening;
using LeadTools.Extensions;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(PowerUnit))]
	public class PowerUnitView : MonoBehaviour
	{
		[SerializeField] private float _enteredPositionY;
		[SerializeField] private float _animationDuration;
		[SerializeField] private Transform _buttonObject;
		
		private CubeMoveService _moveService;
		private PowerUnit _powerUnit;
		private Tweener _animationTweener;
		
		[Inject]
		private void Inject(Cube cube) =>
			_moveService = cube.Component.MoveService;

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _powerUnit);

		private void OnEnable() =>
			_powerUnit.Entered += OnEntered;

		private void OnDisable()
		{
			_powerUnit.Entered -= OnEntered;

			if (_animationTweener != null)
			{
				_animationTweener.Kill();
				_animationTweener = null;
			}
		}

		private void OnEntered()
		{
			_moveService.StepEnded += OnEnterStepEnded;

			if (_animationTweener == null)
			{
				_animationTweener = _buttonObject.DOMoveY(_enteredPositionY, _animationDuration).SetEase(Ease.InOutFlash);
			}
			else
			{
				_animationTweener.PlayForward();
			}
		}

		private void OnEnterStepEnded()
		{
			_moveService.StepEnded -= OnEnterStepEnded;
			_moveService.StepStarted += OnExitStepStarted;

			_animationTweener.Pause();
		}

		private void OnExitStepStarted()
		{
			_moveService.StepStarted -= OnExitStepStarted;
			
			_animationTweener.PlayBackwards();
		}
	}
}